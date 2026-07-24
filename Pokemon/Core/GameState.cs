using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Pokemon.Models;

namespace Pokemon.Core
{
  // ViewModel에서 동작을 제어하는데 사용
  public enum BattlePhase
  {
    Start,            // 전투 시작, 포켓몬을 고르는 단계
    WaitingForInput,  // 유저 선택 대기
    TurnProcessing,   // 계산 중
    BattleEnd         // 전투 종료
  }

  public class TurnOutcome
  {
    public bool BattleEnded { get; init; }
    public bool RequiresPlayerSwitch { get; init; }
    public Trainer? Winner { get; init; }
  }

  /// <summary>
  /// 전투 진행 중 발생하는 사건 하나하나. UI에서 순서대로(일정 시간마다 다음 것) 재생하는 용도.
  /// </summary>
  public abstract record BattleEvent
  {
    public sealed record Message(string Text) : BattleEvent;

    public sealed record DamageDealt(bool TargetIsPlayer, int NewHp) : BattleEvent; 

    public sealed record HpRecovered(bool TargetIsPlayer, int NewHp) : BattleEvent;

    public sealed record PokemonSwitched(bool IsPlayerSide, int HpAtSwitch) : BattleEvent;

    public sealed record AttackEffect(bool AttackerIsPlayer, PokemonType SkillType) : BattleEvent;
  }

  public class BattleState
  {
    public int Turn { get; set; } // odd Player, even Opposite
    public Trainer Player { get; set; }
    public Trainer Opposite { get; set; }
    public Pokemon.Models.Pokemon PlayerPokemon { get; set; }
    public Pokemon.Models.Pokemon OppPokemon { get; set; }
    public BattlePhase BattlePhase { get; set; }
    public List<BattleEvent> Events { get; } = new();

    /// <summary>
    /// ViewModel에서 순서대로 재생할 메시지 이벤트 추가
    /// </summary>
    /// <param name="message">표시할 문자열</param>
    public void AddLog(string message)
    {
      Events.Add(new BattleEvent.Message(message));
    }

    public BattleState(Trainer user, Trainer opposite)
    {
      Player = user;
      Opposite = opposite;

      // 무한 모드에서는 이전 전투에서 1번 포켓몬이 기절한 채로 다음 전투에 넘어올 수 있으므로,
      // 항상 첫 번째 "살아있는" 포켓몬을 선택함(단순히 Pokemons[0]을 쓰면 안 됨).
      PlayerPokemon = user.AvailablePokemons.FirstOrDefault()
        ?? throw new InvalidOperationException("사용 가능한 플레이어 포켓몬이 없습니다.");
      OppPokemon = opposite.AvailablePokemons.FirstOrDefault()
        ?? throw new InvalidOperationException("상대가 사용 가능한 포켓몬이 없습니다.");

      BattlePhase = BattlePhase.Start;
    }
  }

  public class BattleEngine
  {
    private readonly BattleState state;

    public BattleState State => state;

    public BattleEngine(BattleState _state)
    {
      this.state = _state;
    }

    public int CalculateDamage(Pokemon.Models.Pokemon src, Skill skill, Pokemon.Models.Pokemon dst)
    {
      double multiplier = TypeChart.GetMultiplier(skill.Type, dst.Type);
      int raw = skill.Power + src.Attack - dst.Defense;
      int damage = (int)Math.Round(raw * multiplier);
      return Math.Max(1, damage);
    }

    public void ExecuteAttack(Pokemon.Models.Pokemon src, Skill skill, Pokemon.Models.Pokemon dst)
    {
      bool targetIsPlayer = dst == state.PlayerPokemon;
      bool attackerIsPlayer = src == state.PlayerPokemon;

      state.AddLog($"{src.Name}의 {skill.Name}!");
      state.Events.Add(new BattleEvent.AttackEffect(attackerIsPlayer, skill.Type));

      int damage = CalculateDamage(src, skill, dst);
      dst.TakeDamage(damage);

      state.Events.Add(new BattleEvent.DamageDealt(targetIsPlayer, dst.CurrentHp));
      state.AddLog($"{dst.Name}은(는) {damage}의 피해를 입었다!");

      double effectiveness =
        TypeChart.GetMultiplier(skill.Type, dst.Type);

      if (effectiveness > 1.0)
      {
        state.AddLog("효과가 굉장했다!");
      }
      else if (effectiveness < 1.0)
      {
        state.AddLog("효과가 별로인 듯하다...");
      }

      if (dst.IsFainted)
      {
        state.AddLog($"{dst.Name}은(는) 쓰러졌다!");
      }
    }

    public bool CheckBattleEnd(out Trainer? winner)
    {
      winner = null;
      if (state.Opposite.IsDefeated)
      {
        winner = state.Player;
        return true;
      }
      if (state.Player.IsDefeated)
      {
        winner = state.Opposite;
        return true;
      }
      return false;
    }

    /// <summary>
    /// 상대 AI 턴: 살아있는 포켓몬이 랜덤 기술로 공격만 함(교체 없음)
    /// </summary>
    public void OppositeAction()
    {
      List<Skill> moves = state.OppPokemon.Skills.ToList();
      Skill selectedMove = moves[Random.Shared.Next(moves.Count)];
      ExecuteAttack(state.OppPokemon, selectedMove, state.PlayerPokemon);
      state.Turn++;
    }

    private void ValidatePlayerSwitch(Pokemon.Models.Pokemon selected)
    {
      if (!state.Player.Pokemons.Contains(selected))
      {
        throw new InvalidOperationException("보유하지 않은 포켓몬입니다.");
      }

      if (selected.IsFainted)
      {
        throw new InvalidOperationException("기절한 포켓몬은 교체할 수 없습니다.");
      }

      if (ReferenceEquals(selected, state.PlayerPokemon))
      {
        throw new InvalidOperationException("현재 전투 중인 포켓몬입니다.");
      }
    }

    /// <summary>
    /// 현재 포켓몬이 살아 있으면 일반 교체 후 상대가 공격하고,
    /// 기절한 상태이면 강제 교체만 하고 상대는 추가 공격하지 않는다.
    /// </summary>
    public TurnOutcome PerformPlayerSwitch(Pokemon.Models.Pokemon selected)
    {
      bool isForcedSwitch = state.PlayerPokemon.IsFainted;

      ValidatePlayerSwitch(selected);

      state.PlayerPokemon = selected;
      state.Events.Add(new BattleEvent.PokemonSwitched(
        IsPlayerSide: true,
        HpAtSwitch: selected.CurrentHp));
      state.AddLog($"가라! {selected.Name}!");

      if (isForcedSwitch)
      {
        return new TurnOutcome { BattleEnded = false };
      }

      state.Turn++;
      OppositeAction();

      if (!state.PlayerPokemon.IsFainted)
      {
        return new TurnOutcome { BattleEnded = false };
      }

      if (!state.Player.AvailablePokemons.Any())
      {
        CheckBattleEnd(out var winner);
        return new TurnOutcome { BattleEnded = true, Winner = winner };
      }

      state.AddLog("교체할 포켓몬을 선택하세요.");
      return new TurnOutcome
      {
        BattleEnded = false,
        RequiresPlayerSwitch = true
      };
    }

    /// <summary>
    /// 플레이어가 스킬을 선택했을 때의 한 턴 전체 처리(공격 -> 기절 처리 -> 상대 턴 -> 기절 처리 -> 종료 판정)
    /// </summary>
    public TurnOutcome PerformPlayerTurn(Skill skill)
    {
      state.Turn++;
      ExecuteAttack(state.PlayerPokemon, skill, state.OppPokemon);

      if (state.OppPokemon.IsFainted)
      {
        var next = state.Opposite.AvailablePokemons.FirstOrDefault();
        if (next is null)
        {
          CheckBattleEnd(out var winner);
          return new TurnOutcome { BattleEnded = true, Winner = winner };
        }
        state.OppPokemon = next;
        state.Events.Add(new BattleEvent.PokemonSwitched(
          IsPlayerSide: false,
          HpAtSwitch: next.CurrentHp));
        state.AddLog(
          $"{state.Opposite.Name}{KoreanParticle.Topic(state.Opposite.Name)} " +
          $"{next.Name}{KoreanParticle.Object(next.Name)} 내보냈다!");

        // 상대가 쓰러져 새 포켓몬을 내보낸 턴은 여기서 종료하고, 상대는 공격하지 않음(새 턴은 플레이어부터 시작)
        return new TurnOutcome { BattleEnded = false };
      }

      OppositeAction();

      if (state.PlayerPokemon.IsFainted)
      {
        if (!state.Player.AvailablePokemons.Any())
        {
          CheckBattleEnd(out var winner);
          return new TurnOutcome { BattleEnded = true, Winner = winner };
        }

        state.AddLog("교체할 포켓몬을 선택하세요.");
        return new TurnOutcome
        {
          BattleEnded = false,
          RequiresPlayerSwitch = true
        };
      }

      return new TurnOutcome { BattleEnded = false };
    }

    /// <summary>
    /// 플레이어가 전투 중 도구를 사용했을 때의 한 턴 전체 처리(효과 발동 -> 상대 턴 -> 기절 처리 -> 종료 판정)
    /// </summary>
    public TurnOutcome PerformPlayerItemTurn(Item item)
    {
      state.Turn++;
      state.AddLog($"{item.Name}을(를) 사용했다!");

      switch (item.EffectType)
      {
        case ItemEffectType.HealHp:
          int recoveredHp = state.PlayerPokemon.Heal(item.Value);
          state.Events.Add(new BattleEvent.HpRecovered(TargetIsPlayer: true, NewHp: state.PlayerPokemon.CurrentHp));
          state.AddLog($"{state.PlayerPokemon.Name}의 HP가 {recoveredHp} 회복되었다!");
          break;
      }

      OppositeAction();

      if (state.PlayerPokemon.IsFainted)
      {
        if (!state.Player.AvailablePokemons.Any())
        {
          CheckBattleEnd(out var winner);
          return new TurnOutcome { BattleEnded = true, Winner = winner };
        }

        state.AddLog("교체할 포켓몬을 선택하세요.");
        return new TurnOutcome
        {
          BattleEnded = false,
          RequiresPlayerSwitch = true
        };
      }

      return new TurnOutcome { BattleEnded = false };
    }
  }

  public static class Game
  {
    public static GameState State { get; } = new GameState("도전자");
  }

  public class GameState
  {
    public Trainer Player { get; set; }

    public BattleEngine? CurrentBattle { get; set; } // 현재 전투 중이라면 존재하고, 탐색 중이라면 null

    /// <summary>
    /// 플레이어의 소지 도구(Key = Item, Value = 보유 개수). 테스트용으로 HP Potion 3개를 가지고 시작함.
    /// </summary>
    public Dictionary<Item, int> Inventory { get; } = new()
    {
      { ItemDB.HpPotion, 3 }
    };

    private readonly HashSet<int> defeatedBossIds = new();

    public const int TotalBossCount = 4;

    public bool AllBossesDefeated => defeatedBossIds.Count >= TotalBossCount;

    public bool IsBossDefeated(int bossNumber) => defeatedBossIds.Contains(bossNumber);

    public void MarkBossDefeated(int bossNumber) => defeatedBossIds.Add(bossNumber);

    public GameState(string playerName)
    {
      Player = TrainerDB.CreatePlayer(playerName);
    }

    public int? CurrentBossNumber { get; set; }

    /// <summary>
    /// 무한 모드에서 현재 몇 번째 트레이너와 싸우고 있는지. Run 시작 전에는 0.
    /// </summary>
    public int CurrentTrainerNumber { get; private set; }

    public void BattleStart(Trainer opponent, int bossNumber)
    {
      // 새 전투마다 주인공 로스터를 새로 만들어 이전 전투의 HP가 이어지지 않게 함
      Player = TrainerDB.CreatePlayer(Player.Name);
      CurrentBossNumber = bossNumber;
      CurrentTrainerNumber = 0; // 보스전은 무한 모드가 아니므로 이전 Run 상태가 남아있지 않도록 정리
      BattleState battleState = new BattleState(Player, opponent);
      CurrentBattle = new BattleEngine(battleState);
    }

    /// <summary>
    /// 무한 모드 새 Run을 시작함. 여기서만 플레이어를 새로 생성하며,
    /// 이후 StartNextTrainer로 넘어갈 때는 Player를 그대로 유지해서 HP/기절 상태/가방이 이어짐.
    /// </summary>
    public void StartNewRun()
    {
      Player = TrainerDB.CreatePlayer(Player.Name);
      Inventory.Clear();
      Inventory[ItemDB.HpPotion] = 3;
      CurrentBossNumber = null;
      CurrentTrainerNumber = 1;
      CreateCurrentTrainerBattle();
    }

    /// <summary>
    /// 현재 트레이너를 쓰러뜨리고 보상을 받은 뒤 호출. Player는 재생성하지 않음.
    /// </summary>
    public void StartNextTrainer()
    {
      CurrentTrainerNumber++;
      CreateCurrentTrainerBattle();
    }

    private void CreateCurrentTrainerBattle()
    {
      Trainer opponent = TrainerDB.CreateRandomTrainer(CurrentTrainerNumber);
      BattleState battleState = new BattleState(Player, opponent);
      CurrentBattle = new BattleEngine(battleState);
    }

    /// <summary>
    /// 무한 모드 보상 등으로 도구를 획득했을 때 호출. 이미 있으면 개수만 늘림.
    /// </summary>
    public void AddItem(Item item, int amount = 1)
    {
      if (amount <= 0)
      {
        return;
      }

      Inventory[item] = GetItemCount(item) + amount;
    }

    public int GetItemCount(Item item) =>
      Inventory.TryGetValue(item, out int count) ? count : 0;

    /// <summary>
    /// 해당 도구가 하나 이상 있으면 1개를 소비하고 true, 없으면 false를 반환함.
    /// </summary>
    public bool ConsumeItem(Item item)
    {
      int count = GetItemCount(item);
      if (count <= 0)
      {
        return false;
      }

      Inventory[item] = count - 1;
      return true;
    }

    /// <summary>
    /// UI에서 도구 버튼을 눌렀을 때 호출. 전투 중이 아니거나 소지하지 않은 도구면 null 반환.
    /// </summary>
    public TurnOutcome? UseBattleItem(Item item)
    {
      if (CurrentBattle is null)
      {
        return null;
      }

      if (!ConsumeItem(item))
      {
        return null;
      }

      return CurrentBattle.PerformPlayerItemTurn(item);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Pokemon.Models;

namespace Pokemon
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
    public Trainer? Winner { get; init; }
  }

  public class BattleState
  {
    public int Turn { get; set; } // odd Player, even Opposite
    public Trainer Player { get; set; }
    public Trainer Opposite { get; set; }
    public Pokemon.Models.Pokemon PlayerPokemon { get; set; }
    public Pokemon.Models.Pokemon OppPokemon { get; set; }
    public BattlePhase BattlePhase { get; set; }
    public List<string> BattleLog { get; set; }

    /// <summary>
    /// ViewModel에서 Binding할 List
    /// </summary>
    /// <param name="message">표시할 문자열</param>
    public void AddLog(string message)
    {
      BattleLog.Add(message);
    }

    public BattleState(Trainer user, Trainer opposite)
    {
      Player = user;
      Opposite = opposite;
      PlayerPokemon = user.Pokemons[0];
      OppPokemon = opposite.Pokemons[0];
      BattlePhase = BattlePhase.Start;
      BattleLog = new();
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
      int damage = CalculateDamage(src, skill, dst);
      dst.TakeDamage(damage);

      string logMessage = $"{src.Name}의 {skill.Name}! {dst.Name}에게 {damage}의 피해!";
      state.AddLog(logMessage);

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
        state.AddLog($"상대가 {next.Name}을(를) 내보냈다!");
        state.OppPokemon = next;
      }

      OppositeAction();

      if (state.PlayerPokemon.IsFainted)
      {
        var next = state.Player.AvailablePokemons.FirstOrDefault();
        if (next is null)
        {
          CheckBattleEnd(out var winner);
          return new TurnOutcome { BattleEnded = true, Winner = winner };
        }
        state.AddLog($"가라! {next.Name}!");
        state.PlayerPokemon = next;
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

    public void BattleStart(Trainer opponent, int bossNumber)
    {
      // 새 전투마다 주인공 로스터를 새로 만들어 이전 전투의 HP가 이어지지 않게 함
      Player = TrainerDB.CreatePlayer(Player.Name);
      CurrentBossNumber = bossNumber;
      BattleState battleState = new BattleState(Player, opponent);
      CurrentBattle = new BattleEngine(battleState);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Pokemon;

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
  class BattleState
  {
    public int Turn { get; set; } // odd Player, even Opposite
    public Trainer Player { get; set; }
    public Trainer Opposite { get; set; }
    public Pokemon PlayerPokemon { get; set; }
    public Pokemon OppPokemon { get; set; }
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

    public BattleState(Trainer _user, Trainer _opposite)
    {
      Player = _user;
      Opposite = _opposite;
      BattlePhase = BattlePhase.Start;
      BattleLog = new();
    }
  }

  public class BattleEngine
  {
    private readonly BattleState state;

    public BattleEngine(BattleState _state)
    {
      this.state = state;
    }
    public int CalculateDamage(Pokemon src, Move skill, Pokemon dst)
    {
      int damage = 0;
      /* Pokemon class 따라 구현. */
      return damage;
    }
    public void ExecuteAttack(Pokemon src, Move skill, Pokemon dst)
    {
      int damage = CalculateDamage(src, skill, dst);
      // TODO: Pokemon.Takedamage() 구현 필요
      // HP가 0 이하가 되어 IsFainted(쓰러짐)을 올리는 기능이 필요함.
      // dst.TakeDamage(damage); 

      string logMessage = $"{src.Name}의 {skill.Name}! {dst.Name}에게 {damage}의 피해!";
      state.AddLog(logMessage);
    }

    public bool CheckBattleEnd(out Trainer winner)// 참조 param
    {
      winner = null;
      // TODO: Pokemon 쓰러짐 상태 판별 flag 및 Trainer.가용 포멧몬 수
      if (state.OppPokemon.IsFainted && !state.Opposite.battlableCount)
      {
        winner = state.Player;
        return true;
      }
      if (state.PlayerPokemon.IsFainted && !state.Player.battlableCount)
      {
        winner = state.Opposite;
        return true;
      }
      return false;
    }


    /// <summary>
    /// 플레이어 교체 가능한 포켓몬 리스트 반환
    /// </summary>
    public List<Pokemon> GetSwitchablePlayerPokemons(Trainer player)
    {
      // TODO:
      // LINQ 구문으로 List 가공해서 반환 -> UI에 반영하기 버튼 = 포켓몬 b, c, d, 취소
      return player.Pokemons.Where(p => p != state.PlayerPokemon && !p.IsFainted).ToList();
    }

    /// <summary>
    /// 플레이어 포켓몬 교체 실행
    /// </summary>
    public void SwitchPlayerPokemon(Pokemon newPokemon)
    {
      state.AddLog($"돌아와! {state.PlayerPokemon.Name}!");
      state.PlayerPokemon = newPokemon;
      state.AddLog($"가라! {newPokemon.Name}!");
    }

    /// <summary>
    /// AI 포켓몬 교체 실행 (즉시 처리)
    /// </summary>
    public void SwitchEnemyPokemon(Pokemon newPokemon)
    {
      state.AddLog($"상대 트레이너가 {state.OppPokemon.Name}을(를) {newPokemon.Name}(으)로 교체했습니다!");
      state.OppPokemon = newPokemon;
    }

    public void OppositeAction()
    {
      // TODO: Pokemon이 List<Move> Skill을 가지고 있어야 함.
      int selectable = state.OppPokemon.Skill.Count + 1;
      int select = Random.Shared.Next(0, selectable);


      if(select < selectable - 1)
      {
        Move selectedMove = state.OppPokemon.Skill[select];
        ExecuteAttack(state.OppPokemon, selectedMove, state.PlayerPokemon);
      }
      else
      {
        List<Pokemon> switchablePokemons = GetSwitchablePlayerPokemons(state.Opposite);
        selectable = switchablePokemons.Count;
        SwitchEnemyPokemon(switchablePokemons[Random.Shared.Next(0, selectable)]);
        // TODO: 포켓몬 교체 로직 검토 필요. (교체 후 턴 종료, 교체 후 턴 진행 등)
      }

      state.Turn++; 
    }
  }


  class GameState
  {
    public Trainer Player { get; set; }

    public BattleEngine CurrentBattle { get; set; } // 현재 전투 중이라면 존재하고, 탐색 중이라면 null

    public GameState(string playerName)
    {
      Player = new Trainer();
    }

    // TODO: 상대 트레이너(이름, 포켓몬) 생성 메소드 필요

    public BattleStart(Trainer opponent)
    {
      BattleState battleState = new BattleState(Player, opponent);
      CurrentBattle = new BattleEngine(battleState);
    }

  }
}

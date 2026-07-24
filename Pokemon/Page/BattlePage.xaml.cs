using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Pokemon.Core;
using Pokemon.Models;

namespace Pokemon.Page
{
  /// <summary>
  /// BattlePage.xaml에 대한 상호 작용 논리
  /// </summary>
  public partial class BattlePage : System.Windows.Controls.Page
  {
    private static readonly TimeSpan HpAnimationDuration = TimeSpan.FromMilliseconds(400);
    // private static readonly TimeSpan MessageInterval = TimeSpan.FromSeconds(1.1);
    private const string ReadyPrompt = "어떻게 하시겠습니까?";

    // SkillButtonPage 등 자식 페이지에서 전투 화면을 갱신할 때 사용
    public static BattlePage? Current { get; private set; }

    private readonly Queue<BattleEvent> pendingEvents = new();
    // private readonly DispatcherTimer messageTimer;
    private Action? onSequenceComplete;

    public BattlePage() : this("bg-beach.png")
    {
    }

    public BattlePage(string bgFileName)
    {
      InitializeComponent();
      Current = this;
      // messageTimer = new DispatcherTimer { Interval = MessageInterval };
      // messageTimer.Tick += (_, _) => AdvanceMessage();
      BattleImageFrame.Navigate(new BattleImagePage(bgFileName));
      RefreshUI();
      TextBox.Text = ReadyPrompt;
    }
 
    /// <summary>
    /// 한 턴에서 발생한 이벤트들을 일정 시간 간격으로 자동 재생함.
    /// 전부 재생하고 나면 onComplete를 호출함.
    /// </summary>
    public async void PlaySequence(IEnumerable<BattleEvent> events, Action onComplete)
    {
      pendingEvents.Clear();
      foreach (var battleEvent in events)
      {
        pendingEvents.Enqueue(battleEvent);
      }

      onSequenceComplete = onComplete;

      // 비동기 시퀀스 시작
      await AdvanceMessageAsync();
    }

    // async 메서드로 변경
    private async Task AdvanceMessageAsync()
    {
      while (pendingEvents.Count > 0)
      {
        var battleEvent = pendingEvents.Dequeue();

        if (battleEvent is BattleEvent.DamageDealt damage)
        {
          AnimateHp(damage);
          continue;
        }

        if (battleEvent is BattleEvent.PokemonSwitched switched)
        {
          RefreshSide(switched.IsPlayerSide);
          continue;
        }

        if (battleEvent is BattleEvent.Message message)
        {
          // 한 글자씩 출력하는 비동기 로직 실행 후 대기
          await TypeMessageAsync(message.Text, charsPerSecond: 15);

          // 문장이 다 찍힌 후 다음 메시지로 넘어가기 전 잠시 대기(예: 1초)
          await Task.Delay(800);
          continue;
        }
      }

      // 모든 이벤트 처리 완료 후
      RefreshUI();
      // TextBox.Text = ReadyPrompt;
      await TypeMessageAsync(ReadyPrompt, charsPerSecond: 15);

      var complete = onSequenceComplete;
      onSequenceComplete = null;
      complete?.Invoke();
    }

    // 텍스트를 한 글자씩 덧붙이는 헬퍼 메서드
    private async Task TypeMessageAsync(string fullText, int charsPerSecond)
    {
      TextBox.Text = ""; // 기존 텍스트 초기화

      // 1초에 n개 -> 글자당 지연 시간 계산 (예: 6글자/초 -> 약 166ms)
      int delayMs = 1000 / charsPerSecond;

      foreach (char c in fullText)
      {
        TextBox.Text += c; // string에 char 추가
        await Task.Delay(delayMs); // UI 스레드를 블로킹하지 않고 지연
      }
    }

    private void AnimateHp(BattleEvent.DamageDealt damage)
    {
      var bar = damage.TargetIsPlayer ? PlayerHpBar : OppHpBar;
      var animation = new DoubleAnimation
      {
        To = damage.NewHp,
        Duration = HpAnimationDuration,
      };
      bar.BeginAnimation(RangeBase.ValueProperty, animation);
    }

    public void RefreshUI()
    {
      RefreshSide(isPlayerSide: false);
      RefreshSide(isPlayerSide: true);
    }

    private void RefreshSide(bool isPlayerSide)
    {
      var engine = Game.State.CurrentBattle;
      if (engine is null)
      {
        return;
      }
      var state = engine.State;

      if (isPlayerSide)
      {
        var pokemon = state.PlayerPokemon;
        PlayerNameText.Text = pokemon.Name;
        PlayerHpBar.BeginAnimation(RangeBase.ValueProperty, null);
        PlayerHpBar.Maximum = pokemon.MaxHp;
        PlayerHpBar.Value = pokemon.CurrentHp;
        PlayerSpriteImage.Source = LoadSprite(pokemon.BackImagePath);
      }
      else
      {
        var pokemon = state.OppPokemon;
        OppNameText.Text = pokemon.Name;
        OppHpBar.BeginAnimation(RangeBase.ValueProperty, null);
        OppHpBar.Maximum = pokemon.MaxHp;
        OppHpBar.Value = pokemon.CurrentHp;
        OppSpriteImage.Source = LoadSprite(pokemon.FrontImagePath);
      }
    }

    private static BitmapImage LoadSprite(string relativePath) =>
        new BitmapImage(new Uri($"pack://siteoforigin:,,,{relativePath}"));
  }
}

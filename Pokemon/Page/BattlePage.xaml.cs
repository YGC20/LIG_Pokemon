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
    private static readonly TimeSpan EffectMoveDuration = TimeSpan.FromMilliseconds(550);
    private static readonly TimeSpan EffectFlashDuration = TimeSpan.FromMilliseconds(250);
    private const string ReadyPrompt = "어떻게 하시겠습니까?";
    private static readonly TypeToColorConverter TypeColorConverter = new();

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
      AudioService.PlayBgm("battle.mp3");
    }
 
    /// <summary>
    /// 한 턴에서 발생한 이벤트들을 일정 시간 간격으로 자동 재생함.
    /// 전부 재생하고 나면 onComplete를 호출함.
    /// </summary>
    public async void PlaySequence(IEnumerable<BattleEvent> events, Action onComplete, bool showReadyPrompt = true)
    {
      pendingEvents.Clear();
      foreach (var battleEvent in events)
      {
        pendingEvents.Enqueue(battleEvent);
      }

      onSequenceComplete = onComplete;

      // 비동기 시퀀스 시작
      await AdvanceMessageAsync(showReadyPrompt);
    }

    // async 메서드로 변경
    private async Task AdvanceMessageAsync(bool showReadyPrompt)
    {
      while (pendingEvents.Count > 0)
      {
        var battleEvent = pendingEvents.Dequeue();

        if (battleEvent is BattleEvent.DamageDealt damage)
        {
          AnimateHp(damage);
          continue;
        }

        if (battleEvent is BattleEvent.HpRecovered recovery)
        {
          AnimateHpRecovery(recovery);
          continue;
        }

        if (battleEvent is BattleEvent.PokemonSwitched switched)
        {
          RefreshSide(switched.IsPlayerSide);
          continue;
        }

        if (battleEvent is BattleEvent.AttackEffect effect)
        {
          PlayAttackEffect(effect.AttackerIsPlayer, effect.SkillType);
          await Task.Delay(EffectMoveDuration);
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

      var complete = onSequenceComplete;
      onSequenceComplete = null;
      complete?.Invoke();

      // 새 포켓몬을 강제로 골라야 하는 경우 등엔 "어떻게 하시겠습니까?" 문구를 보여주지 않음
      if (showReadyPrompt)
      {
        await TypeMessageAsync(ReadyPrompt, charsPerSecond: 15);
      }
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
      AudioService.PlaySfx("attack_sound.mp3");

      var bar = damage.TargetIsPlayer ? PlayerHpBar : OppHpBar;
      var animation = new DoubleAnimation
      {
        To = damage.NewHp,
        Duration = HpAnimationDuration,
      };
      bar.BeginAnimation(RangeBase.ValueProperty, animation);
    }

    private void AnimateHpRecovery(BattleEvent.HpRecovered recovery)
    {
      var bar = recovery.TargetIsPlayer ? PlayerHpBar : OppHpBar;
      var animation = new DoubleAnimation
      {
        To = recovery.NewHp,
        Duration = HpAnimationDuration,
      };
      bar.BeginAnimation(RangeBase.ValueProperty, animation);
    }

    /// <summary>
    /// 공격자 스프라이트에서 대상 스프라이트로 날아가는 투사체 이펙트를 재생하고,
    /// 도착 지점에서 타격 이펙트를 이어서 재생함.
    /// </summary>
    private void PlayAttackEffect(bool attackerIsPlayer, PokemonType skillType)
    {
      var attackerSprite = attackerIsPlayer ? PlayerSpriteImage : OppSpriteImage;
      var targetSprite = attackerIsPlayer ? OppSpriteImage : PlayerSpriteImage;

      if (attackerSprite.ActualWidth == 0 || targetSprite.ActualWidth == 0)
      {
        return;
      }

      Point start = attackerSprite.TranslatePoint(
          new Point(attackerSprite.ActualWidth / 2, attackerSprite.ActualHeight / 2), EffectCanvas);
      Point end = targetSprite.TranslatePoint(
          new Point(targetSprite.ActualWidth / 2, targetSprite.ActualHeight / 2), EffectCanvas);

      var brush = (Brush)TypeColorConverter.Convert(skillType, typeof(Brush), null!, CultureInfo.InvariantCulture);

      var scaleTransform = new ScaleTransform(0.4, 0.4);
      var rotateTransform = new RotateTransform(0);
      var projectile = new Ellipse
      {
        Width = 40,
        Height = 40,
        Fill = brush,
        Stroke = Brushes.White,
        StrokeThickness = 2,
        RenderTransformOrigin = new Point(0.5, 0.5),
        RenderTransform = new TransformGroup { Children = { scaleTransform, rotateTransform } },
        Effect = new DropShadowEffect
        {
          Color = ((SolidColorBrush)brush).Color,
          BlurRadius = 25,
          ShadowDepth = 0,
          Opacity = 0.9,
        },
      };
      Canvas.SetLeft(projectile, start.X - projectile.Width / 2);
      Canvas.SetTop(projectile, start.Y - projectile.Height / 2);
      EffectCanvas.Children.Add(projectile);

      // 접근할수록 가속되고(EaseIn), 커지면서 다가오는 느낌을 줌
      var ease = new QuadraticEase { EasingMode = EasingMode.EaseIn };
      var moveX = new DoubleAnimation(start.X - projectile.Width / 2, end.X - projectile.Width / 2, EffectMoveDuration) { EasingFunction = ease };
      var moveY = new DoubleAnimation(start.Y - projectile.Height / 2, end.Y - projectile.Height / 2, EffectMoveDuration) { EasingFunction = ease };
      var growAnim = new DoubleAnimation(0.4, 1.6, EffectMoveDuration) { EasingFunction = ease };
      moveX.Completed += (_, _) =>
      {
        EffectCanvas.Children.Remove(projectile);
        PlayImpactFlash(end, brush);
      };

      projectile.BeginAnimation(Canvas.LeftProperty, moveX);
      projectile.BeginAnimation(Canvas.TopProperty, moveY);
      scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, growAnim);
      scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, growAnim);
    }

    private void PlayImpactFlash(Point center, Brush brush)
    {
      var flash = new Ellipse
      {
        Width = 16,
        Height = 16,
        Fill = brush,
        Opacity = 0.85,
        RenderTransformOrigin = new Point(0.5, 0.5),
        RenderTransform = new ScaleTransform(1, 1),
      };
      Canvas.SetLeft(flash, center.X - flash.Width / 2);
      Canvas.SetTop(flash, center.Y - flash.Height / 2);
      EffectCanvas.Children.Add(flash);

      var scaleAnim = new DoubleAnimation(6, 8, EffectFlashDuration);
      var fadeAnim = new DoubleAnimation(flash.Opacity, 0, EffectFlashDuration);
      fadeAnim.Completed += (_, _) => EffectCanvas.Children.Remove(flash);

      var scaleTransform = (ScaleTransform)flash.RenderTransform;
      scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
      scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
      flash.BeginAnimation(OpacityProperty, fadeAnim);
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

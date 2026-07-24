using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Pokemon.Core;

namespace Pokemon.Page
{
    /// <summary>
    /// BattlePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BattlePage : System.Windows.Controls.Page
    {
        private static readonly TimeSpan HpAnimationDuration = TimeSpan.FromMilliseconds(400);
        private static readonly TimeSpan MessageInterval = TimeSpan.FromSeconds(1.1);
        private const string ReadyPrompt = "어떻게 하시겠습니까?";

        // SkillButtonPage 등 자식 페이지에서 전투 화면을 갱신할 때 사용
        public static BattlePage? Current { get; private set; }

        private readonly Queue<BattleEvent> pendingEvents = new();
        private readonly DispatcherTimer messageTimer;
        private Action? onSequenceComplete;

        public BattlePage() : this("bg-beach.png")
        {
        }

        public BattlePage(string bgFileName)
        {
            InitializeComponent();
            Current = this;
            messageTimer = new DispatcherTimer { Interval = MessageInterval };
            messageTimer.Tick += (_, _) => AdvanceMessage();
            BattleImageFrame.Navigate(new BattleImagePage(bgFileName));
            RefreshUI();
            TextBox.Text = ReadyPrompt;
        }

        /// <summary>
        /// 한 턴에서 발생한 이벤트들을 일정 시간 간격으로 자동 재생함.
        /// 전부 재생하고 나면 onComplete를 호출함.
        /// </summary>
        public void PlaySequence(IEnumerable<BattleEvent> events, Action onComplete)
        {
            messageTimer.Stop();
            pendingEvents.Clear();
            foreach (var battleEvent in events)
            {
                pendingEvents.Enqueue(battleEvent);
            }

            onSequenceComplete = onComplete;
            messageTimer.Start();
            AdvanceMessage();
        }

        private void AdvanceMessage()
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
                    RefreshSide(switched.IsPlayerSide, switched.HpAtSwitch);
                    continue;
                }

                if (battleEvent is BattleEvent.Message message)
                {
                    TextBox.Text = message.Text;
                    return;
                }
            }

            messageTimer.Stop();
            RefreshUI();
            TextBox.Text = ReadyPrompt;

            var complete = onSequenceComplete;
            onSequenceComplete = null;
            complete?.Invoke();
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

        private void RefreshSide(bool isPlayerSide, int? displayedHp = null)
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
                PlayerHpBar.Value = displayedHp ?? pokemon.CurrentHp;
                PlayerSpriteImage.Source = LoadSprite(pokemon.BackImagePath);
            }
            else
            {
                var pokemon = state.OppPokemon;
                OppNameText.Text = pokemon.Name;
                OppHpBar.BeginAnimation(RangeBase.ValueProperty, null);
                OppHpBar.Maximum = pokemon.MaxHp;
                OppHpBar.Value = displayedHp ?? pokemon.CurrentHp;
                OppSpriteImage.Source = LoadSprite(pokemon.FrontImagePath);
            }
        }

        private static BitmapImage LoadSprite(string relativePath) =>
            new BitmapImage(new Uri($"pack://siteoforigin:,,,{relativePath}"));
    }
}

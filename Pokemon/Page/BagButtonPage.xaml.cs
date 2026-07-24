using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pokemon.Core;
using Pokemon.Models;

namespace Pokemon.Page
{
    public partial class BagButtonPage : System.Windows.Controls.Page
    {
        public BagButtonPage()
        {
            InitializeComponent();

            RefreshItemButtons();
        }

        private void RefreshItemButtons()
        {
            int hpPotionCount = Game.State.GetItemCount(ItemDB.HpPotion);

            Item1Button.Content = $"{ItemDB.HpPotion.Name} * {hpPotionCount}";
            Item1Button.IsEnabled = hpPotionCount > 0;
        }

        private void UseItem(Item item)
        {
            var engine = Game.State.CurrentBattle;
            var battlePage = BattlePage.Current;
            if (engine is null || battlePage is null)
            {
                return;
            }

            var state = engine.State;
            int eventsBefore = state.Events.Count;

            TurnOutcome? outcome = Game.State.UseBattleItem(item);
            if (outcome is null)
            {
                return;
            }

            var newEvents = state.Events.Skip(eventsBefore).ToList();

            SetItemButtonsEnabled(false);
            battlePage.PlaySequence(newEvents, () => OnTurnMessagesFinished(outcome), showReadyPrompt: !outcome.RequiresPlayerSwitch);
        }

        private void Item1ButtonClick(object sender, RoutedEventArgs e)
        {
            UseItem(ItemDB.HpPotion);
        }

        private void SetItemButtonsEnabled(bool isEnabled)
        {
            Item1Button.IsEnabled = isEnabled;
            Item2Button.IsEnabled = false;
            Item3Button.IsEnabled = false;
            Item4Button.IsEnabled = false;
            BackButton.IsEnabled = isEnabled;
        }

        private void OnTurnMessagesFinished(TurnOutcome outcome)
        {
            if (outcome.BattleEnded)
            {
                bool playerWon = outcome.Winner == Game.State.Player;

                if (playerWon && Game.State.CurrentTrainerNumber > 0)
                {
                    // 무한 모드: 트레이너를 쓰러뜨림 -> 보상 3택1 화면으로
                    BattlePage.Current?.ShowRewardSelection();
                    return;
                }

                if (playerWon && Game.State.CurrentBossNumber is int bossNumber)
                {
                    Game.State.MarkBossDefeated(bossNumber);
                }

                var mainWindow = (Pokemon.MainWindow)Application.Current.MainWindow;
                mainWindow.MainFrame.Navigate(new ResultPage(playerWon));
                return;
            }

            if (outcome.RequiresPlayerSwitch)
            {
                NavigationService.Navigate(new PokemonButtonPage());
                return;
            }

            NavigationService.Navigate(new MenuButtonPage());
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuButtonPage());
        }
    }
}

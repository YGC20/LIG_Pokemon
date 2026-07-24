using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pokemon.Core;

namespace Pokemon.Page
{
    /// <summary>
    /// PokemonButtonPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PokemonButtonPage : System.Windows.Controls.Page
    {
        public PokemonButtonPage()
        {
            InitializeComponent();

            var state = Game.State.CurrentBattle?.State;
            if (state is null)
            {
                return;
            }

            var buttons = new[]
            {
                Pokemon1Button,
                Pokemon2Button,
                Pokemon3Button,
                Pokemon4Button
            };

            for (int index = 0; index < buttons.Length; index++)
            {
                if (index >= state.Player.Pokemons.Count)
                {
                    buttons[index].Visibility = Visibility.Collapsed;
                    continue;
                }

                var pokemon = state.Player.Pokemons[index];
                buttons[index].Content =
                    $"{pokemon.Name}\nHP {pokemon.CurrentHp}/{pokemon.MaxHp}";
                buttons[index].IsEnabled =
                    !pokemon.IsFainted &&
                    !ReferenceEquals(pokemon, state.PlayerPokemon);
            }

            // 현재 포켓몬이 기절했다면 교체가 필수이므로 뒤로 갈 수 없다.
            BackButton.IsEnabled = !state.PlayerPokemon.IsFainted;
        }

        private void SwitchPokemon(int index)
        {
            var engine = Game.State.CurrentBattle;
            var battlePage = BattlePage.Current;
            if (engine is null || battlePage is null)
            {
                return;
            }

            var state = engine.State;
            if (index < 0 || index >= state.Player.Pokemons.Count)
            {
                return;
            }

            int eventsBefore = state.Events.Count;

            try
            {
                TurnOutcome outcome =
                    engine.PerformPlayerSwitch(state.Player.Pokemons[index]);

                var newEvents = state.Events
                    .Skip(eventsBefore)
                    .ToList();

                SetPokemonButtonsEnabled(false);
                battlePage.PlaySequence(
                    newEvents,
                    () => OnSwitchFinished(outcome),
                    showReadyPrompt: !outcome.RequiresPlayerSwitch);
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SetPokemonButtonsEnabled(bool isEnabled)
        {
            Pokemon1Button.IsEnabled = isEnabled;
            Pokemon2Button.IsEnabled = isEnabled;
            Pokemon3Button.IsEnabled = isEnabled;
            Pokemon4Button.IsEnabled = isEnabled;
            BackButton.IsEnabled = isEnabled;
        }

        private void OnSwitchFinished(TurnOutcome outcome)
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

                var mainWindow =
                    (Pokemon.MainWindow)Application.Current.MainWindow;
                mainWindow.MainFrame.Navigate(new ResultPage(playerWon));
                return;
            }

            // 일반 교체 직후 상대 공격으로 새 포켓몬이 기절한 경우
            // 다시 강제 교체 화면을 표시한다.
            if (outcome.RequiresPlayerSwitch)
            {
                NavigationService.Navigate(new PokemonButtonPage());
                return;
            }

            NavigationService.Navigate(new MenuButtonPage());
        }

        private void Pokemon1ButtonClick(object sender, RoutedEventArgs e) =>
            SwitchPokemon(0);

        private void Pokemon2ButtonClick(object sender, RoutedEventArgs e) =>
            SwitchPokemon(1);

        private void Pokemon3ButtonClick(object sender, RoutedEventArgs e) =>
            SwitchPokemon(2);

        private void Pokemon4ButtonClick(object sender, RoutedEventArgs e) =>
            SwitchPokemon(3);

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuButtonPage());
        }
    }
}

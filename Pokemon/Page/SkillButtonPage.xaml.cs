using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// SkillButtonPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SkillButtonPage : System.Windows.Controls.Page
    {
        public SkillButtonPage()
        {
            InitializeComponent();

            var skills = Game.State.CurrentBattle?.State.PlayerPokemon.Skills;
            if (skills is { Count: 4 })
            {
                Skill1Button.Content = skills[0].Name;
                Skill2Button.Content = skills[1].Name;
                Skill3Button.Content = skills[2].Name;
                Skill4Button.Content = skills[3].Name;

                Skill1Button.DataContext = skills[0];
                Skill2Button.DataContext = skills[1];
                Skill3Button.DataContext = skills[2];
                Skill4Button.DataContext = skills[3];
            }
        }

        private void UseSkill(int index)
        {
            var engine = Game.State.CurrentBattle;
            var battlePage = BattlePage.Current;
            if (engine is null || battlePage is null)
            {
                return;
            }

            var state = engine.State;
            var skill = state.PlayerPokemon.Skills[index];

            int eventsBefore = state.Events.Count;
            TurnOutcome outcome = engine.PerformPlayerTurn(skill);
            var newEvents = state.Events.Skip(eventsBefore).ToList();

            SetSkillButtonsEnabled(false);
            battlePage.PlaySequence(newEvents, () => OnTurnMessagesFinished(outcome));
        }

        private void SetSkillButtonsEnabled(bool isEnabled)
        {
            Skill1Button.IsEnabled = isEnabled;
            Skill2Button.IsEnabled = isEnabled;
            Skill3Button.IsEnabled = isEnabled;
            Skill4Button.IsEnabled = isEnabled;
            BackButton.IsEnabled = isEnabled;
        }

        private void OnTurnMessagesFinished(TurnOutcome outcome)
        {
            if (outcome.BattleEnded)
            {
                bool playerWon = outcome.Winner == Game.State.Player;
                if (playerWon && Game.State.CurrentBossNumber is int bossNumber)
                {
                    Game.State.MarkBossDefeated(bossNumber);
                }

                var mainWindow = (Pokemon.MainWindow)System.Windows.Application.Current.MainWindow;
                mainWindow.MainFrame.Navigate(new ResultPage(playerWon));
                return;
            }

            NavigationService.Navigate(new MenuButtonPage());
        }

        private void Skill1ButtonClick(object sender, RoutedEventArgs e) => UseSkill(0);

        private void Skill2ButtonClick(object sender, RoutedEventArgs e) => UseSkill(1);

        private void Skill3ButtonClick(object sender, RoutedEventArgs e) => UseSkill(2);

        private void Skill4ButtonClick(object sender, RoutedEventArgs e) => UseSkill(3);

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuButtonPage());
        }
    }
}

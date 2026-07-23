using System;
using System.Collections.Generic;
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
            }
        }

        private void UseSkill(int index)
        {
            var engine = Game.State.CurrentBattle;
            if (engine is null)
            {
                return;
            }

            var skill = engine.State.PlayerPokemon.Skills[index];
            TurnOutcome outcome = engine.PerformPlayerTurn(skill);
            BattlePage.Current?.RefreshUI();

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

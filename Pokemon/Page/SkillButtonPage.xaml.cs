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
using Pokemon.Models;

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
                Skill1Button.Content = BuildSkillContent(skills[0]);
                Skill2Button.Content = BuildSkillContent(skills[1]);
                Skill3Button.Content = BuildSkillContent(skills[2]);
                Skill4Button.Content = BuildSkillContent(skills[3]);

                Skill1Button.DataContext = skills[0];
                Skill2Button.DataContext = skills[1];
                Skill3Button.DataContext = skills[2];
                Skill4Button.DataContext = skills[3];
            }
        }

        // 스킬 이름 아래에 타입/위력 상세 정보를 함께 보여줌
        private static object BuildSkillContent(Skill skill)
        {
            var panel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            panel.Children.Add(new TextBlock
            {
                Text = skill.Name,
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            panel.Children.Add(new TextBlock
            {
                Text = $"{skill.Type.ToKoreanName()} · 위력 {skill.Power}",
                FontSize = 13,
                FontWeight = FontWeights.Normal,
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            return panel;
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
            battlePage.PlaySequence(newEvents, () => OnTurnMessagesFinished(outcome), showReadyPrompt: !outcome.RequiresPlayerSwitch);
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

                var mainWindow = (Pokemon.MainWindow)System.Windows.Application.Current.MainWindow;
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

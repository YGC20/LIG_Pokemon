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
using Pokemon.Core;
using Pokemon.Models;

namespace Pokemon.Page
{
    /// <summary>
    /// BossMenuPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BossMenuPage : System.Windows.Controls.Page
    {
        public BossMenuPage()
        {
            InitializeComponent();
            AudioService.PlayBgm("battle_choice_bgm.mp3");
        }

        private void Boss1Button_Click(object sender, RoutedEventArgs e)
        {
            Game.State.BattleStart(TrainerDB.CreateGymLeader(1), 1);
            NavigationService.Navigate(new BattlePage("bg-beach.png"));
        }

        private void Boss2Button_Click(object sender, RoutedEventArgs e)
        {
            Game.State.BattleStart(TrainerDB.CreateGymLeader(2), 2);
            NavigationService.Navigate(new BattlePage("bg-city.png"));
        }

        private void Boss3Button_Click(object sender, RoutedEventArgs e)
        {
            Game.State.BattleStart(TrainerDB.CreateGymLeader(3), 3);
            NavigationService.Navigate(new BattlePage("bg-dampcave.png"));
        }

        private void Boss4Button_Click(object sender, RoutedEventArgs e)
        {
            Game.State.BattleStart(TrainerDB.CreateGymLeader(4), 4);
            NavigationService.Navigate(new BattlePage("bg-forest.png"));
        }
    }
}

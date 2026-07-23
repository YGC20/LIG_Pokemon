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
    /// BossMenuPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BossMenuPage : System.Windows.Controls.Page
    {
        public BossMenuPage()
        {
            InitializeComponent();
        }

        private void Boss1Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BattlePage("bg-beach.png"));
        }

        private void Boss2Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BattlePage("bg-city.png"));
        }

        private void Boss3Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BattlePage("bg-dampcave.png"));
        }

        private void Boss4Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BattlePage("bg-forest.png"));
        }
    }
}

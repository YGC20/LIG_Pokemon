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
    /// PokemonButtonPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PokemonButtonPage : System.Windows.Controls.Page
    {
        public PokemonButtonPage()
        {
            InitializeComponent();
        }

        private void Pokemon1ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Pokemon2ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Pokemon3ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Pokemon4ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuButtonPage());
        }
    }
}

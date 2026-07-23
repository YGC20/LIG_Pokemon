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
    /// MenuButtonPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuButtonPage : System.Windows.Controls.Page
    {
        public MenuButtonPage()
        {
            InitializeComponent();
        }

        private void SkillButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void PokemonButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = (Pokemon.MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new BossMenuPage());
        }
    }
}

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

namespace Pokemon.Page
{
    /// <summary>
    /// ResultPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultPage : System.Windows.Controls.Page
    {
        private readonly bool gameCleared;

        public ResultPage(bool playerWon)
        {
            InitializeComponent();

            gameCleared = playerWon && Game.State.AllBossesDefeated;

            ResultText.Text = gameCleared
                ? "축하합니다!\n모든 관장을 격파했습니다!\nGAME CLEAR!"
                : playerWon
                    ? "승리했습니다!"
                    : "패배했습니다...";
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = (Pokemon.MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(gameCleared ? new MenuPage() : new BossMenuPage());
        }
    }
}

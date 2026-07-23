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
    /// BattlePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BattlePage : System.Windows.Controls.Page
    {
        public BattlePage() : this("bg-beach.png")
        {
        }

        public BattlePage(string bgFileName)
        {
            InitializeComponent();
            BattleImageFrame.Navigate(new BattleImagePage(bgFileName));
        }
    }
}

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

namespace Pokemon.Page
{
    /// <summary>
    /// BattlePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BattlePage : System.Windows.Controls.Page
    {
        // SkillButtonPage 등 자식 페이지에서 전투 화면을 갱신할 때 사용
        public static BattlePage? Current { get; private set; }

        public BattlePage() : this("bg-beach.png")
        {
        }

        public BattlePage(string bgFileName)
        {
            InitializeComponent();
            Current = this;
            BattleImageFrame.Navigate(new BattleImagePage(bgFileName));
            RefreshUI();
        }

        public void RefreshUI()
        {
            var engine = Game.State.CurrentBattle;
            if (engine is null)
            {
                return;
            }
            var state = engine.State;

            OppNameText.Text = state.OppPokemon.Name;
            OppHpBar.Maximum = state.OppPokemon.MaxHp;
            OppHpBar.Value = state.OppPokemon.CurrentHp;
            OppSpriteImage.Source = LoadSprite(state.OppPokemon.FrontImagePath);

            PlayerNameText.Text = state.PlayerPokemon.Name;
            PlayerHpBar.Maximum = state.PlayerPokemon.MaxHp;
            PlayerHpBar.Value = state.PlayerPokemon.CurrentHp;
            PlayerSpriteImage.Source = LoadSprite(state.PlayerPokemon.BackImagePath);

            TextBox.Text = string.Join("\n", state.BattleLog.TakeLast(6));
        }

        private static BitmapImage LoadSprite(string relativePath) =>
            new BitmapImage(new Uri($"pack://siteoforigin:,,,/{relativePath}"));
    }
}

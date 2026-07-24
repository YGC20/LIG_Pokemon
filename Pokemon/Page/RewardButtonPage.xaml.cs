using System;
using System.Linq;
using System.Windows;
using Pokemon.Core;
using Pokemon.Models;

namespace Pokemon.Page
{
    /// <summary>
    /// 무한 모드에서 트레이너를 쓰러뜨린 뒤 도구 3택1로 보여주는 화면.
    /// </summary>
    public partial class RewardButtonPage : System.Windows.Controls.Page
    {
        private readonly Item?[] rewardItems = new Item?[3];

        public RewardButtonPage()
        {
            InitializeComponent();
            CreateRandomRewards();
        }

        private void CreateRandomRewards()
        {
            var selectedItems = ItemDB.All
                .OrderBy(_ => Random.Shared.Next())
                .Take(3)
                .ToList();

            var buttons = new[] { Reward1Button, Reward2Button, Reward3Button };

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < selectedItems.Count)
                {
                    Item item = selectedItems[i];
                    rewardItems[i] = item;
                    buttons[i].Content = $"{item.Name}\n+1";
                    buttons[i].IsEnabled = true;
                }
                else
                {
                    rewardItems[i] = null;
                    buttons[i].Content = "EMPTY";
                    buttons[i].IsEnabled = false;
                }
            }
        }

        private void ClaimReward(int index)
        {
            Item? selectedItem = rewardItems[index];
            if (selectedItem is null)
            {
                return;
            }

            // 중복 클릭 방지
            Reward1Button.IsEnabled = false;
            Reward2Button.IsEnabled = false;
            Reward3Button.IsEnabled = false;

            Game.State.AddItem(selectedItem, 1);
            BattlePage.Current?.ContinueToNextTrainer(selectedItem);
        }

        private void Reward1ButtonClick(object sender, RoutedEventArgs e) => ClaimReward(0);

        private void Reward2ButtonClick(object sender, RoutedEventArgs e) => ClaimReward(1);

        private void Reward3ButtonClick(object sender, RoutedEventArgs e) => ClaimReward(2);
    }
}

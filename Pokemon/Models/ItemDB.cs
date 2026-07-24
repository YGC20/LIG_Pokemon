using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon.Models
{
    public static class ItemDB
    {
        public static readonly Item HpPotion =
            new Item(
                "HP Potion",
                ItemEffectType.HealHp,
                20
            );

        // 무한 모드 보상 화면 등에서 무작위로 하나 뽑을 때 쓰는 전체 목록
        public static readonly IReadOnlyList<Item> All = new[]
        {
            HpPotion,
        };
    }
}

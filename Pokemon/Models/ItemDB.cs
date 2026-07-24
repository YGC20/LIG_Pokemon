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
    }
}

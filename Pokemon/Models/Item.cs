using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon.Models
{
    public enum ItemEffectType
    {
        HealHp
    }

    public class Item
    {
        public string Name { get; }
        public ItemEffectType EffectType { get; }
        public int Value { get; }

        public Item(
            string name,
            ItemEffectType effectType,
            int value)
        {
            Name = name;
            EffectType = effectType;
            Value = value;
        }
    }
}

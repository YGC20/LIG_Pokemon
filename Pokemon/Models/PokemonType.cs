using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon.Models
{
    public enum PokemonType
    {
        Water,
        Grass,
        Electric,
        Fire,
        Ground,
        Flying,
        Ice,
        Fighting,
        Normal,
    }

    public static class PokemonTypeExtensions
    {
        public static string ToKoreanName(this PokemonType type) => type switch
        {
            PokemonType.Water => "물",
            PokemonType.Grass => "풀",
            PokemonType.Electric => "전기",
            PokemonType.Fire => "불꽃",
            PokemonType.Ground => "땅",
            PokemonType.Flying => "비행",
            PokemonType.Ice => "얼음",
            PokemonType.Fighting => "격투",
            PokemonType.Normal => "노말",
            _ => type.ToString(),
        };
    }
}

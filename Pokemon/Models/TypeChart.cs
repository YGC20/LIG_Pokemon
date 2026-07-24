using System.Collections.Generic;

namespace Pokemon.Models;

// 공격자가 방어자에게 2배 데미지를 주는 쌍만 등록.
// 등록된 쌍의 역방향은 자동으로 0.5배, 그 외는 1배로 취급.
internal static class TypeChart
{
    private static readonly HashSet<(PokemonType Attacker, PokemonType Defender)> StrongAgainst = new()
    {
        (PokemonType.Water, PokemonType.Fire),
        (PokemonType.Water, PokemonType.Ground),
        (PokemonType.Grass, PokemonType.Electric),
        (PokemonType.Grass, PokemonType.Water),
        (PokemonType.Electric, PokemonType.Water),
        (PokemonType.Ground, PokemonType.Fire),
        (PokemonType.Ground, PokemonType.Electric),
        (PokemonType.Fire, PokemonType.Grass),
    };

    public static double GetMultiplier(PokemonType attacker, PokemonType defender)
    {
        if (attacker == defender)
        {
            return 1.0;
        }
        if (StrongAgainst.Contains((attacker, defender)))
        {
            return 2.0;
        }
        if (StrongAgainst.Contains((defender, attacker)))
        {
            return 0.5;
        }
        return 1.0;
    }
}

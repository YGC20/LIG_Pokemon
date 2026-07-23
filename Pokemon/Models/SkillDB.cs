namespace Pokemon.Models;

// 타입당 4개씩(위력 오름차순), SkillButtonPage의 4버튼과 1:1로 매칭됨.
// 학습 프로젝트라 같은 타입 포켓몬은 전부 이 4기술을 그대로 공유함(기술폭 고정).
internal static class SkillDB
{
    // Water
    public static readonly Skill WaterGun = new("물대포", PokemonType.Water, 40);
    public static readonly Skill BubbleBeam = new("거품광선", PokemonType.Water, 55);
    public static readonly Skill Surf = new("파도타기", PokemonType.Water, 75);
    public static readonly Skill HydroPump = new("하이드로펌프", PokemonType.Water, 110);

    // Grass
    public static readonly Skill VineWhip = new("덩굴채찍", PokemonType.Grass, 45);
    public static readonly Skill RazorLeaf = new("잎날가르기", PokemonType.Grass, 55);
    public static readonly Skill LeafBlade = new("리프블레이드", PokemonType.Grass, 75);
    public static readonly Skill FrenzyPlant = new("프렌지플랜트", PokemonType.Grass, 110);

    // Electric
    public static readonly Skill ThunderShock = new("전기쇼크", PokemonType.Electric, 40);
    public static readonly Skill Discharge = new("방전", PokemonType.Electric, 65);
    public static readonly Skill Thunderbolt = new("10만볼트", PokemonType.Electric, 90);
    public static readonly Skill Thunder = new("번개", PokemonType.Electric, 110);

    // Fire
    public static readonly Skill Ember = new("불꽃세례", PokemonType.Fire, 40);
    public static readonly Skill FireFang = new("불태우기", PokemonType.Fire, 60);
    public static readonly Skill Flamethrower = new("화염방사", PokemonType.Fire, 90);
    public static readonly Skill Overheat = new("오버히트", PokemonType.Fire, 120);

    // Ground
    public static readonly Skill MudSlap = new("진흙뿌리기", PokemonType.Ground, 20);
    public static readonly Skill SandTomb = new("사구몰이", PokemonType.Ground, 45);
    public static readonly Skill Dig = new("다이빙", PokemonType.Ground, 60);
    public static readonly Skill Earthquake = new("지진", PokemonType.Ground, 100);

    public static System.Collections.Generic.IReadOnlyList<Skill> GetMovesFor(PokemonType type) => type switch
    {
        PokemonType.Water => new[] { WaterGun, BubbleBeam, Surf, HydroPump },
        PokemonType.Grass => new[] { VineWhip, RazorLeaf, LeafBlade, FrenzyPlant },
        PokemonType.Electric => new[] { ThunderShock, Discharge, Thunderbolt, Thunder },
        PokemonType.Fire => new[] { Ember, FireFang, Flamethrower, Overheat },
        PokemonType.Ground => new[] { MudSlap, SandTomb, Dig, Earthquake },
        _ => throw new System.ArgumentOutOfRangeException(nameof(type)),
    };
}

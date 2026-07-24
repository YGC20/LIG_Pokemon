using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Models;

// 모든 기술 중 4개를 중복 없이 무작위로 뽑아 포켓몬에게 배정한다.
internal static class SkillDB
{
    // Water
    public static readonly Skill WaterGun = new("물대포", PokemonType.Water, 20);
    public static readonly Skill BubbleBeam = new("거품광선", PokemonType.Water, 35);
    public static readonly Skill Surf = new("파도타기", PokemonType.Water, 55);
    public static readonly Skill HydroPump = new("하이드로펌프", PokemonType.Water, 90);

    // Grass
    public static readonly Skill VineWhip = new("덩굴채찍", PokemonType.Grass, 25);
    public static readonly Skill RazorLeaf = new("잎날가르기", PokemonType.Grass, 35);
    public static readonly Skill LeafBlade = new("리프블레이드", PokemonType.Grass, 55);
    public static readonly Skill FrenzyPlant = new("프렌지플랜트", PokemonType.Grass, 90);

    // Electric
    public static readonly Skill ThunderShock = new("전기쇼크", PokemonType.Electric, 20);
    public static readonly Skill Discharge = new("방전", PokemonType.Electric, 45);
    public static readonly Skill Thunderbolt = new("10만볼트", PokemonType.Electric, 70);
    public static readonly Skill Thunder = new("번개", PokemonType.Electric, 90);

    // Fire
    public static readonly Skill Ember = new("불꽃세례", PokemonType.Fire, 20);
    public static readonly Skill FireFang = new("불태우기", PokemonType.Fire, 40);
    public static readonly Skill Flamethrower = new("화염방사", PokemonType.Fire, 70);
    public static readonly Skill Overheat = new("오버히트", PokemonType.Fire, 100);

    // Ground
    public static readonly Skill MudSlap = new("진흙뿌리기", PokemonType.Ground, 15);
    public static readonly Skill SandTomb = new("사구몰이", PokemonType.Ground, 25);
    public static readonly Skill Dig = new("다이빙", PokemonType.Ground, 40);
    public static readonly Skill Earthquake = new("지진", PokemonType.Ground, 80);

    // Flying
    public static readonly Skill Gust = new("돌풍", PokemonType.Flying, 20);
    public static readonly Skill WingAttack = new("날개치기", PokemonType.Flying, 40);
    public static readonly Skill AirSlash = new("에어슬래시", PokemonType.Flying, 55);
    public static readonly Skill BraveBird = new("브레이브버드", PokemonType.Flying, 100);

    // Ice
    public static readonly Skill IceShard = new("얼음조각", PokemonType.Ice, 20);
    public static readonly Skill AuroraBeam = new("오로라빔", PokemonType.Ice, 45);
    public static readonly Skill IceBeam = new("냉동빔", PokemonType.Ice, 70);
    public static readonly Skill Blizzard = new("블리자드", PokemonType.Ice, 90);

    // Fighting
    public static readonly Skill LowSweep = new("딴죽걸기", PokemonType.Fighting, 20);
    public static readonly Skill KarateChop = new("가라테촙", PokemonType.Fighting, 35);
    public static readonly Skill CloseCombat = new("인파이트", PokemonType.Fighting, 75);
    public static readonly Skill HighJumpKick = new("필살앞차기", PokemonType.Fighting, 110);

    // Normal
    public static readonly Skill Scratch = new("할퀴기", PokemonType.Normal, 15);
    public static readonly Skill Tackle = new("몸통박치기", PokemonType.Normal, 30);
    public static readonly Skill Slam = new("슬램", PokemonType.Normal, 60);
    public static readonly Skill HyperBeam = new("하이퍼빔", PokemonType.Normal, 120);

    private static readonly IReadOnlyList<Skill> AllSkills =
    [
        WaterGun, BubbleBeam, Surf, HydroPump,
        VineWhip, RazorLeaf, LeafBlade, FrenzyPlant,
        ThunderShock, Discharge, Thunderbolt, Thunder,
        Ember, FireFang, Flamethrower, Overheat,
        MudSlap, SandTomb, Dig, Earthquake,
        Gust, WingAttack, AirSlash, BraveBird,
        IceShard, AuroraBeam, IceBeam, Blizzard,
        LowSweep, KarateChop, CloseCombat, HighJumpKick,
        Scratch, Tackle, Slam, HyperBeam
    ];

    public static IReadOnlyList<Skill> GetRandomMoves(int count = 4)
    {
        if (count < 1 || count > AllSkills.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return AllSkills
            .OrderBy(_ => Random.Shared.Next())
            .Take(count)
            .ToArray();
    }

    public static IReadOnlyList<Skill> GetMovesFor(PokemonType type) => type switch
    {
        PokemonType.Water => [WaterGun, BubbleBeam, Surf, HydroPump],
        PokemonType.Grass => [VineWhip, RazorLeaf, LeafBlade, FrenzyPlant],
        PokemonType.Electric => [ThunderShock, Discharge, Thunderbolt, Thunder],
        PokemonType.Fire => [Ember, FireFang, Flamethrower, Overheat],
        PokemonType.Ground => [MudSlap, SandTomb, Dig, Earthquake],
        PokemonType.Flying => [Gust, WingAttack, AirSlash, BraveBird],
        PokemonType.Ice => [IceShard, AuroraBeam, IceBeam, Blizzard],
        PokemonType.Fighting => [LowSweep, KarateChop, CloseCombat, HighJumpKick],
        PokemonType.Normal => [Scratch, Tackle, Slam, HyperBeam],
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
    };
}

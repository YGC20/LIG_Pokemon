using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Models;

// 타입별 전용 기술 4개(위력 오름차순)를 정의. PokemonDB에서 포켓몬별 무브셋을 구성할 때 사용.
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
    public static readonly Skill SandTomb = new("대지의 힘", PokemonType.Ground, 45);
    public static readonly Skill Dig = new("구멍파기", PokemonType.Ground, 40);
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
    public static readonly Skill LowSweep = new("지구던지기", PokemonType.Fighting, 20);
    public static readonly Skill KarateChop = new("깨뜨리다", PokemonType.Fighting, 35);
    public static readonly Skill CloseCombat = new("인파이트", PokemonType.Fighting, 75);
    public static readonly Skill HighJumpKick = new("필살앞차기", PokemonType.Fighting, 110);

    // Normal
    public static readonly Skill Scratch = new("할퀴기", PokemonType.Normal, 15);
    public static readonly Skill Tackle = new("몸통박치기", PokemonType.Normal, 30);
    public static readonly Skill Slam = new("슬램", PokemonType.Normal, 60);
    public static readonly Skill HyperBeam = new("하이퍼빔", PokemonType.Normal, 120);

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

    // 자기 타입 기술 중 가장 약한 1개를 몸통박치기(아무나 쓸 수 있는 범용기)로 교체.
    // PokemonDB에서 무브셋을 따로 지정하지 않은 포켓몬의 기본 무브셋으로 사용됨.
    public static IReadOnlyList<Skill> GetMovesForWithTackle(PokemonType type)
    {
        if (type == PokemonType.Normal)
        {
            return GetMovesFor(type);
        }

        return GetMovesFor(type)
            .Skip(1)
            .Append(Tackle)
            .OrderBy(skill => skill.Power)
            .ToArray();
    }
}

namespace Pokemon.Models;

internal static class SkillDB
{
    // Normal
    public static readonly Skill Tackle =
        new("몸통박치기", PokemonType.Normal, 40);

    public static readonly Skill HyperBeam =
        new("파괴광선", PokemonType.Normal, 150);

    // Fire
    public static readonly Skill Ember =
        new("불꽃세례", PokemonType.Fire, 40);

    public static readonly Skill Flamethrower =
        new("화염방사", PokemonType.Fire, 90);

    // Water
    public static readonly Skill WaterGun =
        new("물대포", PokemonType.Water, 40);

    public static readonly Skill HydroPump =
        new("하이드로펌프", PokemonType.Water, 110);

    // Electric
    public static readonly Skill ThunderShock =
        new("전기쇼크", PokemonType.Electric, 40);

    public static readonly Skill Thunderbolt =
        new("10만볼트", PokemonType.Electric, 90);

    // Grass
    public static readonly Skill VineWhip =
        new("덩굴채찍", PokemonType.Grass, 45);

    public static readonly Skill RazorLeaf =
        new("잎날가르기", PokemonType.Grass, 55);

    // Ice
    public static readonly Skill IceShard =
        new("얼음뭉치", PokemonType.Ice, 40);

    public static readonly Skill Blizzard =
        new("눈보라", PokemonType.Ice, 110);

    // Fighting
    public static readonly Skill KarateChop =
        new("태권당수", PokemonType.Fighting, 50);

    public static readonly Skill CloseCombat =
        new("인파이트", PokemonType.Fighting, 120);

    // Poison
    public static readonly Skill Acid =
        new("용해액", PokemonType.Poison, 40);

    public static readonly Skill SludgeBomb =
        new("오물폭탄", PokemonType.Poison, 90);

    // Ground
    public static readonly Skill MudSlap =
        new("진흙뿌리기", PokemonType.Ground, 20);

    public static readonly Skill Earthquake =
        new("지진", PokemonType.Ground, 100);

    // Flying
    public static readonly Skill Gust =
        new("바람일으키기", PokemonType.Flying, 40);

    public static readonly Skill AirSlash =
        new("에어슬래시", PokemonType.Flying, 75);

    // Psychic
    public static readonly Skill Confusion =
        new("염동력", PokemonType.Psychic, 50);

    public static readonly Skill Psychic =
        new("사이코키네시스", PokemonType.Psychic, 90);

    // Bug
    public static readonly Skill BugBite =
        new("벌레먹기", PokemonType.Bug, 60);

    public static readonly Skill XScissor =
        new("시저크로스", PokemonType.Bug, 80);

    // Rock
    public static readonly Skill RockThrow =
        new("돌떨구기", PokemonType.Rock, 50);

    public static readonly Skill StoneEdge =
        new("스톤에지", PokemonType.Rock, 100);

    // Ghost
    public static readonly Skill Lick =
        new("핥기", PokemonType.Ghost, 30);

    public static readonly Skill ShadowBall =
        new("섀도볼", PokemonType.Ghost, 80);

    // Dragon
    public static readonly Skill DragonBreath =
        new("용의숨결", PokemonType.Dragon, 60);

    public static readonly Skill DragonClaw =
        new("드래곤클로", PokemonType.Dragon, 80);

    // Dark
    public static readonly Skill Bite =
        new("물기", PokemonType.Dark, 60);

    public static readonly Skill DarkPulse =
        new("악의파동", PokemonType.Dark, 80);

    // Steel
    public static readonly Skill MetalClaw =
        new("메탈클로", PokemonType.Steel, 50);

    public static readonly Skill IronHead =
        new("아이언헤드", PokemonType.Steel, 80);

    // Fairy
    public static readonly Skill FairyWind =
        new("요정의바람", PokemonType.Fairy, 40);

    public static readonly Skill Moonblast =
        new("문포스", PokemonType.Fairy, 95);
}

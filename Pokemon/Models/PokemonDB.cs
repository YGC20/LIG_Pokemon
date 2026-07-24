using System.Collections.Generic;

namespace Pokemon.Models;

// 실제 도감번호를 그대로 사용(스프라이트 파일명과 매칭). 타입은 9종 체계에서 실제 포켓몬과 가장 잘 어울리는 쪽으로 배정.
// (단, 뮤츠는 전기 관장 테마(파치리스+뮤츠)를 맞추기 위해 전기 유지)
// 스킬은 기본적으로 자기 타입 4개(SkillDB.GetMovesFor)를 쓰지만, 실제 포켓몬이 배울 법한 다른 타입 기술이
// 있으면 skills를 직접 넘겨 섞어 씀(예: 잠만보 - 노말 자속 + 땅/격투 기술).
// 딱히 어울리는 다른 타입이 없는 경우엔 노말 기술(할퀴기/몸통박치기)을 채워 넣음(예: 톱치, 야돈).
// 여기 있는 인스턴스는 "템플릿"이며, 실전 로스터를 만들 때는 반드시 Clone()해서 사용해야 함.
internal static class PokemonDB
{
    private static Pokemon Make(int id, string name, PokemonType type, int hp, int atk, int def, IReadOnlyList<Skill>? skills = null) =>
        new(id, name, type, hp, atk, def, skills ?? SkillDB.GetMovesFor(type));

    public static readonly Pokemon Pikachu = Make(25, "피카츄", PokemonType.Electric, 55, 28, 18,
        new[] { SkillDB.ThunderShock, SkillDB.Tackle, SkillDB.Discharge, SkillDB.Thunder });
    public static readonly Pokemon Slowpoke = Make(79, "야돈", PokemonType.Water, 70, 20, 22,
        new[] { SkillDB.Scratch, SkillDB.Tackle, SkillDB.Surf, SkillDB.HydroPump });
    public static readonly Pokemon Magikarp = Make(129, "잉어킹", PokemonType.Water, 35, 8, 8,
        new[] { SkillDB.WaterGun, SkillDB.Tackle, SkillDB.BubbleBeam, SkillDB.Surf });
    public static readonly Pokemon Snorlax = Make(143, "잠만보", PokemonType.Normal, 130, 28, 34,
        new[] { SkillDB.Tackle, SkillDB.CloseCombat, SkillDB.Earthquake, SkillDB.HyperBeam });
    public static readonly Pokemon Dragonite = Make(149, "망나뇽", PokemonType.Flying, 95, 44, 32,
        new[] { SkillDB.IceShard, SkillDB.WingAttack, SkillDB.Discharge, SkillDB.BraveBird });
    public static readonly Pokemon Mewtwo = Make(150, "뮤츠", PokemonType.Electric, 100, 52, 34,
        new[] { SkillDB.Discharge, SkillDB.IceBeam, SkillDB.CloseCombat, SkillDB.Thunder });
    public static readonly Pokemon Heracross = Make(214, "헤라크로스", PokemonType.Fighting, 80, 38, 28,
        new[] { SkillDB.LowSweep, SkillDB.KarateChop, SkillDB.CloseCombat, SkillDB.Earthquake });
    public static readonly Pokemon HoOh = Make(250, "칠색조", PokemonType.Fire, 100, 46, 34,
        new[] { SkillDB.Ember, SkillDB.FireFang, SkillDB.Flamethrower, SkillDB.BraveBird });
    public static readonly Pokemon Mudkip = Make(258, "물짱이", PokemonType.Water, 60, 26, 24,
        new[] { SkillDB.MudSlap, SkillDB.WaterGun, SkillDB.BubbleBeam, SkillDB.Surf });
    public static readonly Pokemon Trapinch = Make(328, "톱치", PokemonType.Ground, 50, 30, 16,
        new[] { SkillDB.Scratch, SkillDB.Tackle, SkillDB.Dig, SkillDB.Earthquake });
    public static readonly Pokemon Castform = Make(351, "캐스퐁", PokemonType.Normal, 55, 24, 20,
        new[] { SkillDB.WaterGun, SkillDB.IceShard, SkillDB.Tackle, SkillDB.FireFang });
    public static readonly Pokemon Piplup = Make(393, "팽도리", PokemonType.Water, 58, 24, 22,
        new[] { SkillDB.WaterGun, SkillDB.AuroraBeam, SkillDB.Surf, SkillDB.HydroPump });
    public static readonly Pokemon Bibarel = Make(400, "비버통", PokemonType.Water, 65, 26, 26,
        new[] { SkillDB.WaterGun, SkillDB.Tackle, SkillDB.BubbleBeam, SkillDB.Slam });
    public static readonly Pokemon Pachirisu = Make(417, "파치리스", PokemonType.Electric, 55, 24, 20,
        new[] { SkillDB.ThunderShock, SkillDB.Tackle, SkillDB.Discharge, SkillDB.Thunderbolt });
    public static readonly Pokemon Drifloon = Make(425, "흔들풍손", PokemonType.Flying, 70, 32, 20,
        new[] { SkillDB.Gust, SkillDB.AuroraBeam, SkillDB.AirSlash, SkillDB.BraveBird });
    public static readonly Pokemon Garchomp = Make(445, "한카리아스", PokemonType.Ground, 95, 48, 30,
        new[] { SkillDB.SandTomb, SkillDB.Dig, SkillDB.Flamethrower, SkillDB.Earthquake });
    public static readonly Pokemon Lucario = Make(448, "루카리오", PokemonType.Fighting, 85, 45, 28,
        new[] { SkillDB.IceShard, SkillDB.KarateChop, SkillDB.CloseCombat, SkillDB.HighJumpKick });
    public static readonly Pokemon Dialga = Make(483, "디아루가", PokemonType.Electric, 105, 50, 38,
        new[] { SkillDB.Discharge, SkillDB.Thunderbolt, SkillDB.IceBeam, SkillDB.Thunder });
    public static readonly Pokemon Palkia = Make(484, "펄기아", PokemonType.Water, 100, 50, 36,
        new[] { SkillDB.WaterGun, SkillDB.BubbleBeam, SkillDB.Thunderbolt, SkillDB.HydroPump });
}

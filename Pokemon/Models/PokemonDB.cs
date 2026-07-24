using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Pokemon.Models;

// 실제 도감번호를 그대로 사용해 스프라이트 파일명과 연결한다.
// 정적 인스턴스는 템플릿이며, 실전 로스터는 CreateRandomRoster로 새 인스턴스를 만든다.
internal static class PokemonDB
{
    private static Pokemon Make(
        int id,
        string name,
        PokemonType type,
        int hp,
        int atk,
        int def,
        IReadOnlyList<Skill>? skills = null) =>
        new(id, name, type, hp, atk, def, skills ?? SkillDB.GetMovesFor(type));

    public static readonly Pokemon Pikachu = Make(25, "피카츄", PokemonType.Electric, 55, 28, 18,
        [SkillDB.ThunderShock, SkillDB.Tackle, SkillDB.Discharge, SkillDB.Thunder]);
    public static readonly Pokemon Slowpoke = Make(79, "야돈", PokemonType.Water, 70, 20, 22,
        [SkillDB.Scratch, SkillDB.Tackle, SkillDB.Surf, SkillDB.HydroPump]);
    public static readonly Pokemon Magikarp = Make(129, "잉어킹", PokemonType.Water, 35, 8, 8,
        [SkillDB.WaterGun, SkillDB.Tackle, SkillDB.BubbleBeam, SkillDB.Surf]);
    public static readonly Pokemon Snorlax = Make(143, "잠만보", PokemonType.Normal, 130, 28, 34,
        [SkillDB.Tackle, SkillDB.CloseCombat, SkillDB.Earthquake, SkillDB.HyperBeam]);
    public static readonly Pokemon Dragonite = Make(149, "망나뇽", PokemonType.Flying, 95, 44, 32,
        [SkillDB.IceShard, SkillDB.WingAttack, SkillDB.Discharge, SkillDB.BraveBird]);
    public static readonly Pokemon Mewtwo = Make(150, "뮤츠", PokemonType.Electric, 100, 52, 34,
        [SkillDB.Discharge, SkillDB.IceBeam, SkillDB.CloseCombat, SkillDB.Thunder]);
    public static readonly Pokemon Heracross = Make(214, "헤라크로스", PokemonType.Fighting, 80, 38, 28,
        [SkillDB.LowSweep, SkillDB.KarateChop, SkillDB.CloseCombat, SkillDB.Earthquake]);
    public static readonly Pokemon HoOh = Make(250, "칠색조", PokemonType.Fire, 100, 46, 34,
        [SkillDB.Ember, SkillDB.FireFang, SkillDB.Flamethrower, SkillDB.BraveBird]);
    public static readonly Pokemon Mudkip = Make(258, "물짱이", PokemonType.Water, 60, 26, 24,
        [SkillDB.MudSlap, SkillDB.WaterGun, SkillDB.BubbleBeam, SkillDB.Surf]);
    public static readonly Pokemon Trapinch = Make(328, "톱치", PokemonType.Ground, 50, 30, 16,
        [SkillDB.Scratch, SkillDB.Tackle, SkillDB.Dig, SkillDB.Earthquake]);
    public static readonly Pokemon Castform = Make(351, "캐스퐁", PokemonType.Normal, 55, 24, 20,
        [SkillDB.WaterGun, SkillDB.IceShard, SkillDB.Tackle, SkillDB.FireFang]);
    public static readonly Pokemon Piplup = Make(393, "팽도리", PokemonType.Water, 58, 24, 22,
        [SkillDB.WaterGun, SkillDB.AuroraBeam, SkillDB.Surf, SkillDB.HydroPump]);
    public static readonly Pokemon Bibarel = Make(400, "비버통", PokemonType.Water, 65, 26, 26,
        [SkillDB.WaterGun, SkillDB.Tackle, SkillDB.BubbleBeam, SkillDB.Slam]);
    public static readonly Pokemon Pachirisu = Make(417, "파치리스", PokemonType.Electric, 55, 24, 20,
        [SkillDB.ThunderShock, SkillDB.Tackle, SkillDB.Discharge, SkillDB.Thunderbolt]);
    public static readonly Pokemon Drifloon = Make(425, "흔들풍손", PokemonType.Flying, 70, 32, 20,
        [SkillDB.Gust, SkillDB.AuroraBeam, SkillDB.AirSlash, SkillDB.BraveBird]);
    public static readonly Pokemon Garchomp = Make(445, "한카리아스", PokemonType.Ground, 95, 48, 30,
        [SkillDB.SandTomb, SkillDB.Dig, SkillDB.Flamethrower, SkillDB.Earthquake]);
    public static readonly Pokemon Lucario = Make(448, "루카리오", PokemonType.Fighting, 85, 45, 28,
        [SkillDB.IceShard, SkillDB.KarateChop, SkillDB.CloseCombat, SkillDB.HighJumpKick]);
    public static readonly Pokemon Dialga = Make(483, "디아루가", PokemonType.Electric, 105, 50, 38,
        [SkillDB.Discharge, SkillDB.Thunderbolt, SkillDB.IceBeam, SkillDB.Thunder]);
    public static readonly Pokemon Palkia = Make(484, "펄기아", PokemonType.Water, 100, 50, 36,
        [SkillDB.WaterGun, SkillDB.BubbleBeam, SkillDB.Thunderbolt, SkillDB.HydroPump]);

    private static readonly IReadOnlyList<Pokemon> AllPokemon =
    [
        Pikachu, Slowpoke, Magikarp, Snorlax, Dragonite,
        Mewtwo, Heracross, HoOh, Mudkip, Trapinch,
        Castform, Piplup, Bibarel, Pachirisu, Drifloon,
        Garchomp, Lucario, Dialga, Palkia,
        .. LoadSpritePokemon()
    ];

    private static IReadOnlyList<Pokemon> LoadSpritePokemon()
    {
        string frontDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "Assets",
            "Pokemon",
            "Front");
        string backDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "Assets",
            "Pokemon",
            "Back");

        if (!Directory.Exists(frontDirectory) ||
            !Directory.Exists(backDirectory))
        {
            return [];
        }

        PokemonType[] types = Enum.GetValues<PokemonType>();
        IReadOnlyDictionary<string, string> koreanNames =
            LoadKoreanNames();

        return Directory
            .EnumerateFiles(frontDirectory, "*.png")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(spriteKey =>
                spriteKey is not null &&
                !int.TryParse(spriteKey, out _) &&
                File.Exists(Path.Combine(
                    backDirectory,
                    $"{spriteKey}.png")))
            .OrderBy(spriteKey => spriteKey)
            .Select((spriteKey, index) =>
            {
                int value = GetStableValue(spriteKey!);
                PokemonType type = types[value % types.Length];

                return new Pokemon(
                    id: 10_000 + index,
                    name: GetPokemonName(spriteKey!, koreanNames),
                    type: type,
                    maxHp: 50 + value % 61,
                    attack: 20 + value % 31,
                    defense: 15 + value % 26,
                    skills: SkillDB.GetMovesFor(type),
                    spriteKey: spriteKey);
            })
            .ToArray();
    }

    private static IReadOnlyDictionary<string, string> LoadKoreanNames()
    {
        string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Assets",
            "Pokemon",
            "pokemon-names.tsv");

        if (!File.Exists(filePath))
        {
            return new Dictionary<string, string>();
        }

        var names = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (string line in File.ReadLines(filePath).Skip(1))
        {
            string[] columns = line.Split('\t', 2);

            if (columns.Length == 2 &&
                !string.IsNullOrWhiteSpace(columns[0]) &&
                !string.IsNullOrWhiteSpace(columns[1]))
            {
                names[columns[0]] = columns[1];
            }
        }

        return names;
    }

    private static int GetStableValue(string value)
    {
        int result = 17;

        foreach (char character in value)
        {
            result = unchecked(result * 31 + character);
        }

        return result & int.MaxValue;
    }

    private static string GetPokemonName(
        string spriteKey,
        IReadOnlyDictionary<string, string> koreanNames)
    {
        if (koreanNames.TryGetValue(spriteKey, out string? koreanName))
        {
            return koreanName;
        }

        string spacedName = spriteKey.Replace('-', ' ');

        return CultureInfo.InvariantCulture.TextInfo
            .ToTitleCase(spacedName);
    }

    public static IReadOnlyList<Pokemon> CreateRandomRoster(int count = 4)
    {
        if (count < 1 || count > AllPokemon.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return AllPokemon
            .OrderBy(_ => Random.Shared.Next())
            .Take(count)
            .Select(template => template.Clone(SkillDB.GetRandomMoves(4)))
            .ToArray();
    }
}

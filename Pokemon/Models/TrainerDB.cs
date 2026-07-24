using System;
using System.IO;
using System.Linq;

namespace Pokemon.Models;

// 배틀이 시작될 때마다 플레이어와 관장에게 무작위 포켓몬 4마리를 배정한다.
internal static class TrainerDB
{
    private const int RosterSize = 4;

    private static readonly string[] Adjectives =
    [
        "짧은 바지",
        "열정적인",
        "수상한",
        "용감한",
        "재빠른",
        "노련한",
        "승부욕 강한",
        "잠이 덜 깬",
        "자신만만한",
        "여행을 좋아하는",
        "포켓몬을 사랑하는",
        "신비로운"
    ];

    private static readonly string[] Roles =
    [
        "소년",
        "소녀",
        "등산가",
        "연구원",
        "무도가",
        "수영선수",
        "포켓몬 애호가",
        "베테랑",
        "캠핑족",
        "예술가",
        "탐험가",
        "엘리트 트레이너"
    ];

    private static readonly string[] GivenNames =
    [
        "기태",
        "민준",
        "서준",
        "지우",
        "하늘",
        "도윤",
        "유진",
        "수빈",
        "예린",
        "가람",
        "태오",
        "나래",
        "현우",
        "다은",
        "시온",
        "은호"
    ];

    private static Trainer CreateRandomTrainer(
        string name,
        string? spriteKey = null)
    {
        var trainer = new Trainer(name, spriteKey);
        trainer.Pokemons.AddRange(PokemonDB.CreateRandomRoster(RosterSize));
        return trainer;
    }

    public static Trainer CreatePlayer(string name) =>
        CreateRandomTrainer(name);

    public static Trainer CreateGymLeader(int bossNumber)
    {
        _ = bossNumber switch
        {
            1 or 2 or 3 or 4 => bossNumber,
            _ => throw new System.ArgumentOutOfRangeException(nameof(bossNumber)),
        };

        return CreateRandomTrainer(
            CreateRandomOpponentName(),
            GetRandomTrainerSpriteKey());
    }

    private static string CreateRandomOpponentName()
    {
        string adjective =
            Adjectives[Random.Shared.Next(Adjectives.Length)];
        string role =
            Roles[Random.Shared.Next(Roles.Length)];
        string givenName =
            GivenNames[Random.Shared.Next(GivenNames.Length)];

        return $"{adjective} {role} {givenName}";
    }

    private static string? GetRandomTrainerSpriteKey()
    {
        string directory = Path.Combine(
            AppContext.BaseDirectory,
            "Assets",
            "Trainers");

        if (!Directory.Exists(directory))
        {
            return null;
        }

        string[] spriteKeys = Directory
            .EnumerateFiles(directory, "*.png")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name!)
            .ToArray();

        return spriteKeys.Length == 0
            ? null
            : spriteKeys[Random.Shared.Next(spriteKeys.Length)];
    }
}

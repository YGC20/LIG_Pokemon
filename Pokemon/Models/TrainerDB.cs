namespace Pokemon.Models;

// 배틀이 시작될 때마다 플레이어와 관장에게 무작위 포켓몬 4마리를 배정한다.
internal static class TrainerDB
{
    private const int RosterSize = 4;

    private static Trainer CreateRandomTrainer(string name)
    {
        var trainer = new Trainer(name);
        trainer.Pokemons.AddRange(PokemonDB.CreateRandomRoster(RosterSize));
        return trainer;
    }

    public static Trainer CreatePlayer(string name) =>
        CreateRandomTrainer(name);

    public static Trainer CreateGymLeader(int bossNumber)
    {
        string name = bossNumber switch
        {
            1 => "물의 관장",
            2 => "전기의 관장",
            3 => "땅의 관장",
            4 => "숲의 수호자",
            _ => throw new System.ArgumentOutOfRangeException(nameof(bossNumber)),
        };

        return CreateRandomTrainer(name);
    }
}

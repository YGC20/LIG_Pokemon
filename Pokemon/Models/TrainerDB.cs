namespace Pokemon.Models;

// 주인공/관장 로스터는 전부 고정. 매 전투마다 새 Trainer/Pokemon 인스턴스를 만들어야
// (Clone) 이전 전투에서 깎인 HP가 다음 전투에 그대로 남는 문제가 없음.
internal static class TrainerDB
{
    public static Trainer CreatePlayer(string name)
    {
        var trainer = new Trainer(name);
        trainer.Pokemons.Add(PokemonDB.Pikachu.Clone());
        trainer.Pokemons.Add(PokemonDB.Mudkip.Clone());
        trainer.Pokemons.Add(PokemonDB.Heracross.Clone());
        trainer.Pokemons.Add(PokemonDB.Lucario.Clone());
        return trainer;
    }

    public static Trainer CreateGymLeader(int bossNumber) => bossNumber switch
    {
        1 => CreateGymLeader1(),
        2 => CreateGymLeader2(),
        3 => CreateGymLeader3(),
        4 => CreateGymLeader4(),
        _ => throw new System.ArgumentOutOfRangeException(nameof(bossNumber)),
    };

    // 물 관장
    private static Trainer CreateGymLeader1()
    {
        var trainer = new Trainer("물의 관장");
        trainer.Pokemons.Add(PokemonDB.Magikarp.Clone());
        trainer.Pokemons.Add(PokemonDB.Piplup.Clone());
        return trainer;
    }

    // 전기 관장
    private static Trainer CreateGymLeader2()
    {
        var trainer = new Trainer("전기의 관장");
        trainer.Pokemons.Add(PokemonDB.Pachirisu.Clone());
        trainer.Pokemons.Add(PokemonDB.Mewtwo.Clone());
        return trainer;
    }

    // 땅 관장
    private static Trainer CreateGymLeader3()
    {
        var trainer = new Trainer("땅의 관장");
        trainer.Pokemons.Add(PokemonDB.Trapinch.Clone());
        trainer.Pokemons.Add(PokemonDB.Garchomp.Clone());
        return trainer;
    }

    // 숲의 수호자
    private static Trainer CreateGymLeader4()
    {
        var trainer = new Trainer("숲의 수호자");
        trainer.Pokemons.Add(PokemonDB.Snorlax.Clone());
        trainer.Pokemons.Add(PokemonDB.Drifloon.Clone());
        return trainer;
    }
}

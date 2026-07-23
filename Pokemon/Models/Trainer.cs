using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon.Models;

public class Trainer
{
    public string Name { get; }

    public List<Pokemon.Pokemon> Pokemons { get; } = [];

    public IEnumerable<Pokemon.Pokemon> AvailablePokemons =>
        Pokemons.Where(pokemon => !pokemon.IsFainted);

    public int RemainingPokemonCount =>
        Pokemons.Count(pokemon => !pokemon.IsFainted);

    // 모든 포켓몬이 쓰러졌는지
    public bool IsDefeated =>
        RemainingPokemonCount == 0;

    public Trainer(string name)
    {
        Name = name;
    }
}
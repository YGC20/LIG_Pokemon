using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon.Models
{
    public class Skill
    {
        public string Name { get; }
        public PokemonType Type { get; }
        public int Power { get; }

        public Skill(string name, PokemonType type, int power)
        {
            Name = name;
            Type = type;
            Power = power;
        }
    }
}

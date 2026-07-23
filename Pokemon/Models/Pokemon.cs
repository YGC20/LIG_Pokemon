using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon.Models
{


    namespace Pokemon
    {
        public class Pokemon
        {
            public int Id { get; }
            public string Name { get; }
            public PokemonType Type { get; }

            public int MaxHp { get; }
            public int CurrentHp { get; private set; }

            public int Attack { get; }
            public int Defense { get; }

            public IReadOnlyList<Skill> Skills { get; }

            public string FrontImagePath =>
                $"/Assets/Pokemon/Front/{Id}.png";

            public string BackImagePath =>
                $"/Assets/Pokemon/Back/{Id}.png";

            public bool IsFainted => CurrentHp <= 0;

            public Pokemon(
                int id,
                string name,
                PokemonType type,
                int maxHp,
                int attack,
                int defense,
                IEnumerable<Skill> skills)
            {
                Id = id;
                Name = name;
                Type = type;

                MaxHp = maxHp;
                CurrentHp = maxHp;

                Attack = attack;
                Defense = defense;

                Skills = skills.ToList();
            }

            public void TakeDamage(int damage)
            {
                CurrentHp = Math.Max(0, CurrentHp - damage);
            }
        }
    }
}
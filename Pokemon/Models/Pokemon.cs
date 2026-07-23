using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon.Models
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

        // 214(헤라크로스)만 뒷모습 파일명이 "214 (2).png"로 예외적으로 저장되어 있음
        private static readonly Dictionary<int, string> BackFileOverrides = new()
        {
            [214] = "214 (2).png",
        };

        public string FrontImagePath =>
            $"Pokemon_image/foward/{Id}.png";

        public string BackImagePath =>
            BackFileOverrides.TryGetValue(Id, out var fileName)
                ? $"Pokemon_image/back/{fileName}"
                : $"Pokemon_image/back/{Id} (1).png";

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

        // PokemonDB의 static 인스턴스는 여러 전투에서 재사용되는 "템플릿"이므로,
        // 실제 전투 로스터를 만들 때는 항상 이 메서드로 HP가 초기화된 새 인스턴스를 만들어야 함.
        public Pokemon Clone() => new(Id, Name, Type, MaxHp, Attack, Defense, Skills);
    }
}

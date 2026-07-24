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
        public string SpriteKey { get; }

        public int MaxHp { get; }
        public int CurrentHp { get; private set; }

        public int Attack { get; }
        public int Defense { get; }

        public IReadOnlyList<Skill> Skills { get; }

        public string FrontImagePath =>
            $"/Assets/Pokemon/Front/{SpriteKey}.png";

        public string BackImagePath =>
            $"/Assets/Pokemon/Back/{SpriteKey}.png";

        public bool IsFainted => CurrentHp <= 0;

        public Pokemon(
            int id,
            string name,
            PokemonType type,
            int maxHp,
            int attack,
            int defense,
            IEnumerable<Skill> skills,
            string? spriteKey = null)
        {
            Id = id;
            Name = name;
            Type = type;
            SpriteKey = spriteKey ?? id.ToString();

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

        /// <returns>실제로 회복된 HP량(최대 HP를 넘지 않도록 보정된 값)</returns>
        public int Heal(int amount)
        {
            int beforeHp = CurrentHp;
            CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
            return CurrentHp - beforeHp;
        }

        // PokemonDB의 static 인스턴스는 여러 전투에서 재사용되는 "템플릿"이므로,
        // 실제 전투 로스터를 만들 때는 항상 이 메서드로 HP가 초기화된 새 인스턴스를 만들어야 함.
        // statMultiplier: 무한 모드처럼 트레이너 단계가 오를수록 상대를 강화할 때 사용(기본 1.0 = 원본 그대로).
        public Pokemon Clone(IEnumerable<Skill>? skills = null, double statMultiplier = 1.0) =>
            new(
                Id,
                Name,
                Type,
                (int)Math.Round(MaxHp * statMultiplier),
                (int)Math.Round(Attack * statMultiplier),
                (int)Math.Round(Defense * statMultiplier),
                skills ?? Skills,
                SpriteKey);
    }
}

namespace Pokemon.Models;

// 실제 도감번호를 그대로 사용(스프라이트 파일명과 매칭). 타입은 5종 체계에 맞춰 임의로 배정.
// 여기 있는 인스턴스는 "템플릿"이며, 실전 로스터를 만들 때는 반드시 Clone()해서 사용해야 함.
internal static class PokemonDB
{
    private static Pokemon Make(int id, string name, PokemonType type, int hp, int atk, int def) =>
        new(id, name, type, hp, atk, def, SkillDB.GetMovesFor(type));

    public static readonly Pokemon Pikachu = Make(25, "피카츄", PokemonType.Electric, 55, 28, 18);
    public static readonly Pokemon Slowpoke = Make(79, "야돈", PokemonType.Water, 70, 20, 22);
    public static readonly Pokemon Magikarp = Make(129, "잉어킹", PokemonType.Water, 35, 8, 8);
    public static readonly Pokemon Snorlax = Make(143, "잠만보", PokemonType.Ground, 130, 28, 34);
    public static readonly Pokemon Dragonite = Make(149, "망나뇽", PokemonType.Fire, 95, 44, 32);
    public static readonly Pokemon Mewtwo = Make(150, "뮤츠", PokemonType.Electric, 100, 52, 34);
    public static readonly Pokemon Heracross = Make(214, "헤라크로스", PokemonType.Grass, 80, 38, 28);
    public static readonly Pokemon HoOh = Make(250, "칠색조", PokemonType.Fire, 100, 46, 34);
    public static readonly Pokemon Mudkip = Make(258, "물짱이", PokemonType.Water, 60, 26, 24);
    public static readonly Pokemon Trapinch = Make(328, "톱치", PokemonType.Ground, 50, 30, 16);
    public static readonly Pokemon Castform = Make(351, "캐스퐁", PokemonType.Water, 55, 24, 20);
    public static readonly Pokemon Piplup = Make(393, "팽도리", PokemonType.Water, 58, 24, 22);
    public static readonly Pokemon Bibarel = Make(400, "비버통", PokemonType.Water, 65, 26, 26);
    public static readonly Pokemon Pachirisu = Make(417, "파치리스", PokemonType.Electric, 55, 24, 20);
    public static readonly Pokemon Drifloon = Make(425, "배루키", PokemonType.Grass, 70, 32, 20);
    public static readonly Pokemon Garchomp = Make(445, "한카리아스", PokemonType.Ground, 95, 48, 30);
    public static readonly Pokemon Lucario = Make(448, "루카리오", PokemonType.Fire, 85, 45, 28);
    public static readonly Pokemon Dialga = Make(483, "디아루가", PokemonType.Electric, 105, 50, 38);
    public static readonly Pokemon Palkia = Make(484, "펄기아", PokemonType.Water, 100, 50, 36);
}

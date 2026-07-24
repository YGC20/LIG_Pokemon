using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon.Models;

// 실전 로스터에 쓸 포켓몬을 전부 여기에 하드코딩한다(스프라이트 자동 스캔 방식은 폐기 —
// 타입과 무관한 랜덤 스킬/랜덤 타입 배정이 뒤섞이는 문제가 있었음).
// 각 포켓몬은 기본적으로 자기 타입 기술 4개를 쓰되, Make()에 skills를 안 넘기면
// SkillDB.GetMovesForWithTackle로 자기 타입 기술 3개 + 몸통박치기(누구나 쓰는 범용기)를 자동 배정한다.
// 특정 포켓몬에게 더 어울리는 조합(예: 잠만보 = 노말+격투+땅)이 있으면 skills를 직접 넘겨서 덮어쓴다.
internal static class PokemonDB
{
    private static Pokemon Make(
        int id,
        string name,
        PokemonType type,
        int hp,
        int atk,
        int def,
        IReadOnlyList<Skill>? skills = null,
        string? spriteKey = null) =>
        new(id, name, type, hp, atk, def, skills ?? SkillDB.GetMovesForWithTackle(type), spriteKey);

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

    // 아래는 스프라이트 리소스(Assets/Pokemon/Front,Back)에 맞춰 새로 추가한 포켓몬들.
    // 무브셋은 지정하지 않아 Make() 기본값(자기 타입 기술 3개 + 몸통박치기)을 그대로 사용함.
    public static readonly Pokemon Abomasnow = Make(10001, "눈설왕", PokemonType.Grass, 83, 20, 39, spriteKey: "abomasnow");
    public static readonly Pokemon Abra = Make(10002, "캐이시", PokemonType.Normal, 93, 24, 38, spriteKey: "abra");
    public static readonly Pokemon Aerodactyl = Make(10003, "프테라", PokemonType.Flying, 79, 42, 34, spriteKey: "aerodactyl");
    public static readonly Pokemon Aggron = Make(10004, "보스로라", PokemonType.Ground, 51, 21, 40, spriteKey: "aggron");
    public static readonly Pokemon Aipom = Make(10005, "에이팜", PokemonType.Normal, 104, 36, 16, spriteKey: "aipom");
    public static readonly Pokemon Altaria = Make(10006, "파비코리", PokemonType.Flying, 77, 33, 16, spriteKey: "altaria");
    public static readonly Pokemon Anorith = Make(10007, "아노딥스", PokemonType.Ground, 51, 40, 33, spriteKey: "anorith");
    public static readonly Pokemon Arcanine = Make(10008, "윈디", PokemonType.Fire, 55, 33, 37, spriteKey: "arcanine");
    public static readonly Pokemon Arceus = Make(10009, "아르세우스", PokemonType.Normal, 75, 26, 15, spriteKey: "arceus");
    public static readonly Pokemon Aron = Make(10010, "가보리", PokemonType.Ground, 78, 37, 20, spriteKey: "aron");
    public static readonly Pokemon Bagon = Make(10011, "아공이", PokemonType.Normal, 98, 37, 15, spriteKey: "bagon");
    public static readonly Pokemon Baltoy = Make(10012, "오뚝군", PokemonType.Ground, 99, 32, 23, spriteKey: "baltoy");
    public static readonly Pokemon Banette = Make(10013, "다크펫", PokemonType.Normal, 103, 37, 17, spriteKey: "banette");
    public static readonly Pokemon Barboach = Make(10014, "미꾸리", PokemonType.Water, 71, 24, 36, spriteKey: "barboach");
    public static readonly Pokemon Bayleef = Make(10015, "베이리프", PokemonType.Grass, 60, 38, 38, spriteKey: "bayleef");
    public static readonly Pokemon Bellossom = Make(10016, "아르코", PokemonType.Grass, 57, 39, 34, spriteKey: "bellossom");
    public static readonly Pokemon Bellsprout = Make(10017, "모다피", PokemonType.Grass, 80, 41, 30, spriteKey: "bellsprout");
    public static readonly Pokemon Bonsly = Make(10018, "꼬지지", PokemonType.Ground, 106, 32, 39, spriteKey: "bonsly");
    public static readonly Pokemon Bronzor = Make(10019, "동미러", PokemonType.Ground, 50, 50, 40, spriteKey: "bronzor");
    public static readonly Pokemon Buneary = Make(10020, "이어롤", PokemonType.Normal, 95, 26, 16, spriteKey: "buneary");
    public static readonly Pokemon Camerupt = Make(10021, "폭타", PokemonType.Fire, 109, 41, 25, spriteKey: "camerupt");
    public static readonly Pokemon Carvanha = Make(10022, "샤프니아", PokemonType.Water, 70, 22, 22, spriteKey: "carvanha");
    public static readonly Pokemon Caterpie = Make(10023, "캐터피", PokemonType.Grass, 99, 26, 15, spriteKey: "caterpie");
    public static readonly Pokemon Chansey = Make(10024, "럭키", PokemonType.Normal, 73, 24, 19, spriteKey: "chansey");
    public static readonly Pokemon Charizard = Make(10025, "리자몽", PokemonType.Fire, 90, 36, 22, spriteKey: "charizard");
    public static readonly Pokemon Charmander = Make(10026, "파이리", PokemonType.Fire, 73, 31, 19, spriteKey: "charmander");
    public static readonly Pokemon Chatot = Make(10027, "페라페", PokemonType.Flying, 95, 27, 33, spriteKey: "chatot");
    public static readonly Pokemon Cherubi = Make(10028, "체리버", PokemonType.Grass, 72, 39, 22, spriteKey: "cherubi");
    public static readonly Pokemon Chikorita = Make(10029, "치코리타", PokemonType.Grass, 101, 27, 30, spriteKey: "chikorita");
    public static readonly Pokemon Chimchar = Make(10030, "불꽃숭이", PokemonType.Fire, 98, 33, 23, spriteKey: "chimchar");
    public static readonly Pokemon Chinchou = Make(10031, "초라기", PokemonType.Water, 89, 36, 19, spriteKey: "chinchou");
    public static readonly Pokemon Chingling = Make(10032, "랑딸랑", PokemonType.Normal, 59, 33, 17, spriteKey: "chingling");
    public static readonly Pokemon Clefable = Make(10033, "픽시", PokemonType.Normal, 77, 47, 28, spriteKey: "clefable");
    public static readonly Pokemon Clefairy = Make(10034, "삐삐", PokemonType.Normal, 56, 36, 19, spriteKey: "clefairy");
    public static readonly Pokemon Cleffa = Make(10035, "삐", PokemonType.Normal, 84, 39, 19, spriteKey: "cleffa");
    public static readonly Pokemon Cloyster = Make(10036, "파르셀", PokemonType.Ice, 110, 29, 19, spriteKey: "cloyster");
    public static readonly Pokemon Combee = Make(10037, "세꿀버리", PokemonType.Flying, 61, 43, 37, spriteKey: "combee");
    public static readonly Pokemon Combusken = Make(10038, "영치코", PokemonType.Fire, 103, 44, 39, spriteKey: "combusken");
    public static readonly Pokemon Crawdaunt = Make(10039, "가재장군", PokemonType.Water, 73, 44, 37, spriteKey: "crawdaunt");
    public static readonly Pokemon Crobat = Make(10040, "크로뱃", PokemonType.Flying, 63, 27, 25, spriteKey: "crobat");
    public static readonly Pokemon Croconaw = Make(10041, "엘리게이", PokemonType.Water, 108, 30, 32, spriteKey: "croconaw");
    public static readonly Pokemon Cyndaquil = Make(10042, "브케인", PokemonType.Fire, 101, 32, 20, spriteKey: "cyndaquil");
    public static readonly Pokemon Darkrai = Make(10043, "다크라이", PokemonType.Fighting, 60, 39, 32, spriteKey: "darkrai");
    public static readonly Pokemon Deoxys = Make(10044, "테오키스", PokemonType.Normal, 73, 26, 36, spriteKey: "deoxys");
    public static readonly Pokemon Dragonair = Make(10045, "신뇽", PokemonType.Normal, 88, 23, 15, spriteKey: "dragonair");
    public static readonly Pokemon Dratini = Make(10046, "미뇽", PokemonType.Normal, 82, 39, 25, spriteKey: "dratini");
    public static readonly Pokemon Drifblim = Make(10047, "둥실라이드", PokemonType.Flying, 93, 25, 19, spriteKey: "drifblim");
    public static readonly Pokemon Drowzee = Make(10048, "슬리프", PokemonType.Normal, 79, 35, 36, spriteKey: "drowzee");
    public static readonly Pokemon Dugtrio = Make(10049, "닥트리오", PokemonType.Ground, 52, 45, 18, spriteKey: "dugtrio");
    public static readonly Pokemon Dustox = Make(10050, "독케일", PokemonType.Grass, 104, 31, 17, spriteKey: "dustox");
    public static readonly Pokemon Ekans = Make(10051, "아보", PokemonType.Ground, 91, 42, 26, spriteKey: "ekans");
    public static readonly Pokemon Electabuzz = Make(10052, "에레브", PokemonType.Electric, 90, 37, 33, spriteKey: "electabuzz");
    public static readonly Pokemon Electivire = Make(10053, "에레키블", PokemonType.Electric, 65, 47, 22, spriteKey: "electivire");
    public static readonly Pokemon Electrode = Make(10054, "붐볼", PokemonType.Electric, 59, 48, 27, spriteKey: "electrode");
    public static readonly Pokemon Elekid = Make(10055, "에레키드", PokemonType.Electric, 86, 42, 16, spriteKey: "elekid");
    public static readonly Pokemon Empoleon = Make(10056, "엠페르트", PokemonType.Water, 93, 35, 21, spriteKey: "empoleon");
    public static readonly Pokemon Entei = Make(10057, "앤테이", PokemonType.Fire, 74, 32, 15, spriteKey: "entei");
    public static readonly Pokemon Espeon = Make(10058, "에브이", PokemonType.Normal, 65, 21, 40, spriteKey: "espeon");
    public static readonly Pokemon Exploud = Make(10059, "폭음룡", PokemonType.Normal, 87, 32, 31, spriteKey: "exploud");
    public static readonly Pokemon Farfetchd = Make(10060, "파오리", PokemonType.Flying, 86, 21, 15, spriteKey: "farfetchd");
    public static readonly Pokemon Finneon = Make(10061, "형광어", PokemonType.Water, 54, 42, 37, spriteKey: "finneon");
    public static readonly Pokemon Flaaffy = Make(10062, "보송송", PokemonType.Electric, 87, 22, 17, spriteKey: "flaaffy");
    public static readonly Pokemon Flareon = Make(10063, "부스터", PokemonType.Fire, 90, 42, 19, spriteKey: "flareon");
    public static readonly Pokemon Flygon = Make(10064, "플라이곤", PokemonType.Ground, 91, 21, 17, spriteKey: "flygon");
    public static readonly Pokemon Forretress = Make(10065, "쏘콘", PokemonType.Ground, 76, 20, 33, spriteKey: "forretress");
    public static readonly Pokemon Gabite = Make(10066, "한바이트", PokemonType.Ground, 91, 43, 36, spriteKey: "gabite");
    public static readonly Pokemon Gallade = Make(10067, "엘레이드", PokemonType.Fighting, 56, 33, 22, spriteKey: "gallade");
    public static readonly Pokemon Gligar = Make(10068, "글라이거", PokemonType.Flying, 107, 25, 36, spriteKey: "gligar");
    public static readonly Pokemon Gliscor = Make(10069, "글라이온", PokemonType.Flying, 97, 46, 27, spriteKey: "gliscor");
    public static readonly Pokemon Gorebyss = Make(10070, "분홍장이", PokemonType.Water, 99, 50, 38, spriteKey: "gorebyss");
    public static readonly Pokemon Grimer = Make(10071, "질퍽이", PokemonType.Ground, 88, 25, 30, spriteKey: "grimer");
    public static readonly Pokemon Growlithe = Make(10072, "가디", PokemonType.Fire, 62, 34, 19, spriteKey: "growlithe");
    public static readonly Pokemon Grumpig = Make(10073, "피그킹", PokemonType.Normal, 75, 35, 19, spriteKey: "grumpig");
    public static readonly Pokemon Gyarados = Make(10074, "갸라도스", PokemonType.Water, 81, 42, 22, spriteKey: "gyarados");
    public static readonly Pokemon Happiny = Make(10075, "핑복", PokemonType.Normal, 81, 20, 17, spriteKey: "happiny");
    public static readonly Pokemon Hariyama = Make(10076, "하리뭉", PokemonType.Fighting, 77, 49, 30, spriteKey: "hariyama");
    public static readonly Pokemon Haunter = Make(10077, "고우스트", PokemonType.Normal, 99, 44, 21, spriteKey: "haunter");
    public static readonly Pokemon Heatran = Make(10078, "히드런", PokemonType.Fire, 54, 40, 21, spriteKey: "heatran");
    public static readonly Pokemon Hitmonchan = Make(10079, "홍수몬", PokemonType.Fighting, 99, 34, 17, spriteKey: "hitmonchan");
    public static readonly Pokemon Hoppip = Make(10080, "통통코", PokemonType.Grass, 99, 23, 38, spriteKey: "hoppip");
    public static readonly Pokemon Horsea = Make(10081, "쏘드라", PokemonType.Water, 83, 39, 16, spriteKey: "horsea");
    public static readonly Pokemon Houndoom = Make(10082, "헬가", PokemonType.Fire, 73, 49, 31, spriteKey: "houndoom");
    public static readonly Pokemon Hypno = Make(10083, "슬리퍼", PokemonType.Normal, 66, 38, 18, spriteKey: "hypno");
    public static readonly Pokemon Illumise = Make(10084, "네오비트", PokemonType.Grass, 55, 48, 40, spriteKey: "illumise");
    public static readonly Pokemon Infernape = Make(10085, "초염몽", PokemonType.Fire, 76, 44, 22, spriteKey: "infernape");
    public static readonly Pokemon Ivysaur = Make(10086, "이상해풀", PokemonType.Grass, 59, 44, 21, spriteKey: "ivysaur");
    public static readonly Pokemon Jigglypuff = Make(10087, "푸린", PokemonType.Normal, 67, 34, 37, spriteKey: "jigglypuff");
    public static readonly Pokemon Jirachi = Make(10088, "지라치", PokemonType.Normal, 82, 33, 16, spriteKey: "jirachi");
    public static readonly Pokemon Jynx = Make(10089, "루주라", PokemonType.Ice, 98, 47, 25, spriteKey: "jynx");
    public static readonly Pokemon Kabuto = Make(10090, "투구", PokemonType.Water, 66, 22, 28, spriteKey: "kabuto");
    public static readonly Pokemon Kakuna = Make(10091, "딱충이", PokemonType.Normal, 73, 39, 17, spriteKey: "kakuna");
    public static readonly Pokemon Kirlia = Make(10092, "킬리아", PokemonType.Normal, 62, 39, 26, spriteKey: "kirlia");
    public static readonly Pokemon Krabby = Make(10093, "크랩", PokemonType.Water, 67, 32, 15, spriteKey: "krabby");
    public static readonly Pokemon Kyogre = Make(10094, "가이오가", PokemonType.Water, 108, 43, 33, spriteKey: "kyogre");
    public static readonly Pokemon Latias = Make(10095, "라티아스", PokemonType.Flying, 96, 26, 20, spriteKey: "latias");
    public static readonly Pokemon Ledian = Make(10096, "레디안", PokemonType.Flying, 74, 21, 21, spriteKey: "ledian");
    public static readonly Pokemon Lileep = Make(10097, "릴링", PokemonType.Grass, 63, 23, 37, spriteKey: "lileep");
    public static readonly Pokemon Lombre = Make(10098, "로토스", PokemonType.Water, 104, 43, 17, spriteKey: "lombre");
    public static readonly Pokemon Lugia = Make(10099, "루기아", PokemonType.Flying, 108, 24, 38, spriteKey: "lugia");
    public static readonly Pokemon Luxio = Make(10100, "럭시오", PokemonType.Electric, 50, 38, 35, spriteKey: "luxio");
    public static readonly Pokemon Machoke = Make(10101, "근육몬", PokemonType.Fighting, 89, 27, 28, spriteKey: "machoke");
    public static readonly Pokemon Magby = Make(10102, "마그비", PokemonType.Fire, 107, 48, 24, spriteKey: "magby");
    public static readonly Pokemon Magmortar = Make(10103, "마그마번", PokemonType.Fire, 91, 32, 38, spriteKey: "magmortar");
    public static readonly Pokemon Magnemite = Make(10104, "코일", PokemonType.Electric, 74, 50, 21, spriteKey: "magnemite");
    public static readonly Pokemon Magneton = Make(10105, "레어코일", PokemonType.Electric, 74, 27, 31, spriteKey: "magneton");
    public static readonly Pokemon Makuhita = Make(10106, "마크탕", PokemonType.Fighting, 96, 45, 40, spriteKey: "makuhita");
    public static readonly Pokemon Manectric = Make(10107, "썬더볼트", PokemonType.Electric, 82, 42, 28, spriteKey: "manectric");
    public static readonly Pokemon Marowak = Make(10108, "텅구리", PokemonType.Ground, 90, 33, 34, spriteKey: "marowak");
    public static readonly Pokemon Marshtomp = Make(10109, "늪짱이", PokemonType.Water, 82, 20, 35, spriteKey: "marshtomp");
    public static readonly Pokemon Mawile = Make(10110, "입치트", PokemonType.Ground, 86, 43, 25, spriteKey: "mawile");
    public static readonly Pokemon Meganium = Make(10111, "메가니움", PokemonType.Grass, 70, 22, 39, spriteKey: "meganium");
    public static readonly Pokemon Metagross = Make(10112, "메타그로스", PokemonType.Ground, 71, 45, 15, spriteKey: "metagross");
    public static readonly Pokemon Metapod = Make(10113, "단데기", PokemonType.Normal, 102, 26, 40, spriteKey: "metapod");
    public static readonly Pokemon Mew = Make(10114, "뮤", PokemonType.Normal, 104, 46, 29, spriteKey: "mew");
    public static readonly Pokemon Mightyena = Make(10115, "그라에나", PokemonType.Fighting, 55, 28, 28, spriteKey: "mightyena");
    public static readonly Pokemon Miltank = Make(10116, "밀탱크", PokemonType.Normal, 61, 33, 26, spriteKey: "miltank");
    public static readonly Pokemon Misdreavus = Make(10117, "무우마", PokemonType.Normal, 105, 23, 35, spriteKey: "misdreavus");
    public static readonly Pokemon Moltres = Make(10118, "파이어", PokemonType.Fire, 88, 41, 28, spriteKey: "moltres");
    public static readonly Pokemon Monferno = Make(10119, "파이숭이", PokemonType.Fire, 105, 47, 28, spriteKey: "monferno");
    public static readonly Pokemon Mothim = Make(10120, "나메일", PokemonType.Flying, 108, 20, 22, spriteKey: "mothim");
    public static readonly Pokemon MrMime = Make(10121, "마임맨", PokemonType.Normal, 92, 27, 24, spriteKey: "mr-mime");
    public static readonly Pokemon Muk = Make(10122, "질뻐기", PokemonType.Ground, 100, 34, 19, spriteKey: "muk");
    public static readonly Pokemon Munchlax = Make(10123, "먹고자", PokemonType.Normal, 57, 50, 38, spriteKey: "munchlax");
    public static readonly Pokemon Nidoking = Make(10124, "니드킹", PokemonType.Ground, 58, 50, 31, spriteKey: "nidoking");
    public static readonly Pokemon NidoranF = Make(10125, "니드런♀", PokemonType.Ground, 98, 47, 24, spriteKey: "nidoran-f");
    public static readonly Pokemon Nidorino = Make(10126, "니드리노", PokemonType.Ground, 105, 27, 30, spriteKey: "nidorino");
    public static readonly Pokemon Nosepass = Make(10127, "코코파스", PokemonType.Ground, 58, 25, 38, spriteKey: "nosepass");
    public static readonly Pokemon Octillery = Make(10128, "대포무노", PokemonType.Water, 93, 32, 39, spriteKey: "octillery");
    public static readonly Pokemon Omanyte = Make(10129, "암나이트", PokemonType.Water, 59, 25, 39, spriteKey: "omanyte");
    public static readonly Pokemon Parasect = Make(10130, "파라섹트", PokemonType.Grass, 110, 48, 27, spriteKey: "parasect");
    public static readonly Pokemon Persian = Make(10131, "페르시온", PokemonType.Normal, 55, 34, 24, spriteKey: "persian");
    public static readonly Pokemon Pidgeot = Make(10132, "피죤투", PokemonType.Flying, 80, 40, 40, spriteKey: "pidgeot");
    public static readonly Pokemon Pidgeotto = Make(10133, "피죤", PokemonType.Flying, 51, 47, 31, spriteKey: "pidgeotto");
    public static readonly Pokemon Pineco = Make(10134, "피콘", PokemonType.Grass, 69, 22, 36, spriteKey: "pineco");
    public static readonly Pokemon Pinsir = Make(10135, "쁘사이저", PokemonType.Fighting, 109, 25, 29, spriteKey: "pinsir");
    public static readonly Pokemon Poliwag = Make(10136, "발챙이", PokemonType.Water, 54, 27, 19, spriteKey: "poliwag");
    public static readonly Pokemon Ponyta = Make(10137, "포니타", PokemonType.Fire, 107, 39, 15, spriteKey: "ponyta");
    public static readonly Pokemon Porygon = Make(10138, "폴리곤", PokemonType.Normal, 74, 34, 38, spriteKey: "porygon");
    public static readonly Pokemon Porygon2 = Make(10139, "폴리곤2", PokemonType.Normal, 89, 32, 36, spriteKey: "porygon2");
    public static readonly Pokemon Primeape = Make(10140, "성원숭", PokemonType.Fighting, 103, 50, 37, spriteKey: "primeape");
    public static readonly Pokemon Pupitar = Make(10141, "데기라스", PokemonType.Ground, 99, 38, 15, spriteKey: "pupitar");
    public static readonly Pokemon Quilava = Make(10142, "마그케인", PokemonType.Fire, 104, 50, 23, spriteKey: "quilava");
    public static readonly Pokemon Raichu = Make(10143, "라이츄", PokemonType.Electric, 95, 28, 18, spriteKey: "raichu");
    public static readonly Pokemon Ralts = Make(10144, "랄토스", PokemonType.Normal, 62, 42, 30, spriteKey: "ralts");
    public static readonly Pokemon Rampardos = Make(10145, "램펄드", PokemonType.Ground, 107, 25, 19, spriteKey: "rampardos");
    public static readonly Pokemon Raticate = Make(10146, "레트라", PokemonType.Normal, 51, 45, 31, spriteKey: "raticate");
    public static readonly Pokemon Rayquaza = Make(10147, "레쿠쟈", PokemonType.Flying, 51, 41, 26, spriteKey: "rayquaza");
    public static readonly Pokemon Regirock = Make(10148, "레지락", PokemonType.Ground, 109, 47, 32, spriteKey: "regirock");
    public static readonly Pokemon Relicanth = Make(10149, "시라칸", PokemonType.Water, 95, 36, 28, spriteKey: "relicanth");
    public static readonly Pokemon Remoraid = Make(10150, "총어", PokemonType.Water, 83, 40, 27, spriteKey: "remoraid");
    public static readonly Pokemon Salamence = Make(10151, "보만다", PokemonType.Flying, 87, 23, 31, spriteKey: "salamence");
    public static readonly Pokemon Sandshrew = Make(10152, "모래두지", PokemonType.Ground, 87, 39, 35, spriteKey: "sandshrew");
    public static readonly Pokemon Scyther = Make(10153, "스라크", PokemonType.Flying, 110, 36, 40, spriteKey: "scyther");
    public static readonly Pokemon Seaking = Make(10154, "왕콘치", PokemonType.Water, 105, 25, 26, spriteKey: "seaking");
    public static readonly Pokemon Seviper = Make(10155, "세비퍼", PokemonType.Ground, 80, 36, 16, spriteKey: "seviper");
    public static readonly Pokemon Shellos = Make(10156, "깝질무", PokemonType.Water, 52, 37, 28, spriteKey: "shellos");
    public static readonly Pokemon Shiftry = Make(10157, "다탱구", PokemonType.Grass, 61, 43, 23, spriteKey: "shiftry");
    public static readonly Pokemon Shroomish = Make(10158, "버섯꼬", PokemonType.Grass, 58, 28, 34, spriteKey: "shroomish");
    public static readonly Pokemon Shuckle = Make(10159, "단단지", PokemonType.Ground, 59, 23, 35, spriteKey: "shuckle");
    public static readonly Pokemon Silcoon = Make(10160, "실쿤", PokemonType.Normal, 110, 32, 25, spriteKey: "silcoon");
    public static readonly Pokemon Skitty = Make(10161, "에나비", PokemonType.Normal, 108, 32, 28, spriteKey: "skitty");
    public static readonly Pokemon Skuntank = Make(10162, "스컹탱크", PokemonType.Ground, 73, 48, 29, spriteKey: "skuntank");
    public static readonly Pokemon Smoochum = Make(10163, "뽀뽀라", PokemonType.Ice, 101, 48, 37, spriteKey: "smoochum");
    public static readonly Pokemon Snover = Make(10164, "눈쓰개", PokemonType.Grass, 53, 25, 21, spriteKey: "snover");
    public static readonly Pokemon Solrock = Make(10165, "솔록", PokemonType.Fire, 84, 29, 21, spriteKey: "solrock");
    public static readonly Pokemon Spoink = Make(10166, "피그점프", PokemonType.Normal, 55, 49, 22, spriteKey: "spoink");
    public static readonly Pokemon Squirtle = Make(10167, "꼬부기", PokemonType.Water, 82, 36, 25, spriteKey: "squirtle");
    public static readonly Pokemon Stantler = Make(10168, "노라키", PokemonType.Normal, 81, 47, 35, spriteKey: "stantler");
    public static readonly Pokemon Steelix = Make(10169, "강철톤", PokemonType.Ground, 84, 42, 20, spriteKey: "steelix");
    public static readonly Pokemon Suicune = Make(10170, "스이쿤", PokemonType.Water, 106, 23, 36, spriteKey: "suicune");
    public static readonly Pokemon Sunflora = Make(10171, "해루미", PokemonType.Grass, 94, 30, 20, spriteKey: "sunflora");
    public static readonly Pokemon Sunkern = Make(10172, "해너츠", PokemonType.Grass, 80, 32, 20, spriteKey: "sunkern");
    public static readonly Pokemon Surskit = Make(10173, "비구술", PokemonType.Water, 69, 38, 17, spriteKey: "surskit");
    public static readonly Pokemon Swablu = Make(10174, "파비코", PokemonType.Flying, 80, 28, 28, spriteKey: "swablu");
    public static readonly Pokemon Swalot = Make(10175, "꿀꺽몬", PokemonType.Ground, 83, 27, 32, spriteKey: "swalot");
    public static readonly Pokemon Swellow = Make(10176, "스왈로", PokemonType.Flying, 66, 41, 29, spriteKey: "swellow");
    public static readonly Pokemon Tangela = Make(10177, "덩쿠리", PokemonType.Grass, 83, 48, 28, spriteKey: "tangela");
    public static readonly Pokemon Teddiursa = Make(10178, "깜지곰", PokemonType.Normal, 79, 28, 29, spriteKey: "teddiursa");
    public static readonly Pokemon Tentacool = Make(10179, "왕눈해", PokemonType.Water, 76, 31, 27, spriteKey: "tentacool");
    public static readonly Pokemon Torkoal = Make(10180, "코터스", PokemonType.Fire, 96, 28, 28, spriteKey: "torkoal");
    public static readonly Pokemon Totodile = Make(10181, "리아코", PokemonType.Water, 88, 43, 24, spriteKey: "totodile");
    public static readonly Pokemon Treecko = Make(10182, "나무지기", PokemonType.Grass, 90, 31, 21, spriteKey: "treecko");
    public static readonly Pokemon Typhlosion = Make(10183, "블레이범", PokemonType.Fire, 79, 24, 29, spriteKey: "typhlosion");
    public static readonly Pokemon Unown = Make(10184, "안농", PokemonType.Normal, 103, 37, 27, spriteKey: "unown");
    public static readonly Pokemon Vaporeon = Make(10185, "샤미드", PokemonType.Water, 56, 43, 32, spriteKey: "vaporeon");
    public static readonly Pokemon Venomoth = Make(10186, "도나리", PokemonType.Flying, 64, 35, 28, spriteKey: "venomoth");
    public static readonly Pokemon Venusaur = Make(10187, "이상해꽃", PokemonType.Grass, 95, 45, 33, spriteKey: "venusaur");
    public static readonly Pokemon Vespiquen = Make(10188, "비퀸", PokemonType.Flying, 89, 20, 26, spriteKey: "vespiquen");
    public static readonly Pokemon Voltorb = Make(10189, "찌리리공", PokemonType.Electric, 103, 47, 26, spriteKey: "voltorb");
    public static readonly Pokemon Wailmer = Make(10190, "고래왕자", PokemonType.Water, 98, 32, 23, spriteKey: "wailmer");
    public static readonly Pokemon Wailord = Make(10191, "고래왕", PokemonType.Water, 91, 49, 20, spriteKey: "wailord");
    public static readonly Pokemon Weedle = Make(10192, "뿔충이", PokemonType.Grass, 64, 43, 18, spriteKey: "weedle");
    public static readonly Pokemon Weezing = Make(10193, "또도가스", PokemonType.Ground, 54, 21, 33, spriteKey: "weezing");
    public static readonly Pokemon Whiscash = Make(10194, "메깅", PokemonType.Water, 95, 38, 34, spriteKey: "whiscash");
    public static readonly Pokemon Wingull = Make(10195, "갈모매", PokemonType.Flying, 86, 26, 36, spriteKey: "wingull");
    public static readonly Pokemon Wurmple = Make(10196, "개무소", PokemonType.Grass, 102, 50, 16, spriteKey: "wurmple");
    public static readonly Pokemon Xatu = Make(10197, "네이티오", PokemonType.Flying, 109, 44, 32, spriteKey: "xatu");
    public static readonly Pokemon Yanma = Make(10198, "왕자리", PokemonType.Flying, 91, 24, 34, spriteKey: "yanma");
    public static readonly Pokemon Zangoose = Make(10199, "쟝고", PokemonType.Normal, 63, 25, 34, spriteKey: "zangoose");
    public static readonly Pokemon Zapdos = Make(10200, "썬더", PokemonType.Electric, 75, 26, 29, spriteKey: "zapdos");

    private static readonly IReadOnlyList<Pokemon> AllPokemon =
    [
        Pikachu, Slowpoke, Magikarp, Snorlax, Dragonite,
        Mewtwo, Heracross, HoOh, Mudkip, Trapinch,
        Castform, Piplup, Bibarel, Pachirisu, Drifloon,
        Garchomp, Lucario, Dialga, Palkia,
        Abomasnow, Abra, Aerodactyl, Aggron, Aipom, Altaria, Anorith, Arcanine,
        Arceus, Aron, Bagon, Baltoy, Banette, Barboach, Bayleef, Bellossom,
        Bellsprout, Bonsly, Bronzor, Buneary, Camerupt, Carvanha, Caterpie, Chansey,
        Charizard, Charmander, Chatot, Cherubi, Chikorita, Chimchar, Chinchou, Chingling,
        Clefable, Clefairy, Cleffa, Cloyster, Combee, Combusken, Crawdaunt, Crobat,
        Croconaw, Cyndaquil, Darkrai, Deoxys, Dragonair, Dratini, Drifblim, Drowzee,
        Dugtrio, Dustox, Ekans, Electabuzz, Electivire, Electrode, Elekid, Empoleon,
        Entei, Espeon, Exploud, Farfetchd, Finneon, Flaaffy, Flareon, Flygon,
        Forretress, Gabite, Gallade, Gligar, Gliscor, Gorebyss, Grimer, Growlithe,
        Grumpig, Gyarados, Happiny, Hariyama, Haunter, Heatran, Hitmonchan, Hoppip,
        Horsea, Houndoom, Hypno, Illumise, Infernape, Ivysaur, Jigglypuff, Jirachi,
        Jynx, Kabuto, Kakuna, Kirlia, Krabby, Kyogre, Latias, Ledian,
        Lileep, Lombre, Lugia, Luxio, Machoke, Magby, Magmortar, Magnemite,
        Magneton, Makuhita, Manectric, Marowak, Marshtomp, Mawile, Meganium, Metagross,
        Metapod, Mew, Mightyena, Miltank, Misdreavus, Moltres, Monferno, Mothim,
        MrMime, Muk, Munchlax, Nidoking, NidoranF, Nidorino, Nosepass, Octillery,
        Omanyte, Parasect, Persian, Pidgeot, Pidgeotto, Pineco, Pinsir, Poliwag,
        Ponyta, Porygon, Porygon2, Primeape, Pupitar, Quilava, Raichu, Ralts,
        Rampardos, Raticate, Rayquaza, Regirock, Relicanth, Remoraid, Salamence, Sandshrew,
        Scyther, Seaking, Seviper, Shellos, Shiftry, Shroomish, Shuckle, Silcoon,
        Skitty, Skuntank, Smoochum, Snover, Solrock, Spoink, Squirtle, Stantler,
        Steelix, Suicune, Sunflora, Sunkern, Surskit, Swablu, Swalot, Swellow,
        Tangela, Teddiursa, Tentacool, Torkoal, Totodile, Treecko, Typhlosion, Unown,
        Vaporeon, Venomoth, Venusaur, Vespiquen, Voltorb, Wailmer, Wailord, Weedle,
        Weezing, Whiscash, Wingull, Wurmple, Xatu, Yanma, Zangoose, Zapdos,
    ];

    public static IReadOnlyList<Pokemon> CreateRandomRoster(int count = 4)
    {
        if (count < 1 || count > AllPokemon.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return AllPokemon
            .OrderBy(_ => Random.Shared.Next())
            .Take(count)
            .Select(template => template.Clone())
            .ToArray();
    }
}

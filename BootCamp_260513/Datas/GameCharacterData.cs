namespace RPGGame.Datas
{
    internal class GameCharacterData
    {
        // 캐릭터에 공통으로 들어가는 데이터들 모아둠
        public string ImagePath { get; private set; }   // 이미지
        public string Name { get; private set; }        // 이름
        public int Hp { get; private set; }             // 체력
        public int Mp { get; private set; }             // 마나
        public int Attack { get; private set; }         // 공격력
        public int Defense { get; private set; }        // 방어력

        // 캐릭터 Init()으로 초기화 시 이 데이터를 받음 
        public GameCharacterData(string path, string name, int hp, int mp, int attack, int defense = 1) 
        {
            ImagePath = path;
            Name = name;
            Hp = hp;
            Mp = mp;
            Attack = attack;
            Defense = defense;
        }
    }

    // 아이템 보너스 스탯을 추가해줌
    internal class BonusStat
    {
        public int Atk { get; private set; }
        public int Def { get; private set; }

        public void Add(int atk, int def)
        {
            Atk += atk;
            Def += def;
        }
    }
}

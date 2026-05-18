namespace RPGGame.Datas
{
    // 직업 데이터베이스
    internal class JobDataBase
    {
        public static GameCharacterData[] JobDatas =
        {
                                // 이미지, 이름, 체력, 마나, 공격력, 방어력 순
            new GameCharacterData ("Warrior.png", "전사", 200, 50, 20, 10),
            new GameCharacterData ("Mage.png", "마법사", 80, 200, 12, 5),
            new GameCharacterData ("Archer.png", "아처", 130, 100, 16, 7)
        };
    } 
}
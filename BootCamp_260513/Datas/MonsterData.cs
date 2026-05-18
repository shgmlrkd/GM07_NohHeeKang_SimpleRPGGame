namespace RPGGame.Datas
{
    internal class MonsterData : GameCharacterData
    {
        // 캐릭터 데이터에서 몬스터만 갖고 있는 데이터 따로 만듦
        // 플레이어한테 줄 경험치, 골드를 만듦
        public int RewardExp { get; private set; }
        public int RewardGold { get; private set; }
        public MonsterData(string path, string name, int hp, int mp, int attack, int defense, int rewardExp, int rewardGold) 
            : base(path, name, hp, mp, attack, defense)
        {
            RewardExp = rewardExp;
            RewardGold = rewardGold;
        }
    }
}

namespace RPGGame.Datas
{
    // 플레이어의 스킬 데이터 (이름, 계수, 비용)을 따로 저장
    internal class SkillData
    {
        public string Name { get; private set; }
        public float Multiplier { get; private set; }
        public int Cost { get; private set; }

        public SkillData(string name, float multiplier, int cost)
        {
            Name = name;
            Multiplier = multiplier;
            Cost = cost;
        }
    }
}
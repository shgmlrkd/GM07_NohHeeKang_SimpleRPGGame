using RPGGame.Utility;
using RPGGame.Datas;

namespace RPGGame.Characters
{
    internal class Monster : Character
    {
        public MonsterData MData { get; private set; }

        // 몬스터는 플레이어의 레벨 범위에 따라 Normal, Elite, Boss 등급으로 나오는 몬스터가 달라짐
        public Monster(int playerLevel)
        { 
            // 범위에 맞는 몬스터 데이터 리스트
            List<MonsterData> datas = MonsterDataBase.GetAvailableMonsters(playerLevel);
            // 랜덤하게 인덱스 뽑음
            int index = RandomHelper.Next(0, datas.Count);
            // 실제로 플레이어가 상대할 몬스터 데이터를 저장
            MData = datas[index];
            CharacterData = MData;
            // 초기화
            Init();
        }
    }
}
using RPGGame.Enums;

namespace RPGGame.Datas
{
    // 몬스터는 노말, 엘리트, 보스를 enum타입으로 만들어 키값으로 놓고
    // 몬스터 데이터를 리스트로 묶어서 딕셔너리의 벨류값으로 넣음
    internal class MonsterDataBase
    {
        public static Dictionary<MonsterTypeEnum, List<MonsterData>> DictMonsterDatas = new Dictionary<MonsterTypeEnum, List<MonsterData>>
        {
            // 노말 몬스터
            {
                MonsterTypeEnum.Normal,
                new List<MonsterData>
                {
                    new("Slime.png", "슬라임", 10, 5, 5, 5, 3, 5),
                    new("Goblin.png", "고블린", 30, 20, 12, 7, 8, 8),
                    new("Orc.png", "오크", 100, 80, 18, 12, 15, 16),
                }
            },

            // 엘리트 몬스터
            {
                MonsterTypeEnum.Elite,
                new List<MonsterData>
                {
                    new("Golem.png", "골렘", 300, 50, 10, 30, 20, 35),
                }
            },

            // 보스 몬스터
            {
                MonsterTypeEnum.Boss,
                new List<MonsterData>
                {
                    new("Dragon.png", "드래곤", 220, 150, 30, 20, 50, 100),
                }
            }
        };

        // 플레이어의 레벨에 따라 몬스터 데이터 리스트가 달라짐
        // ex)
        // 플레이어 레벨 : 7 이면 노말, 엘리트 몬스터가 result로 들감
        // 플레이어 레벨 : 9 이면 노말, 엘리트, 보스 몬스터가 result로 들감
        public static List<MonsterData> GetAvailableMonsters(int playerLevel)
        {
            List<MonsterData> result = new List<MonsterData>();

            result.AddRange(DictMonsterDatas[MonsterTypeEnum.Normal]);

            if(playerLevel >= 6)
            {
                result.AddRange(DictMonsterDatas[MonsterTypeEnum.Elite]);
            }

            if(playerLevel >= 9)
            {
                result.AddRange(DictMonsterDatas[MonsterTypeEnum.Boss]);
            }

            return result;
        }
    }
}      
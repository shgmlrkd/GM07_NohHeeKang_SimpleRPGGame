using RPGGame.Enums;

namespace RPGGame.Datas
{
    // 스킬 데이터도 딕셔너리로 키값은 직업 타입, 벨류값은 스킬 데이터를 리스트로 담은 것으로 만듦
    internal static class SkillDataBase
    {
        private static Dictionary<JobTypeEnum, List<SkillData>> dictSkillDatas
                                                                = new Dictionary<JobTypeEnum, List<SkillData>>();
        
        static SkillDataBase() 
        {
            dictSkillDatas[JobTypeEnum.Warrior] = new List<SkillData>()
            {
                new SkillData("파워 스트라이크", 2.0f, 10),
                new SkillData("슬래시 블러스트", 2.6f, 22),
                new SkillData("혼돈 회오리", 3.5f, 45),
                new SkillData("회오리감자", 10.0f, 45)
            };

            dictSkillDatas[JobTypeEnum.Mage] = new List<SkillData>()
            {
                new SkillData("에너지 볼트", 2.5f, 13),
                new SkillData("매직 애로우", 3.1f, 28),
                new SkillData("메테오", 4.0f, 50),
            };

            dictSkillDatas[JobTypeEnum.Archer] = new List<SkillData>()
            {
                new SkillData("트윈 애로우", 2.3f, 11),
                new SkillData("애로우 레인", 2.7f, 24),
                new SkillData("드래곤 펄스", 3.6f, 46),
            };
        }

        // 직업에 맞는 스킬 데이터 리스트를 반환
        public static List<SkillData> GetSkills(JobTypeEnum jobType)
        {
            if (dictSkillDatas.ContainsKey(jobType))
            {
                return dictSkillDatas[jobType];
            }

            return null;
        }
    }
}

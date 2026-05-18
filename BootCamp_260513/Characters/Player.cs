using RPGGame.Datas;
using RPGGame.Enums;
using RPGGame.Items;
using RPGGame.Managers;
using RPGGame.Utility;
using static RPGGame.Texts.GameText;

namespace RPGGame.Characters
{
    internal class Player : Character
    {
        public Inventory Inven { get; private set; }                        // 인벤토리
        public BonusStat ItemStats { get; private set; } = new BonusStat(); // 아이템 스탯
        public List<SkillData> skills { get; private set; }                 // 각 직업별 스킬을 가져올 리스트
        public string[] SkillMenuTexts { get; private set; }                // 각 직업별 스킬을 메뉴로 보여주기 위한 문자열 배열
        public JobTypeEnum JobType { get; private set; }                    // 직업 타입 (Warrior, Mage, Archer)

        public int TotalAttack  // 총 플레이어의 공격력
        {
            get 
            { 
                return Attack + ItemStats.Atk; 
            }
        }
        public int TotalDefense // 총 플레이어의 방어력
        {
            get
            {
                return Defense + ItemStats.Def;
            }
        }

        public int Level { get; private set; }
        public int Gold { get; private set; }
        private int curExp = 0;
        private int maxExp = 0;
        private int maxHp = 0;
        private int maxMp = 0;

        // 플레이어 생성시 인벤토리도 같이 생성
        public Player()
        {
            Inven = new Inventory();
        }

        // 레벨업 체크
        private void CheckLevelUp()
        {
            int levelUpCount = 0;

            // 경험치가 최대치보다 높으면 레벨업 진행 후
            // 레벨업 한 개수를 셈
            while (curExp >= maxExp)
            {
                curExp -= maxExp;
                LevelUp();
                levelUpCount++;
            }

            // 레벨업 안했으면 리턴
            if (levelUpCount == 0)
            {
                return;
            }

            // 했을 경우 스탯을 올릴 수 있는 횟수를 레벨업 횟수만큼 줌 (스탯을 다 찍으면 for문과 while문 끝)
            for (int i = 1; i <= levelUpCount; i++)
            {
                int statSelected = 0;
                bool isSelected = false;

                // 최대 체력, 최대 마나, 공격력, 방어력 중 선택
                while (!isSelected)
                {
                    ConsoleUI.RenderGameFrame();
                    ConsoleUI.DrawCharacter(JobDataBase.JobDatas[(int)JobType].ImagePath, 9, 7);

                    // 포인트가 몇개 남았는지 표시
                    PrintCenteredText($" LEVEL UP! ({Level}Lv) ", 0.35f);
                    PrintCenteredText($"보너스 스탯을 선택하세요! (남은 포인트: {levelUpCount - i + 1})", 0.40f);

                    // 스탯 메뉴 출력
                    PrintMenu(PlayerLevelUp.statMenus, statSelected);

                    ConsoleKey key = Console.ReadKey(true).Key;

                    // 선택지 이동
                    statSelected = ConsoleUtil.InputKey(key, statSelected, PlayerLevelUp.statMenus.Length);

                    if (key == ConsoleKey.Spacebar)
                    {
                        // 선택한 스탯 적용
                        switch ((StatSelectEnum)statSelected)
                        {
                            case StatSelectEnum.Hp:
                                maxHp += 10;
                                Hp = maxHp;
                                break;
                            case StatSelectEnum.Mp:
                                maxMp += 5;
                                Mp = maxMp;
                                break;
                            case StatSelectEnum.Attack:
                                Attack += 2;
                                break;
                            case StatSelectEnum.Defense:
                                Defense += 1;
                                break;
                        }
                        isSelected = true; // 현재 포인트 소모 완료, 다음 루프로
                    }
                }
            }
        }

        // 레벨 올리고 경험치 최대치 증가
        private void LevelUp()
        {
            Level++;

            maxExp = ExpTable.RequiredExp[Level];
        }

        // 플레이어만 갖고 있는 변수들 오버라이딩해서 초기화
        protected override void Init()
        {
            base.Init();

            Gold = 1000;
            Level = 9;
            curExp = 0;
            maxExp = ExpTable.RequiredExp[Level];
            maxHp = CharacterData.Hp;
            maxMp = CharacterData.Mp;
        }

        // 플레이어 정보 출력
        public override void ShowInfo(int x, int y)
        { 
            // Total 스탯을 활용해 합산 결과와 보너스를 같이 보여줌
            string atkBonus = ItemStats.Atk > 0 ? $"(+{ItemStats.Atk})" : "";
            string defBonus = ItemStats.Def > 0 ? $"(+{ItemStats.Def})" : "";

            // 간격 맞추기 용도
            int rightX = x + 18;

            Console.SetCursorPosition(x, y);
            Console.Write($"이름 : {CharacterData.Name}");
            Console.SetCursorPosition(rightX, y);
            Console.Write($"골드 : {Gold}");

            Console.SetCursorPosition(x, y + 1);
            Console.Write($"레벨 : {Level}");
            Console.SetCursorPosition(rightX, y + 1);
            Console.Write($"경험치 : ({curExp} / {maxExp})");

            Console.SetCursorPosition(x, y + 2);
            Console.Write($"체력 : {Hp} / {maxHp}");
            Console.SetCursorPosition(rightX, y + 2);
            Console.Write($"공격력 : {TotalAttack} {atkBonus}");

            Console.SetCursorPosition(x, y + 3);
            Console.Write($"마나 : {Mp} / {maxMp}");
            Console.SetCursorPosition(rightX, y + 3);
            Console.Write($"방어력 : {TotalDefense} {defBonus}");
        }

        // 데미지 넣기
        public override int TakeDamage(int damage)
        {
            int totalDamage = damage - TotalDefense;

            if (totalDamage < 0)
            {
                totalDamage = 0;
            }

            Hp -= totalDamage;

            if (Hp < 0)
            {
                Hp = 0;
            }

            return totalDamage;
        }

        // 플레이어 객체 생성 후 직업 설정하는 메서드
        public void SetPlayerJob(JobTypeEnum jobType)
        {
            // 직업 타입 저장
            JobType = jobType;
            // 직업 별 스킬 리스트로 저장
            skills = SkillDataBase.GetSkills(jobType);
            // 캐릭터 공통 데이터 저장
            CharacterData = JobDataBase.JobDatas[(int)jobType];
            
            if (skills != null)
            {
                // 스킬 3개 + "뒤로 가기" 문자열 배열 가져오기 총 길이 4
                SkillMenuTexts = PlayerSkill.PlayerSkills(skills);
            }

            Init();
        }

        // 몬스터를 잡았을 경우 보상을 주는 메서드
        public void GetReward(int exp, int gold)
        {
            curExp += exp;
            Gold += gold;

            PrintCenteredText($"경험치 +{exp} 획득했습니다!", 0.46f);
            PrintCenteredText($"골드 +{gold} 획득했습니다!", 0.52f);

            Thread.Sleep(2000);

            CheckLevelUp();
        }

        // 아이템 장착하면 플레이어 스탯 증가 시켜주는 메서드
        // 해제하면 다시 스탯 제거 해줌
        public void UpdateItemStats(int atk = 0, int def = 0, bool isEquip = true)
        {
            // 장착
            if (isEquip)
            {
                ItemStats.Add(atk, def);
            }
            else // 해제
            { 
                ItemStats.Add(-atk, -def); 
            } 
        }

        // 상점에서 물건 구입 시도 메서드
        public bool TryBuy(Item item)
        {
            if (Gold < item.Price)
            {
                PrintCenteredText("골드가 부족합니다.", 0.5f);
                return false;
            }

            if (Inven.Add(item))
            {
                Gold -= item.Price;
                return true;
            }

            return false;
        }

        // 체력 포션 먹었을 때 회복시켜주는 메서드
        public void HealHp(int amount)
        {
            Hp += amount;

            // 회복 후 체력이 최대 체력을 넘지 않도록 방지
            if (Hp > maxHp)
            {
                Hp = maxHp;
            }

            PrintCenteredText($"체력을 {amount}만큼 회복했습니다! (현재 HP: {Hp}/{maxHp})", 0.46f);
        }

        // 마나 포션 먹었을 때 회복시켜주는 메서드
        public void HealMp(int amount)
        {
            Mp += amount;

            if (Mp > maxMp)
            {
                Mp = maxMp;
            }

            PrintCenteredText($"마나를 {amount}만큼 회복했습니다! (현재 MP: {Mp}/{maxMp})", 0.46f);
        }

        // 스킬 사용시 현재 마나량 체크 후 true, false 반환
        public bool UseMp(int cost)
        {
            if (Mp < cost)
            {
                return false;
            }

            Mp -= cost;
            return true;
        }
    }
}
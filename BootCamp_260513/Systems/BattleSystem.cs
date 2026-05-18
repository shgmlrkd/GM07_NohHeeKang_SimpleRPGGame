using RPGGame.Characters;
using RPGGame.Datas;
using RPGGame.Enums;
using RPGGame.Texts;
using RPGGame.Utility;
using static RPGGame.Texts.GameText;

namespace RPGGame.Systems
{
    internal class BattleSystem
    {
        public Player player { get; private set; }
        public Monster monster { get; private set; }
        
        private string[] battleMenus = Battle.Menus;
        private string battlePromptText = Battle.Prompt;

        private int battleSelected = 0;

        public BattleSystem(Player player, Monster monster)
        {
            this.player = player;
            this.monster = monster;
        }

        public GameStateEnum OpenBattleMenu()
        {
            // 플레이어 턴에서 기본 공격, 스킬 공격, 뒤로 가기 메뉴가 있는데
            // 공격을 했다면 true, 안했다면 false 반환
            // 즉 턴을 소모 했는가를 반환
            bool turnConsumed = PlayerTurn();

            // 턴을 소모하지 않았다면 게임 상태는 Hunting 상태로 반환
            if(!turnConsumed)
            {
                return GameStateEnum.Hunting;
            }

            // 몬스터가 죽었는지 확인 죽었다면 Reward 획득 후
            // 게임 상태는 플레이 메뉴로 반환
            if (monster.IsDead())
            {
                ConsoleUI.RenderGameFrame();
                ConsoleUI.DrawCharacter(JobDataBase.JobDatas[(int)player.JobType].ImagePath, 9, 7);

                player.GetReward(monster.MData.RewardExp, monster.MData.RewardGold);

                return GameStateEnum.PlayMenu;
            }

            // 몬스터 차례
            MonsterTurn();

            // 플레이어가 죽었다면 게임 오버
            // 게임 상태는 게임 종료를 반환
            if (player.IsDead())
            {
                ConsoleUI.RenderGameFrame();

                PrintCenteredText($"{player.CharacterData.Name}이(가) 죽었습니다. . .", 0.46f);
                PrintCenteredText("게임 오버", 0.52f);
                Thread.Sleep(1500);
                return GameStateEnum.Exit;
            }

            return GameStateEnum.Hunting;
        }

        // 싸움 중 도망치기
        public GameStateEnum TryRun()
        {
            // 40% 확률로 도망 가능
            bool success = RandomHelper.Chance(100);

            ConsoleUI.RenderGameFrame();

            if (success)
            {
                PrintCenteredText("간신히 도망치기에 성공했다...", 0.4f);

                Thread.Sleep(1000);

                return GameStateEnum.PlayMenu;
            }

            PrintCenteredText("도망치기에 실패했다!", 0.4f);

            Thread.Sleep(1000);

            // 도망에 실패하면 몬스터한테 맞음
            MonsterTurn();

            // 플레이어 죽으면 게임 오버
            if (player.IsDead())
            {
                PrintCenteredText($"{player.JobType.ToString()}이(가) 죽었습니다. . .", 0.46f);
                PrintCenteredText("게임 오버", 0.52f);
                Thread.Sleep(1500);
                return GameStateEnum.Exit;
            }

            return GameStateEnum.Hunting;
        }

        private bool PlayerTurn()
        {
            // 플레이어는 기본 공격 또는 스킬 공격을 하면 몬스터 턴으로 넘어가고
            // 뒤로 가기를 선택하면 여전히 플레이어 턴임
            bool isOpen = true;

            while (isOpen)
            {
                ConsoleUI.RenderGameFrame();

                ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));
                ConsoleUI.PrintCharacter(monster, (0.65f, 0.25f), 68);

                PrintCenteredText(battlePromptText, 0.3f);
                PrintMenu(battleMenus, battleSelected);

                ConsoleKey key = Console.ReadKey(true).Key;

                battleSelected = ConsoleUtil.InputKey(key, battleSelected, battleMenus.Length);

                if (key == ConsoleKey.Spacebar)
                {
                    switch ((AttackTypeEnum)battleSelected)
                    {
                        case AttackTypeEnum.BaseAttack:
                            int damage = monster.TakeDamage(player.TotalAttack);

                            Battle.AttackText(player, monster, damage);

                            Thread.Sleep(1000);

                            isOpen = false;

                            battleSelected = 0;
                            return true;

                        case AttackTypeEnum.SkillAttack:
                            battleSelected = 0;
                            if (OpenSkillMenu())
                            {
                                isOpen = false;
                                return true;
                            }
                            break;
                        case AttackTypeEnum.Back:
                            battleSelected = 0;
                            return false;
                    }
                }
            }

            return false;
        }

        // 스킬 공격을 누르면 어떤 스킬을 사용할 지 스킬 메뉴가 열림
        private bool OpenSkillMenu()
        {
            bool isSkillOpen = true;
            int skillSelected = 0;

            string[] skillMenus = player.SkillMenuTexts;

            while (isSkillOpen)
            {
                ConsoleUI.RenderGameFrame();

                ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));

                PrintCenteredText("시전할 스킬을 선택하세요!", 0.3f);
                PrintMenu(skillMenus, skillSelected);

                ConsoleKey key = Console.ReadKey(true).Key;
                skillSelected = ConsoleUtil.InputKey(key, skillSelected, skillMenus.Length);

                if (key == ConsoleKey.Spacebar)
                {
                    SkillMenuEnum currentChoice = (SkillMenuEnum)skillSelected;

                    // 뒤로가기
                    if (currentChoice == SkillMenuEnum.Back)
                    {
                        skillSelected = 0;
                        return false;
                    }

                    // 선택한 스킬 데이터 가져오기
                    SkillData selectedSkill = player.skills[(int)currentChoice];

                    // 마나 부족 체크
                    if (player.Mp < selectedSkill.Cost)
                    {
                        ConsoleUI.RenderGameFrame();
                        ConsoleUI.DrawCharacter(player.CharacterData.ImagePath, 9, 7);

                        PrintCenteredText("마나가 부족합니다!", 0.46f);
                        PrintCenteredText($"현재 마나: {player.Mp} / 소모 마나: {selectedSkill.Cost}", 0.52f);
                        Thread.Sleep(1500);
                        skillSelected = 0;
                        continue;
                    }

                    // 마나 소비 및 데미지 계산
                    player.UseMp(selectedSkill.Cost);

                    int finalDamage = (int)(player.TotalAttack * selectedSkill.Multiplier);

                    int damage = monster.TakeDamage(finalDamage);

                    // 연출 출력
                    ConsoleUI.RenderGameFrame();
                    ConsoleUI.DrawCharacter(player.CharacterData.ImagePath, 9, 7);
                    PrintCenteredText($"[{selectedSkill.Name}] 시전!!", 0.4f);
                    Battle.AttackText(player, monster, damage);
                    Thread.Sleep(1500);
                    skillSelected = 0;
                    return true;
                }
            }

            return false;
        }

        // 몬스터 차례엔 그냥 공격력만큼 데미지를 줌
        private void MonsterTurn()
        {
            int damage = player.TakeDamage(monster.Attack);

            Battle.AttackText(monster, player, damage);

            Thread.Sleep(1000);
        }
    }
}
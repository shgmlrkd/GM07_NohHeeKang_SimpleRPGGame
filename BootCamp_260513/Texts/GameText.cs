using RPGGame.Characters;
using RPGGame.Datas;
using RPGGame.Utility;

namespace RPGGame.Texts
{
    // 필요한 텍스트들을 모아둠
    internal class GameText
    {
        public static string Exit = "게임을 종료합니다.";

        public static class MainMenu
        {
            public const string Title = "콘솔 퀘스트";
            public static readonly string[] Menus =
            {
                "게임 시작",
                "게임 종료"
            };
        }

        public static class PlayMenu
        {
            public const string Prompt = "무엇을 하시겠습니까?";
            public static readonly string[] Menus =
            {
                "사냥 하기",
                "상점 가기",
                "게임 종료"
            };
        }

        public static class PlayerLevelUp
        {
            public static readonly string[] statMenus = 
                { "체력 (+10)", "마나 (+5)", "공격력 (+2)", "방어력 (+1)" };
        }

        public static class PlayerSkill
        {
            public static string[] PlayerSkills(List<SkillData> skillDatas)
            {
                string[] skills =
                {
                    $"{skillDatas[0].Name} (MP - {skillDatas[0].Cost})",
                    $"{skillDatas[1].Name} (MP - {skillDatas[1].Cost})",
                    $"{skillDatas[2].Name} (MP - {skillDatas[2].Cost})",
                    $"{skillDatas[3].Name} (MP - {skillDatas[3].Cost})",
                    "뒤로 가기"
                };

                return skills;
            }
        }

        public static class ShopMenu
        {
            public const string Prompt = "상점에 오신 것을 환영합니다.";
            public static readonly string[] Menus =
            {
                "무기",
                "방어구",
                "포션",
                "뒤로 가기"
            };
        }

        public static class JobSelect
        {
            public const string Prompt = "어떤 직업으로 시작하시겠습니까?";
            public static readonly string[] Jobs =
            {
                "전사",
                "마법사",
                "아처"
            };

            public static string GetSelectedText(string jobName)
            {
                return $"{jobName}을(를) 선택하셨습니다.";
            }
        }

        public static class Hunting
        {
            public const string Title = "사냥터";
            public const string Prompt = "어떤 행동을 하시겠습니까?";
            public static readonly string[] Menus =
            {
                "공격 하기",
                "인벤토리 보기",
                "도망 가기"
            };

            public static string AttackText(Character attacker, Character target)
            {
                return $"{attacker.CharacterData.Name}이(가) {target.CharacterData.Name}을 공격!!!\n" +
                       $"{target.CharacterData.Name}이(가) {attacker.Attack}만큼 피해를 입었습니다.";
            }
        }

        public static class Battle
        {
            public const string Prompt = "어떤 공격을 하시겠습니까?";
            public static readonly string[] Menus =
            {
                "기본 공격",
                "스킬 공격",
                "뒤로 가기"
            };

            public static void AttackText(Character attacker, Character target, int damage)
            {
                ConsoleUI.RenderGameFrame();

                string text = $"{attacker.CharacterData.Name}이(가) {target.CharacterData.Name}을 공격했습니다.";

                PrintCenteredText(text, 0.4f);

                if (damage > 0)
                {
                    text = $"{target.CharacterData.Name}이(가) {damage} 피해를 입었습니다.";
                    PrintCenteredText(text, 0.45f);
                }
                else
                {
                    text = $"{target.CharacterData.Name}이(가) 공격을 방어했습니다.";
                    PrintCenteredText(text, 0.45f);
                }
            }
        }


        public static void PrintMenu(string[] menus, int selected)
        {
            // 메뉴 출력
            for (int i = 0; i < menus.Length; i++)
            {
                // 선택된 메뉴 표시한 문자열
                string menuText = ConsoleUI.GetTextLength(i, selected, menus[i]);

                int x = ConsoleUtil.GetCenteredXByString(Console.WindowWidth, menuText);
                int y = ConsoleUtil.GetSizeByRatio(Console.WindowHeight, 0.45f) + (i * 2);

                Console.SetCursorPosition(x, y);
                Console.Write(menuText);
            }
        }

        public static void PrintMenu(string[] menus, int selected, int ratioX)
        {
            // 메뉴 출력
            for (int i = 0; i < menus.Length; i++)
            {
                // 선택된 메뉴 표시한 문자열
                string menuText = ConsoleUI.GetTextLength(i, selected, menus[i]);

                int x = ConsoleUtil.GetSizeByRatio(Console.WindowWidth , ratioX);
                int y = ConsoleUtil.GetSizeByRatio(Console.WindowHeight, 0.45f) + (i * 2);

                Console.SetCursorPosition(x, y);
                Console.Write(menuText);
            }
        }

        public static void PrintCenteredText(string text, float heightRatio)
        {
            // 커서 세팅 후 문구 출력
            int x = ConsoleUtil.GetCenteredXByString(Console.WindowWidth, text);
            int y = ConsoleUtil.GetSizeByRatio(Console.WindowHeight, heightRatio);

            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }
    }
}

using RPGGame.Characters;
using RPGGame.Datas;
using RPGGame.Enums;
using RPGGame.Texts;
using RPGGame.Utility;
using static RPGGame.Texts.GameText;

namespace RPGGame.Systems
{
    internal class Game
    {
        private const int WIDTH = 116;
        private const int HEIGHT = 30;

        private Player player;
        private Monster monster;

        private BattleSystem battleSystem;
        private Shop shop;
        private Action[] playMenuActions;

        private GameStateEnum gameState = GameStateEnum.MainMenu;

        private (float ratioX, float ratioY) playerLocate = (0.08f, 0.25f);
        private (float ratioX, float ratioY) monsterLocate = (0.65f, 0.25f);

        private string[] mainMenus = MainMenu.Menus;
        private string[] playMenus = PlayMenu.Menus;
        private string[] huntingMenus = Hunting.Menus;

        private string mainTitleText = MainMenu.Title;
        private string jobSelectPromptText = JobSelect.Prompt;
        private string playPromptText = PlayMenu.Prompt;
        private string huntingTitleText = Hunting.Title;
        private string huntingPromptText = Hunting.Prompt;

        private int selected = 0;
        private bool isCharacterCreated = false;

        public Game()
        {
            // 이벤트 함수 배열 -> 게임 플레이 메뉴에서 gameState 상태를 바꾸기 위한 배열
            playMenuActions = new Action[]
            {
                GoHunting,
                OpenShop,
                ExitGame
            };

            // 커서 없애기
            Console.CursorVisible = false;

            // 콘솔창 크기 WIDTH, HEIGHT로 맞추기
            Console.SetWindowSize(WIDTH, HEIGHT);
            Console.SetBufferSize(WIDTH, HEIGHT);
        }

        public void Run()
        {
            while (true)
            {
                switch (gameState)
                {
                    case GameStateEnum.MainMenu:            // 메인 메뉴
                        UpdateMainMenu();
                        break;

                    case GameStateEnum.CreateCharacter:     // 캐릭터 생성
                        CreateCharacter();
                        break;

                    case GameStateEnum.PlayMenu:            // 행동 선택 메뉴
                        UpdatePlayMenu();
                        break;

                    case GameStateEnum.Hunting:             // 사냥 하기
                        UpdateHunting();
                        break;

                    case GameStateEnum.Shop:                // 상점
                        UpdateShop();
                        break;

                    case GameStateEnum.Exit:                // 게임 종료
                        GameExit();
                        return;
                }
            }
        }

        // 메인 화면 출력
        private void UpdateMainMenu()
        {
            // 게임 화면 초기화 및 외곽선 출력
            ConsoleUI.RenderGameFrame();

            // 콘솔 퀘스트 출력
            PrintCenteredText(mainTitleText, 0.3f);

            // 메뉴 출력
            PrintMenu(mainMenus, selected);

            // Up / Down 방향키 입력 받기
            ConsoleKey key = Console.ReadKey(true).Key;
            selected = ConsoleUtil.InputKey(key, selected, mainMenus.Length);

            // 스페이스 바를 누르면 게임 상태 전환
            if (key == ConsoleKey.Spacebar)
            {
                if (selected == (int)MainMenuEnum.Start)
                {
                    gameState = GameStateEnum.CreateCharacter;
                }
                else if (selected == (int)MainMenuEnum.Exit)
                {
                    gameState = GameStateEnum.Exit;
                }

                selected = 0;
            }
        }

        // 캐릭터 생성
        private void CreateCharacter()
        {
            // 게임 화면 초기화 및 외곽선 출력
            ConsoleUI.RenderGameFrame();

            // 어떤 직업으로 시작하시겠습니까? 문구 출력
            PrintCenteredText(jobSelectPromptText, 0.05f);

            int x = ConsoleUtil.GetSizeByRatio(WIDTH, 0.33f);
            int y = ConsoleUtil.GetSizeByRatio(HEIGHT, 0.15f);

            // 직업 개수만큼 캐릭터 그리고 직업 이름 출력하기
            for (int i = 0; i < JobDataBase.JobDatas.Length; i++)
            {
                int cursorX = x * i;
                int cursorY = y;

                // 선택된 메뉴 표시할 문자열
                string characterText = ConsoleUI.GetTextLength(i, selected, JobDataBase.JobDatas[i].Name);

                // 캐릭터 그리기
                ConsoleUI.DrawCharacter(JobDataBase.JobDatas[i].ImagePath, cursorX + 3, cursorY);

                // 캐릭터 이미지 사이즈 알아내고 커서 위치 조정
                (int sizeX, int sizeY) = ConsoleUI.GetImageHalfSize(JobDataBase.JobDatas[i].ImagePath);

                Console.SetCursorPosition(cursorX + sizeX, cursorY + sizeY);
                Console.Write(characterText);
            }

            ConsoleKey key = Console.ReadKey(true).Key;
            selected = ConsoleUtil.InputKey(key, selected, JobDataBase.JobDatas.Length);

            if (key == ConsoleKey.Spacebar)
            {
                x = ConsoleUtil.GetSizeByRatio(WIDTH, 0.33f);
                y = ConsoleUtil.GetSizeByRatio(HEIGHT, 0.15f);

                // 플레이어 딱 한번만 생성
                if (!isCharacterCreated)
                {
                    isCharacterCreated = true;

                    // 게임 외곽 그리고 선택한 캐릭터 그리기
                    ConsoleUI.RenderGameFrame();

                    ConsoleUI.DrawCharacter(JobDataBase.JobDatas[selected].ImagePath);

                    string playerJob = JobSelect.GetSelectedText(JobDataBase.JobDatas[selected].Name);

                    x = ConsoleUtil.GetCenteredXByString(WIDTH, playerJob);
                    y = ConsoleUtil.GetSizeByRatio(HEIGHT, 0.75f);

                    // 플레이어 객체 생성 후 직업에 따른 스탯 설정
                    player = new Player();
                    shop = new Shop(player);

                    player.SetPlayerJob((JobTypeEnum)selected);

                    Console.SetCursorPosition(x, y);
                    Console.Write(playerJob);

                    // 1.5초 멈춤
                    Thread.Sleep(1500);

                    gameState = GameStateEnum.PlayMenu;
                }

                selected = 0;
            }
        }

        private void UpdatePlayMenu()
        {
            // 게임 화면 초기화 및 외곽선 출력
            ConsoleUI.RenderGameFrame();

            // 무엇을 하시겠습니까? 문구 출력
            PrintCenteredText(playPromptText, 0.3f);

            // 플레이어 캐릭터 그리기
            ConsoleUI.PrintCharacter(player, playerLocate);

            // 플레이 메뉴 출력
            PrintMenu(playMenus, selected);

            ConsoleKey key = Console.ReadKey(true).Key;
            selected = ConsoleUtil.InputKey(key, selected, playMenus.Length);

            // gameState를 선택한 메뉴에 해당하는 상태로 바꾸기
            if (key == ConsoleKey.Spacebar)
            {
                playMenuActions[selected]();

                selected = 0;
            }
        }

        // 사냥 하기
        private void UpdateHunting()
        {
            // 게임 외곽 그리기
            ConsoleUI.RenderGameFrame();

            // 사냥터 타이틀 출력
            PrintCenteredText(huntingTitleText, 0.1f);

            // 어떤 행동을 하시겠습니까? 문구 출력
            PrintCenteredText(huntingPromptText, 0.3f);

            // 사냥터 메뉴 출력
            PrintMenu(huntingMenus, selected);
            
            // 플레이어, 몬스터 그리기
            ConsoleUI.PrintCharacter(player, playerLocate);
            ConsoleUI.PrintCharacter(monster, monsterLocate, 68);

            ConsoleKey key = Console.ReadKey(true).Key;
            selected = ConsoleUtil.InputKey(key, selected, huntingMenus.Length);

            if (key == ConsoleKey.Spacebar)
            {
                switch ((HuntingActionEnum)selected)
                {
                    case HuntingActionEnum.Attack:
                        gameState = battleSystem.OpenBattleMenu();
                        break;

                    case HuntingActionEnum.Inventory:
                        player.Inven.ShowInventory(player);
                        break;

                    case HuntingActionEnum.Run:
                        gameState = battleSystem.TryRun();
                        break;
                }

                selected = 0;
            }
        }

        // 상점 가기
        private void UpdateShop()
        {
            ConsoleUI.RenderGameFrame();

            shop.Open();

            gameState = GameStateEnum.PlayMenu;
        }

        // 게임 종료 문구 출력 후 종료
        private void GameExit()
        {
            ConsoleUI.RenderGameFrame();

            PrintCenteredText(Exit, 0.5f);

            Thread.Sleep(1500);
        }

        /*////////////////////////////////////////////////
        
        게임 플레이 메뉴에서 gameState 상태 변환 하기

        ////////////////////////////////////////////////*/

        // 사냥하기 들어가면 몬스터 객체 생성
        // 배틀 시스템 객체 생성
        private void GoHunting()
        {
            monster = new Monster(player.Level);
            battleSystem = new BattleSystem(player, monster);

            gameState = GameStateEnum.Hunting;
        }

        private void OpenShop()
        {
            gameState = GameStateEnum.Shop;
        }

        private void ExitGame()
        {
            gameState = GameStateEnum.Exit;
        }
    }
}
using RPGGame.Characters;
using RPGGame.Enums;
using RPGGame.Items;
using RPGGame.Utility;
using System;
using static RPGGame.Texts.GameText;

namespace RPGGame.Systems
{
    internal class Shop
    {
        private Player player;

        private List<Item> commonItems;
        private List<Item> filteredItems = new List<Item>();

        private Dictionary<JobTypeEnum, List<Item>> jobItems;
        
        private string[] itemMenus;
        private string[] shopMenus = ShopMenu.Menus;
        private string shopPrompt = ShopMenu.Prompt;

        private int selected = 0;

        // 상점 생성자에서는 물건 객체들을 리스트와 딕셔너리로 만듦
        // 직업 별로 나눠놓은 아이템은 딕셔너리로 공통인건 리스트로 만듦
        public Shop(Player player)
        {
            this.player = player;

            commonItems = new List<Item>()
            {   // 공통 포션
                new Potion { Name = "체력 포션", Heal = 40, Price = 10, PotionType = PotionTypeEnum.Hp },
                new Potion { Name = "마나 포션", Heal = 30, Price = 7, PotionType = PotionTypeEnum.Mp }
            };

            jobItems = new Dictionary<JobTypeEnum, List<Item>>()
            {
                {   // 전사 전용 아이템
                    JobTypeEnum.Warrior,
                    new List<Item>()
                    {
                        new Weapon { Name = "철 검", Attack = 5, Price = 20 },
                        new Weapon { Name = "강철 검", Attack = 29, Price = 60 },
                        new Weapon { Name = "기사의 대검", Attack = 44, Price = 120 },

                        new Armor { Name = "가죽 갑옷", Defense = 3, Price = 15 },
                        new Armor { Name = "사슬 갑옷", Defense = 16, Price = 55 },
                        new Armor { Name = "기사 갑옷", Defense = 32, Price = 100 }
                    }
                },

                {   // 마법사 전용 아이템
                    JobTypeEnum.Mage,
                    new List<Item>()
                    {
                        new Weapon { Name = "나무 지팡이", Attack = 4, Price = 18 },
                        new Weapon { Name = "마력 지팡이", Attack = 28, Price = 55 },
                        new Weapon { Name = "대마도사의 지팡이", Attack = 33, Price = 110 },

                        new Armor { Name = "천 로브", Defense = 2, Price = 12 },
                        new Armor { Name = "마법 로브", Defense = 15, Price = 50 },
                        new Armor { Name = "대마법사 로브", Defense = 30, Price = 90 }
                    }
                },

                {   // 아처 전용 아이템
                    JobTypeEnum.Archer,
                    new List<Item>()
                    {
                        new Weapon { Name = "짧은 활", Attack = 5, Price = 19 },
                        new Weapon { Name = "강궁", Attack = 29, Price = 57 },
                        new Weapon { Name = "명사수의 활", Attack = 33, Price = 115 },

                        new Armor { Name = "가벼운 가죽갑옷", Defense = 4, Price = 16 },
                        new Armor { Name = "강화 가죽갑옷", Defense = 16, Price = 53 },
                        new Armor { Name = "사냥꾼의 갑옷", Defense = 31, Price = 95 }
                    }
                }
            };
        }

        // 직업별 아이템만 필터링해서 ("이름 (가격G)")으로 변환
        private void UpdateItemMenus(List<Item> jobItems, ItemEnum itemType)
        {
            int count = 0;
            foreach (Item item in jobItems)
            {
                if (item.ItemType == itemType)
                { 
                    count++;
                }
            }

            itemMenus = new string[count + 1];

            int index = 0;
            foreach (Item item in jobItems)
            {
                if (item.ItemType == itemType)
                {
                    itemMenus[index] = $"{item.Name} ({item.Price}G)";
                    index++;
                }
            }
            itemMenus[itemMenus.Length - 1] = "뒤로 가기";
        }

        // 직업별 아이템과 공통 아이템을 한 리스트로 합지는 메서드
        public List<Item> GetItems(JobTypeEnum job)
        {
            List<Item> result = new List<Item>();

            result.AddRange(commonItems);

            if (jobItems.TryGetValue(job, out List<Item> items))
            {
                result.AddRange(items);
            }

            return result;
        }

        // 상점 열기
        public void Open()
        {
            List<Item> currentItems = GetItems(player.JobType);

            bool isRunning = true;
            bool inCategory = true;

            while (isRunning)
            {
                ConsoleUI.RenderGameFrame();

                ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));
                
                // (88, 9)좌표 기준으로 나의 인벤토리를 보여줌
                player.Inven.ShowInventorySummary(88, 9);

                // 상점 텍스트 출력
                PrintCenteredText(ShopMenu.Prompt, 0.3f);
                
                // 현재 위치가 장비, 방어구, 포션과 같은 카테고리라면
                if (inCategory)
                {
                    // 상점 카테고리 메뉴를 보여줌
                    PrintMenu(shopMenus, selected);
                }
                else // 아니라면
                {
                    // 실제 아이템 메뉴를 보여줌
                    PrintMenu(itemMenus, selected);
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                if (inCategory)
                {
                    selected = ConsoleUtil.InputKey(key, selected, shopMenus.Length);

                    if (key == ConsoleKey.Spacebar)
                    {
                        ItemEnum selectedCategory = (ItemEnum)selected;

                        if (selectedCategory == ItemEnum.Back) // 상점 나가기
                        {
                            isRunning = false;
                        }
                        else // 무기, 방어구, 포션 카테고리에서 선택 시
                        {
                            // 들어간 카테고리에 맞는 실제 직업별 아이템 메뉴를 보여줌
                            UpdateItemMenus(currentItems, selectedCategory);

                            filteredItems.Clear();

                            foreach (Item item in currentItems)
                            {
                                if (item.ItemType == selectedCategory)
                                {
                                    filteredItems.Add(item);
                                }
                            }

                            inCategory = false;
                            selected = 0;
                        }
                    }
                }
                else
                {
                    selected = ConsoleUtil.InputKey(key, selected, itemMenus.Length);

                    if (key == ConsoleKey.Spacebar)
                    {
                        // 선택한 인덱스가 "뒤로 가기"인 경우
                        if (selected == itemMenus.Length - 1)
                        {
                            inCategory = true;
                            selected = 0;
                        }
                        // 실제 아이템을 선택한 경우 (구매)
                        else
                        {
                            ConsoleUI.RenderGameFrame();

                            ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));

                            player.Inven.ShowInventorySummary(88, 9);

                            if (CanBuy(filteredItems[selected]))
                            {
                                inCategory = true;  //  구매 후 카테고리 화면으로 튕겨나가게함
                                selected = 0;
                            }
                        }
                    }
                }
            }
        }

        // 아이템 구매 가능 여부를 확인하고
        // 구매가 가능하다면 실제 아이템을 생성하여 지급
        private bool CanBuy(Item item)
        {
            // 상점에 등록된 원본 아이템의 정보를 기반으로
            // 새 아이템 객체를 생성하여 플레이어에게 지급을 시도
            player.TryBuy(item.CreateItem());

            Thread.Sleep(1500);

            return true;
        }
    }
}
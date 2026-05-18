using RPGGame.Characters;
using RPGGame.Enums;
using RPGGame.Interface;
using RPGGame.Items;
using RPGGame.Utility;
using static RPGGame.Texts.GameText;

namespace RPGGame.Managers
{
    internal class Inventory
    {
        private List<Item> items = new List<Item>();

        private string[] itemMenus = Array.Empty<string>();

        private readonly string[] actionMenus = { "사용/장착/해제", "버리기", "돌아가기" };

        private int selected = 0;

        private void UpdateMenuArray()
        {
            // 인벤토리 아이템 개수에 맞게 배열 크기 재할당
            itemMenus = new string[items.Count + 1];

            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                // 장착 가능한 아이템이면 장착되있으면 [E] 아니면 빈칸 처리
                string equipStatus = (item is IEquipable equipable && equipable.IsEquipped) ? "[E] " : "";
                // 포션이면 개수 표시
                string countInfo = item.ItemType == ItemEnum.Potion ? $" - {item.Count}개" : "";

                // ex) [E] 철 검 or 체력 표션 - 2개
                itemMenus[i] = $"{equipStatus}{item.Name}{countInfo}";
            }

            itemMenus[itemMenus.Length - 1] = "돌아가기";
        }

        // 아이템 추가 (소비품은 중첩, 장비는 개별)
        public bool Add(Item newItem)
        {
            // 포션 중첩
            if (newItem.ItemType == ItemEnum.Potion)
            {
                foreach (Item item in items)
                {
                    if (item.Name == newItem.Name && item.ItemType == ItemEnum.Potion)
                    {
                        item.AddCount(newItem.Count);
                        UpdateMenuArray();

                        PrintCenteredText($"{item.Name}의 개수가 {newItem.Count}개 증가했습니다.", 0.46f);
                        PrintCenteredText($"(현재: {item.Count}개)", 0.52f);
                        return true;
                    }
                }
            }
            // 장비 중복 방지
            else if (newItem.ItemType == ItemEnum.Weapon || newItem.ItemType == ItemEnum.Armor)
            {
                foreach (Item item in items)
                {
                    if (item.Name == newItem.Name)
                    {
                        // 실패 메시지 출력
                        PrintCenteredText($"[실패]", 0.46f);
                        PrintCenteredText($"'{newItem.Name}'을(를) 보유하고 있습니다.", 0.52f);
                        return false;
                    }
                }
            }

            // 새로운 아이템 추가
            items.Add(newItem);
            UpdateMenuArray();

            PrintCenteredText($"{newItem.Name}을(를) 획득했습니다!", 0.5f);
            return true;
        }

        public bool Remove(int index)
        {
            Item target = items[index];

            // 장착 중인 아이템은 버릴 수 없게 막음
            if (target is IEquipable equipable && equipable.IsEquipped)
            {
                PrintCenteredText("장착 중인 아이템은 버릴 수 없습니다!", 0.5f);
                return false;
            }

            PrintCenteredText($"{target.Name}을(를) 버렸습니다.", 0.5f);
            items.RemoveAt(index); 
            Thread.Sleep(1500);
            UpdateMenuArray();
            return true;
        }

        public void UseOrEquipItem(Player player, Item targetItem)
        {
            ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));

            // 장착 가능한 아이템인가 확인
            if (!targetItem.CanEquip)
            {
                // 포션이면 사용
                if (targetItem.ItemType == ItemEnum.Potion)
                {
                    Consume(targetItem, player);
                }
                return;
            }

            // 이 아이템이 장착할 수 있는 아이템이라면
            if (targetItem is IEquipable equipable)
            {
                // 이미 장착된 상태라면 해제
                if (equipable.IsEquipped)
                {
                    equipable.UnEquip(player);
                    PrintCenteredText($"{targetItem.Name} 장착을 해제했습니다.", 0.5f);
                    Thread.Sleep(1500);
                    return;
                }

                // 장착하려는 아이템의 타입과 같은 아이템을 장착하려고 할 때 막아주기
                bool isSlotOccupied = false;

                foreach (Item item in items)
                {
                    if (item is IEquipable otherEquipable)
                    {
                        if (item.ItemType == targetItem.ItemType && otherEquipable.IsEquipped)
                        {
                            isSlotOccupied = true;
                            break;
                        }
                    }
                }

                if (isSlotOccupied)
                {
                    string categoryName = targetItem.ItemType == ItemEnum.Weapon ? "무기" : "방어구";
                    PrintCenteredText($"이미 {categoryName}를 장착 중입니다!", 0.46f);
                    PrintCenteredText("먼저 해제해주세요.", 0.52f);
                    Thread.Sleep(1500);
                    return;
                }

                // 모든 검사를 통과하면 장착
                equipable.Equip(player);
                PrintCenteredText($"{targetItem.Name}을(를) 장착했습니다.", 0.5f);
                Thread.Sleep(1500);
            }
        }

        private void Consume(Item item, Player player)
        {
            // 포션 사용
            if (item is Potion potion)
            {
                // 포션 1개 사용
                potion.RemoveCount(1);
                
                // 포션에 적힌 Heal 수치만큼 회복
                switch (potion.PotionType)
                {
                    case PotionTypeEnum.Hp:
                        player.HealHp(potion.Heal);
                        break;

                    case PotionTypeEnum.Mp:
                        player.HealMp(potion.Heal);
                        break;
                }

                PrintCenteredText($"{potion.Name}을(를) 사용했습니다.", 0.5f);
                Thread.Sleep(1500);

                // 다 썼으면 인벤토리에서 삭제 후 목록 갱신
                if (potion.Count <= 0)
                {
                    items.Remove(potion);
                    UpdateMenuArray();
                }
            }
        }

        public void ShowInventory(Player player)
        {
            UpdateMenuArray();

            while (true)
            {
                // 게임 화면 초기화, 외곽선 출력
                ConsoleUI.RenderGameFrame();

                ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));

                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintCenteredText("[인벤토리 목록]", 0.3f);
                Console.ResetColor();

                if (items.Count == 0)
                {
                    // "돌아가기" 메뉴만 들어있는 상태의 배열을 그대로 출력
                    PrintMenu(itemMenus, 0);

                    // 스페이스바 누르면 return 아니면 아래 코드 실행 X
                    ConsoleKey emptyKey = Console.ReadKey(true).Key;
                    if (emptyKey == ConsoleKey.Spacebar)
                    {
                        return;
                    }

                    continue;
                }

                // 인벤토리에 있는 아이템 메뉴 출력
                PrintMenu(itemMenus, selected);

                ConsoleKey key = Console.ReadKey(true).Key;
                selected = ConsoleUtil.InputKey(key, selected, itemMenus.Length);

                if (key == ConsoleKey.Spacebar)
                {
                    // "돌아가기" 버튼 선택
                    if (selected == itemMenus.Length - 1)
                    {
                        return; // 인벤토리 종료
                    }
                    else
                    {
                        // 장착/해제, 삭제하기, 뒤로가기 메뉴
                        ShowActionMenu(player, selected);

                        // 아이템이 삭제되어 인덱스가 전체 배열 크기를 벗어나는 것을 방지하는 예외 처리
                        if (selected >= itemMenus.Length)
                        {
                            selected = Math.Max(0, itemMenus.Length - 1);
                        }
                    }

                    selected = 0;
                }
            }
        }

        // 장작/해제/사용, 삭제, 뒤로 가기 중 선택하는 메뉴
        private void ShowActionMenu(Player player, int selected)
        {
            int actionSelected = 0; // 서브 메뉴용 선택 변수
            Item targetItem = items[selected];

            while (true)
            {
                ConsoleUI.RenderGameFrame();

                ConsoleUI.PrintCharacter(player, (0.08f, 0.25f));

                PrintCenteredText($"[{targetItem.Name}]에 어떤 행동을 하시겠습니까?", 0.3f);

                PrintMenu(actionMenus, actionSelected);

                ConsoleKey key = Console.ReadKey(true).Key;
                actionSelected = ConsoleUtil.InputKey(key, actionSelected, actionMenus.Length);

                if (key == ConsoleKey.Spacebar)
                {
                    switch ((InventoryActionEnum)actionSelected)
                    {
                        case InventoryActionEnum.UseOrEquipUnEquip:
                            ConsoleUI.RenderGameFrame();
                            Console.SetCursorPosition(0, Console.WindowHeight / 2);

                            UseOrEquipItem(player, targetItem);
                            UpdateMenuArray(); // 갱신

                            return; // 아이템 목록으로 복귀

                        case InventoryActionEnum.Drop:
                            ConsoleUI.RenderGameFrame();
                            Console.SetCursorPosition(0, Console.WindowHeight / 2);

                            if (Remove(selected))
                            {
                                UpdateMenuArray(); // 갱신
                            }

                            return; // 아이템 목록으로 복귀

                        case InventoryActionEnum.Back:
                            return; // 서브 메뉴를 그냥 끄고 아이템 목록으로 복귀
                    }

                    actionSelected = 0;
                }
            }
        }

        // 상점에서 인벤토리 보여주기
        public void ShowInventorySummary(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[ 내 인벤토리 ]");
            Console.ResetColor();

            for (int i = 0; i < items.Count; i++)
            {
                Console.SetCursorPosition(x, y + 2 + i);

                string displayInfo = $"{i + 1}.";

                // 장착 아이템인 경우 장착 여부 표시
                if (items[i] is EquipableItem eItem)
                {
                    string equipMark = eItem.IsEquipped ? "[E] " : "  ";
                    displayInfo += $"{equipMark}{items[i].Name}";
                }
                // 포션인 경우 개수 표시
                else if (items[i] is Potion potion)
                {
                    displayInfo += $"  {items[i].Name} x{potion.Count}";
                }

                Console.Write(displayInfo);
            }
        }
    }
}
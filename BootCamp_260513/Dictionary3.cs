namespace BootCamp_260513
{
    class Item
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Item(string name, int price) 
        {
            Name = name;
            Price = price;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"아이템 : {Name} / 가격 : {Price} 골드");
        }
    }

    class Inventory
    {
        private List<Item> items = new List<Item>();

        public int Count 
        { 
            get 
            { 
                return items.Count;
            }
        }

        public void AddItem(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("없음");
                return;
            }

            items.Add(item);
            Console.WriteLine($"{item.Name}을(를) 추가했다.");
        }

        public void RemoveItem(string itemName)
        {
            foreach (Item item in items)
            {
                if(item.Name == itemName)
                {
                    items.Remove(item);
                    Console.WriteLine($"{itemName}을 제거했다.");
                    return;
                }
            }

            Console.WriteLine($"{itemName}이 없다.");
        }

        public void ShowItems()
        {
            if(items.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있다.");
            }

            foreach(Item item in items)
            {
                item.PrintInfo();
            }
        }
    }

    class Player
    {
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public Inventory Inventory { get; private set; }

        public Player(int playerID,  string name)
        {
            PlayerID = playerID;
            Name = name;
            Inventory = new Inventory();
        }

        public void ShowPlayerInfo()
        {
            Console.WriteLine($"playerID : {PlayerID} / 이름 : {Name}");
        }
    }

    internal class Dictionary3
    {
        static void Main(string[] args)
        {
            Dictionary<int, Player> players = new Dictionary<int, Player>();

            Player player1 = new Player(1001, "전사");
            Player player2 = new Player(1002, "마법사");
            Player player3 = new Player(1003, "도적");

            players.Add(player1.PlayerID, player1);
            players.Add(player2.PlayerID, player2);
            players.Add(player3.PlayerID, player3);

            Item sword = new Item("검", 1000);
            Item shield = new Item("방패", 700);
            Item staff = new Item("지팡이", 1200);

            players[1001].Inventory.AddItem(sword);
            players[1001].Inventory.AddItem(shield);
            players[1002].Inventory.AddItem(staff);

            players[1001].ShowPlayerInfo();
            players[1002].ShowPlayerInfo();

            players[1001].Inventory.ShowItems();
            players[1002].Inventory.ShowItems();

            players[1001].Inventory.RemoveItem("검");

            players[1001].Inventory.ShowItems();
            players[1002].Inventory.ShowItems();
        }
    }
}

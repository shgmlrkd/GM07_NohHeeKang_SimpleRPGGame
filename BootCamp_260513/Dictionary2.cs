namespace BootCamp_260513
{
    internal class Dictionary2
    {
        static void Main(string[] args)
        {
            Dictionary<int, List<string>> inven = new Dictionary<int, List<string>>();

            inven.Add(1, new List<string>());

            inven[1].Add("검");
            inven[1].Add("포션");
            inven[1].Add("방패"); 
            
            inven.Add(2, new List<string>());

            inven[2].Add("활");
            inven[2].Add("화살");
            inven[2].Add("포션");

            foreach (KeyValuePair<int, List<string>> player in inven)
            {
                Console.WriteLine(player.Key);

                foreach (string item in player.Value)
                {
                    Console.WriteLine(item);
                }
            }

            inven[1].Add("전설의 검");

            
            foreach (string item in inven[1])
            {
                Console.WriteLine(item);
            }
            
        }
    }
}

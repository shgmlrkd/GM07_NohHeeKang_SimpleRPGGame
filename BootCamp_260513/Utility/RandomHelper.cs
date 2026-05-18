namespace RPGGame.Utility
{
    // 랜덤 관련 딱히 많이 쓰지는 않음
    internal static class RandomHelper
    {
        private static Random rand = new Random();

        public static int Next(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static bool Chance(int percent)
        {
            return rand.Next(100) < percent;
        }
    }
}

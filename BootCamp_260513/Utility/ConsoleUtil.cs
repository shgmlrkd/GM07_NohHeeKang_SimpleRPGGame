namespace RPGGame.Utility
{
    // 커서 위치, 텍스트 갯수 세기, 현재 선택할 메뉴의 위치 등을 모음
    internal class ConsoleUtil
    {
        // 전체 길이에서 중앙 X 좌표를 반환
        public static int GetCenteredXByString(int totalLength, string str)
        {
            int length = 0;

            foreach (char c in str)
            {
                if (c >= '가' && c <= '힣')
                {
                    length += 2;
                }
                else
                {
                    length += 1;
                }
            }

            return (totalLength - length) / 2;
        }

        // 화면 사이즈 비율을 기준으로 위치를 계산
        public static int GetSizeByRatio(int size, float ratio = 0.5f)
        {
            return (int)(size * ratio);
        }

        // Up/Down 키 입력에 따라 선택 인덱스를 갱신하고 범위를 순환 처리
        public static int InputKey(ConsoleKey key, int selected, in int count)
        {
            switch(key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.LeftArrow:
                    selected--;

                    if (selected < 0)
                    {
                        selected = count - 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    selected++;

                    if (selected >= count)
                    {
                        selected = 0;
                    }
                    break;
            }

            return selected;
        }
    }
}

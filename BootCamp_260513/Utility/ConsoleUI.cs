using RPGGame.Characters;
using System.Drawing;

namespace RPGGame.Utility
{
    // 그림을 출력하기 위한 것들을 모아둠
    internal class ConsoleUI
    {
        // 가로, 세로 크기를 조절해 박스 만들기
        private static void DrawBox()
        {
            int innerWidth = Console.WindowWidth - 2;

            // 윗줄
            Console.WriteLine("╔" + new string('═', innerWidth) + "╗");

            // 중간
            for (int i = 0; i < Console.WindowHeight - 2; i++)
            {
                Console.WriteLine("║" + new string(' ', innerWidth) + "║");
            }

            // 아랫줄
            Console.Write("╚" + new string('═', innerWidth) + "╝");
        }

        private static void DrawBox(int left, int top, int width, int height)
        {
            // 윗줄
            Console.SetCursorPosition(left, top);
            Console.Write("┌" + new string('─', width) + "┐");

            // 중간
            for (int y = 1; y < height - 1; y++)
            {
                Console.SetCursorPosition(left, top + y);
                Console.Write("│" + new string(' ', width) + "│");
            }

            // 아랫줄
            Console.SetCursorPosition(left, top + height - 1);
            Console.Write("└" + new string('─', width) + "┘");
        }

        private static ConsoleColor GetConsoleColor(Color c)
        {
            int color = 0;

            // RGB 비트
            if (c.R > 64) color |= 4;
            if (c.G > 64) color |= 2;
            if (c.B > 64) color |= 1;

            // 회색 계열 보정
            bool isGray =
                Math.Abs(c.R - c.G) < 20 &&
                Math.Abs(c.G - c.B) < 20;

            if (isGray)
            {
                int gray = (c.R + c.G + c.B) / 3;

                if (gray < 40)
                    return ConsoleColor.Black;

                if (gray < 120)
                    return ConsoleColor.DarkGray;

                if (gray < 200)
                    return ConsoleColor.Gray;

                return ConsoleColor.White;
            }

            // 밝은 색 여부
            int max = Math.Max(c.R, Math.Max(c.G, c.B));

            if (max > 180)
                color |= 8;

            return (ConsoleColor)color;
        }

        public static (int x, int y) GetImageHalfSize(string path)
        {
            Bitmap img = new Bitmap(path);

            int x = img.Width / 2;
            int y = img.Height / 2;

            return (x, y);
        }

        public static void DrawCharacter(string path)
        {
            Bitmap img = new Bitmap(path);

            int x = (Console.WindowWidth / 2) - (img.Width / 2);
            int y = (Console.WindowHeight / 2) - (img.Height / 4);

            for (int iy = 0; iy < img.Height - 1; iy += 2)
            {
                for (int ix = 0; ix < img.Width; ix++)
                {
                    Color top = img.GetPixel(ix, iy);
                    Color bottom = img.GetPixel(ix, iy + 1);

                    Console.SetCursorPosition(x + ix, y + (iy / 2));

                    Console.ForegroundColor = GetConsoleColor(top);
                    Console.BackgroundColor = GetConsoleColor(bottom);

                    Console.Write("▀");
                }
            }

            Console.ResetColor();
        }

        public static void DrawCharacter(string path, int offsetX, int offsetY)
        {
            Bitmap img = new Bitmap(path);

            int x = img.Width;
            int y = img.Height;

            for (int iy = 0; iy < img.Height - 1; iy += 2)
            {
                for (int ix = 0; ix < img.Width; ix++)
                {
                    Color top = img.GetPixel(ix, iy);
                    Color bottom = img.GetPixel(ix, iy + 1);

                    Console.SetCursorPosition(ix + offsetX, (iy / 2) + offsetY);

                    Console.ForegroundColor = GetConsoleColor(top);
                    Console.BackgroundColor = GetConsoleColor(bottom);

                    Console.Write("▀");
                }
            }

            Console.ResetColor();
        }

        public static void PrintCharacter(Character character, (float ratioX, float ratioY) locate, int offsetX = 0)
        {
            // 커서 좌표 맞춰서 캐릭터 그리기
            int x = ConsoleUtil.GetSizeByRatio(Console.WindowWidth, locate.ratioX);
            int y = ConsoleUtil.GetSizeByRatio(Console.WindowHeight, locate.ratioY);

            DrawCharacter(character.CharacterData.ImagePath, x, y);

            // 정보 출력
            (int sizeX, int sizeY) = GetImageHalfSize(character.CharacterData.ImagePath);
            character.ShowInfo(offsetX + sizeX - 5, y + sizeY);
        }

        // 현재 선택한 텍스트를 알려주는 문자열 반환
        public static string GetTextLength(int index, int selected, string str)
        {
            if (index == selected)
            {
                return $"[ {str} ]";
            }
            else
            {
                return $"  {str}  ";
            }
        }

        // 게임 화면 프레임 그리기
        public static void RenderGameFrame()
        {
            // 화면 지우기
            Console.Clear(); 
            Console.Write("\x1b[3J"); 
            
            // 게임 외곽선 그리기
            DrawBox();
        }

        public static void DrawInventory()
        {
            // 게임 외곽 그리기
            RenderGameFrame();

            DrawBox(66, 5, 35, 22);
        }
    }
}
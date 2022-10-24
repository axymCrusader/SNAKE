using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNAKE
{
    class Game
    {
        protected const int PGHEIGHT = 22;
        protected const int PGHWIDTH = 28;
        protected static string[,] PlayGround = new string[PGHWIDTH, PGHEIGHT];         // Массив игрового поля (выводим на консоль как на поле 28*22)
        protected static List<int[]> SnakeList = new List<int[]>();              // Змея (состоит из списка одномерных массивов - в каждом значения X,Y)
        public static ConsoleKeyInfo OldPressedKey;                                 // Временное хранилище для нажатой игроком кнопки
        public static bool Started = false;
        public static bool GameEnded = false;
        public static bool GameWin = false;
        protected const ConsoleColor COLOR_FRAME = ConsoleColor.Red;
        protected const ConsoleColor COLOR_SNAKE = ConsoleColor.Green;
        protected const ConsoleColor COLOR_FOOD = ConsoleColor.Yellow;
        
        public bool DirectionCheck(ConsoleKeyInfo newKey, ConsoleKeyInfo oldKey)  //  Метод, проверяющий не захотел ли пользователь развернуться на 180 градусов (так нельзя)                                                                                   //  Метод, не позволяющий двигаться быстрее нажимая клавишу движения в сторону в которую производится движение 
        {
            if (newKey.Key == ConsoleKey.UpArrow && oldKey.Key == ConsoleKey.DownArrow)
            {
                return false; // Если пользователь двигался вниз и хочет начать движение вверх
            }
            if (newKey.Key == ConsoleKey.DownArrow && oldKey.Key == ConsoleKey.UpArrow)
            {
                return false; // Если пользователь двигался вверх и хочет начать движение вниз
            }
            if (newKey.Key == ConsoleKey.LeftArrow && oldKey.Key == ConsoleKey.RightArrow)
            {
                return false; // Если пользователь двигался вправо и хочет начать движение влево
            }
            if (newKey.Key == ConsoleKey.RightArrow && oldKey.Key == ConsoleKey.LeftArrow)
            {
                return false; // Если пользователь двигался влево и хочет начать движение вправо
            }
            if (newKey.Key == ConsoleKey.UpArrow && oldKey.Key == ConsoleKey.UpArrow)
            {
                return false; // Повторное нажатие вверх 
            }
            if (newKey.Key == ConsoleKey.DownArrow && oldKey.Key == ConsoleKey.DownArrow)
            {
                return false; // Повторное нажатие вниз 
            }
            if (newKey.Key == ConsoleKey.LeftArrow && oldKey.Key == ConsoleKey.LeftArrow)
            {
                return false; // Повторное нажатие влево
            }
            if (newKey.Key == ConsoleKey.RightArrow && oldKey.Key == ConsoleKey.RightArrow)
            {
                return false; // Повторное нажатие вправо
            }
            else
            {
                return true;
            }
        }
        public bool Handlekey(ConsoleKeyInfo key)                        // Метод, проверяющий что переданная клавиша - одна из разрешенных 
        {
            return (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.LeftArrow ||  key.Key == ConsoleKey.DownArrow);
        }
        protected void DrawFrame(int X, int Y)
        {
            Console.SetCursorPosition(X*2, Y);
            Console.Write("#");
        }
        public void DrawBarriers(int X, int Y)
        {
            Console.SetCursorPosition(X * 2, Y);
            Console.Write("█");
        }
        public void DrawFood(int X, int Y)
        {
            Console.ForegroundColor = COLOR_FOOD;
            Console.SetCursorPosition(X * 2, Y);
            Console.Write("o");
        }
        public void DrawSnake(int X, int Y)
        {
            Console.ForegroundColor = COLOR_SNAKE;
            Console.SetCursorPosition(X * 2, Y);
            Console.Write("@");
        }
        public void Draw(int X, int Y, string text)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(text);
        }
        public void GenerateFrame()
        {
            for (int j = 0; j < PGHEIGHT; j++)              // Высота
            {

                for (int i = 0; i < PGHWIDTH; i++)           // Ширина
                {
                    if (i == 0 || j == 0 || j == PGHEIGHT - 1 || i == PGHWIDTH - 1)
                    {
                        PlayGround[i, j] = "#";          // Границы игрового поля 
                        DrawFrame(i, j);

                    }
                    else
                    {
                        PlayGround[i, j] = " ";          // Игровое поле
                    }
                }
            }
        }
        public void Clear(int X, int Y)
        {
            Console.SetCursorPosition(X * 2, Y);
            Console.Write(" ");
        }
        public  void GenerateFood()                                      // Генерация еды для змеи в случайном месте игрового поля
        {

            var rnd = new Random();
            int X = rnd.Next(2, PGHWIDTH - 3);
            int Y = rnd.Next(2, PGHEIGHT - 3);

            while (true)
            {
                if (PlayGround[X, Y].Equals("@"))                  // Если случайные значения попадают в тело змеи - генерируем заново. 
                {
                    X = rnd.Next(2, PGHWIDTH - 3);
                    Y = rnd.Next(2, PGHEIGHT - 3);
                }
                else
                {
                    PlayGround[X, Y] = "+";
                    DrawFood(X, Y);
                    break;
                }
            }
        }
        public void GenerateBarriers()                                      
        {

            var rnd = new Random();
            int X = rnd.Next(2, PGHWIDTH - 3);
            int Y = rnd.Next(2, PGHEIGHT - 3);

            while (true)
            {
                if (PlayGround[X, Y].Equals("█"))                  // Если случайные значения попадают в тело змеи - генерируем заново. 
                {
                    X = rnd.Next(2, PGHWIDTH - 3);
                    Y = rnd.Next(2, PGHEIGHT - 3);
                }
                else
                {
                    PlayGround[X, Y] = "x";
                    DrawBarriers(X, Y);
                    break;
                }
            }
        }
        public void ShowScore()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(56, 0);
            Console.Write($"Score: {SnakeList.Count}\t\t\t");

        }
        public void GameOver()
        {
            Console.SetCursorPosition(14, 10);
            Console.Write("Game Over. Your Score: "); Console.WriteLine(SnakeList.Count);
        }
        public void GameWon()
        {
            Console.SetCursorPosition(14, 10);
            Console.Write("Game Won. Your Score: "); Console.WriteLine(SnakeList.Count);
        }
        
          
    }
}

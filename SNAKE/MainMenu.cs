using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SNAKE
{

    class MainMenu
    {
        public static int Speed = 150;

        static object locker = new object();
        private static void DisplayMenu()
        {
            Console.WriteLine("1. New game");
            Console.WriteLine("2. Options");
            Console.WriteLine("3. Exit");
        }
        private static void DisplayOptionsMenu()
        {
            Console.WriteLine("1. Change Difficulty");
            Console.WriteLine("2. Exit");
        }
        private static void GameOptions()
        {
            Console.Clear();
            DisplayOptionsMenu();
            var userCommand = Console.ReadKey().Key;
            switch (userCommand)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    Console.WriteLine($"Speed: {Speed}");
                    Console.Write("Set new Speed: ");
                    Speed = int.Parse(Console.ReadLine());
                    Console.Clear();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    DisplayMenu();
                    break;
            }
        }
        private static void Exit()
        {
            Environment.Exit(0);
        }
        private static void StartGame()
        {
            Game game = new Game();
            Snake snake = new Snake();
            Console.CursorVisible = false;
            Thread SnakeMove = new Thread(new ThreadStart(SnakeMoving));    // Отдельный поток для постоянного движения змеи      
            game.GenerateFrame();                                  // Создаем игровое поле
            for (int i = 0; i < 5; i++)
            {
                game.GenerateBarriers();
            }
            game.Draw(18, 10, "To start press '↑'");
            while (true)                                                    // Отлавливаем нажатие клавиши 
            {
                ConsoleKeyInfo PressedKey = Console.ReadKey(true);          // Записываем значение кнопки
                if (!Game.Started && game.Handlekey(PressedKey))          // Если была нажата кнопка arrr - начинаем игру 
                {
                    Game.Started = true;
                    game.GenerateFood();
                    SnakeMove.Start();
                }
                game.Draw(18, 10, "                  ");
                if (Game.Started && !Game.GameEnded && game.Handlekey(PressedKey) && game.DirectionCheck(PressedKey, Game.OldPressedKey))
                {                                                           // Если игра начата, передаем нажатие кнопки для дальнейшей обработки
                    lock (locker)
                    {
                        snake.Move(PressedKey);                      // Передаем нажатую кнопку для движение в заданную сторону
                    }
                }

                if (Game.GameEnded)
                {
                    game.GameOver();
                }
                if (Game.GameWin)
                {
                    game.GameWon();
                }
            }
            
        }
        private static void SnakeMoving()                                      // Отдельный поток для движения змеи без участия игрока 
        {
            Game game = new Game();
            Snake snake = new Snake();
            while (true)
            {
                lock (locker)
                {
                    snake.Move(Game.OldPressedKey);              // Движемся в ту сторону в которую двигались до этого 
                    Thread.Sleep(Speed);

                    if (Game.GameEnded)
                    {
                        game.GameOver();
                        break;
                    }
                    if (Game.GameWin)
                    {
                        game.GameWon();
                        break;
                    }
                }
            }
        }
        private static void Main()
        {
            Console.Clear();
            var menuCommand = new ConsoleKey();

            while (menuCommand != ConsoleKey.D4)
            {
                DisplayMenu();
                menuCommand = Console.ReadKey().Key;

                switch (menuCommand)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        StartGame();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        GameOptions();
                        break;
                    case ConsoleKey.D3:
                        Exit();
                        break;
                }

                Console.Clear();
            }
        }
    }
}
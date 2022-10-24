using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNAKE
{
    class Snake : Game
    {
        public void Move(ConsoleKeyInfo pressedkey)
        {

            OldPressedKey = pressedkey;
            ShowScore();
            GenerateSnake();     // Создание змейки первый раз в игре         

            for (int n = SnakeList.Count - 1; n >= 1; n--)    // Сохраняем в n максимальный индекс списка и идем от большего к меньшему
            {
                SnakeList[n][0] = SnakeList[n - 1][0];  // X 
                SnakeList[n][1] = SnakeList[n - 1][1];  // Y
            }

            SnakeList[0][0] += Direction(pressedkey)[0];                  // X "головы" змеи
            SnakeList[0][1] += Direction(pressedkey)[1];                  // Y "головы" змеи

            Bump();      // Проверка выхода с границ массива
            Eat();          // Проверка пересечение змеи с едой 
            Barriers(); // Проверка пересечения змеи с преградой 
            for (int j = 0; j < SnakeList.Count; j++)
            {
                if (j < (SnakeList.Count - 1))
                {
                    DrawSnake(SnakeList[j][0], SnakeList[j][1]);
                    PlayGround[SnakeList[j][0], SnakeList[j][1]] = "@";     // Закрашиваем поле где будет змея
                }
                else
                {
                    Clear(SnakeList[j][0], SnakeList[j][1]);
                    PlayGround[SnakeList[j][0], SnakeList[j][1]] = " ";     // Закрашиваем поле откуда змея ушла 
                }
            }
            hanibal();   // Проверка врезания головы змеи в тело змеи
            Win();
        }
        public void Barriers()
        {
            if (PlayGround[SnakeList[0][0], SnakeList[0][1]].Equals("x"))       // Если координаты головы змеи и еды совпадают
            {
                GameEnded = true;
            }
        }
        public void GenerateSnake()
        {
            if (SnakeList.Count < 4)
            {
                int[] tempcord = new int[2] { PGHWIDTH / 2, PGHEIGHT / 2 };
                SnakeList.Add(tempcord);
            }
        }
        public void hanibal()
        {
            if (SnakeList.Count >= 4)
            {
                for (int t = 1; t < SnakeList.Count - 1; t++)       // Последний элемент змеи существует только что бы закрашивать поле после змеи
                {
                    if ((SnakeList[0][0] == SnakeList[t][0]) && (SnakeList[0][1] == SnakeList[t][1]))       // Если врезаемся в тело змеи или границы поля
                    {
                        GameEnded = true;        // Завершаем игру
                    }
                }
            }
        }
        public int[] Direction(ConsoleKeyInfo pressedkey)   // Получаем координаты движения в зависимости от нажатой клавиши. 
        {
            int[] direction = new int[2];

            if (pressedkey.Key == ConsoleKey.UpArrow)
            {
                direction[0] = 0;   // X
                direction[1] = -1;  // Y - вверх
            }
            if (pressedkey.Key == ConsoleKey.RightArrow)
            {
                direction[0] = 1;   // X - вправо
                direction[1] = 0;   // Y
            }
            if (pressedkey.Key == ConsoleKey.LeftArrow)
            {
                direction[0] = -1;  // X - влево
                direction[1] = 0;   // Y
            }
            if (pressedkey.Key == ConsoleKey.DownArrow)
            {
                direction[0] = 0;   // X
                direction[1] = 1;   // Y - вниз 
            }

            return direction;
        }
        public void Eat()
        {
            if (PlayGround[SnakeList[0][0], SnakeList[0][1]].Equals("+"))       // Если координаты головы змеи и еды совпадают
            {
                int[] tempcord = new int[2] { PGHWIDTH / 2, PGHEIGHT / 2 };
                SnakeList.Add(tempcord);          // Добавляем элемент змеи
                GenerateFood();                    // Создаем новую еду   
            }
        }
        public void Bump()
        {
            if (SnakeList[0][0] < 1 || SnakeList[0][0] > PGHWIDTH - 2 || SnakeList[0][1] < 1 || SnakeList[0][1] > PGHEIGHT - 2)       // Если врезаемся в тело змеи или границы поля
            {
                GameEnded = true;                // Завершаем игру
            }
        }
        public void Win()
        {
            if (SnakeList.Count == 100)
            {
                GameWin = true;
            }
        }
    }
}

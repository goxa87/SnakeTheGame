using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace DrunkSnake
{
    /// <summary>
    /// поле
    /// </summary>
    class Field
    {
        /// <summary>
        /// ширина поля
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// высота поля
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Объект змеи
        /// </summary>
        public Snake snake{ get; set; }

        /// <summary>
        /// стена игрового поля
        /// </summary>
        public Wall wall { get; set; }
        /// <summary>
        /// скоросьт таймера
        /// </summary>
        public int TimerSpeed { get; set; }

        /// <summary>
        /// задача для контроля направления в цикле старт
        /// </summary>
        Task controller { get; set;}
        /// <summary>
        /// счет
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// лучший счет из файла
        /// </summary>
        string Best { get; set; }

        /// <summary>
        /// лучший счет цифра
        /// </summary>
        public int BestScore { get; set; }
        /// <summary>
        /// текущее направление (0 вертикаль - 1 горизонталь) (-1 уменьшение - 0 тоже -+1 добавление )
        /// </summary>
        public int[] curDirection { get; set; }
        //++ здесь обработать ивент
        /// <summary>
        /// флаг продолжения игры
        /// </summary>
        bool GameContinue { get; set; }

        /// <summary>
        /// Объект фрукта на поле
        /// </summary>
        public Food food { get; set; }

        object locker = new object();

        /// <summary>
        /// токен отмены цикла ввода направления
        /// </summary>
        CancellationTokenSource token = new CancellationTokenSource();

        /// <summary>
        /// инициализация игрового поля
        /// </summary>
        /// <param name="timer">длинна задержки при отрисовки</param>
        /// <param name="W">стена игрового поля</param>
        public Field(int timer, Wall W)
        {
            Console.CursorVisible = false;

            //BackgroundColor = ConsoleColor.DarkGray;

            wall = W;  // границы игроого поля
            Width = WindowWidth;            
            Height = WindowHeight;

            curDirection = new int[2] { 0,1}; //начальное направление движения вправо
            TimerSpeed = timer;
            snake = new Snake(3, wall.LeftTop[0]+5, wall.LeftTop[1]+5, wall, EndGame); // змея
            food = new Food(wall, snake);
            Score = 0;
            GameContinue = true; // игра продолжается

            // выгрузка лучшего значения
            if (Logger.GetBest() != null)
            {
                BestScore = Logger.GetBest().Count;  // поле значения
                Best = Logger.GetBest().ToString();  // строка для отображения
            }
            else
            {
                BestScore = 0;
                Best = "Лучший счет не установлен";
            }
        }

        /// <summary>
        /// отрисовка змеи и поля
        /// </summary>
        public void Drawing()
        {
            DrawScore(); // рисует счет слева сверху

            foreach (var e in snake.position) // отрисовка змеи
            {
                SetCursorPosition(e[0], e[1]);
                ForegroundColor = ConsoleColor.Yellow;
                Console.Write("*");                          
            }
            //отрисовка фрукта
            ForegroundColor = ConsoleColor.Red;
            SetCursorPosition(food.W, food.H);
            Console.Write("@");
        }
        /// <summary>
        /// Зауск анимации
        /// </summary>
        public void Start()
        {
            DrawField(); 
            DrawInfo();

            var TaskKey = Task.Factory.StartNew(ChangeDirection, token.Token, token.Token);

            while (GameContinue)  // игровой цикл пока не изменится переменная а изменяется колда die класса snake
            {         
                try
                {
                    lock (locker)
                    {
                        Drawing(); // отрисовать
                    }

                    snake.Move(curDirection[0], curDirection[1]); // передвинуть змею

                    // съедание фрукта
                    if ((snake.position[0][0] == food.W) && (snake.position[0][1] == food.H)) // если координаты еды и головы змеи совпали
                    {
                        snake.Grow();  // змея растет
                        Score++; // счет увеличивается
                        TimerSpeed -=2;  // ожидание следующей итерации уменьшается
                        if (TimerSpeed < 5) // но не в - и не в бесконечность
                          TimerSpeed = 5;
                        food.CahgePosition(snake);  // еда двигается на основе новой змеи
                    }

                    Thread.Sleep(TimerSpeed); //скорость игры
                
                    //if(!token.Token.IsCancellationRequested)

                    //token.Cancel();
                    //token.Dispose();
                }
                catch(AggregateException e)
                {
                    //Debug.WriteLine(e.Message);
                    //controller.Wait();
                }
                finally
                {
                    //if (controller.IsCanceled)
                    //    controller.Dispose();
                    
                }

                //if(controller.IsCompleted) // освобождаем ресурсы тааска
                //    controller.Dispose();
            }

            token.Cancel();
            token.Dispose();
        }

        ConsoleKey GetKey()
        {
            return ReadKey().Key;
        }

        /// <summary>
        /// Обработчик нажатия кнопок
        /// </summary>
        public void ChangeDirection(object cancel)
        {
            var t = (CancellationToken)cancel;  // токен отмены
            
            ConsoleKey consoleKey = ConsoleKey.A;

            while (true)
            {
                consoleKey = ReadKey().Key;

                switch (consoleKey)
                {
                    case ConsoleKey.LeftArrow:  // в зависимости от нажатой кнопы
                        {
                            if (((snake.position[0][0] - 1) != (snake.position[1][0]))) // если это движение не навстречу своему телу,  т.е еоордината по горизонтали-1
                                                                                        //это  не тоже что и 2й кубик змеи, то 
                            {  //  curDirection[0] = вертикаль : [1] - горизонталь + или - зависи от направления влево вправо или вверх вниз
                                curDirection[1] = -1; // направление меняется
                                curDirection[0] = 0;
                            }

                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {

                            if (((snake.position[0][1] - 1) != (snake.position[1][1])))
                            {
                                curDirection[0] = -1;
                                curDirection[1] = 0;
                            }
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (((snake.position[0][0] + 1) != (snake.position[1][0])))
                            {
                                curDirection[1] = 1;
                                curDirection[0] = 0;
                            }
                            break;

                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (((snake.position[0][1] + 1) != (snake.position[1][1])))
                            {
                                curDirection[1] = 0;
                                curDirection[0] = 1;
                            }
                            break;
                        }
                }
                consoleKey = ConsoleKey.A;
                if (t.IsCancellationRequested)
                    return;
            }            
        }

        /// <summary>
        /// отображает счет
        /// </summary>
        void DrawScore()
        {
            ForegroundColor = ConsoleColor.Green;
            SetCursorPosition(0, 0);
            Write("Score: " + Score);
        }

        /// <summary>
        /// орисовка границ игрового поля
        /// </summary>
        void DrawField()
        {
            ForegroundColor = ConsoleColor.Blue;
            for (int i = wall.LeftTop[0]; i <= wall.RightBottom[0];i++) // отрисовка горизонтальных линий стены
            {
                SetCursorPosition(i, wall.LeftTop[1]);
                Write("#");
                SetCursorPosition(i, wall.RightBottom[1]);
                Write("#");
            }

            for (int i = wall.LeftTop[1]; i <= wall.RightBottom[1]; i++) // отрисовка вертикальных линий стены
            {
                SetCursorPosition(wall.LeftTop[0],i);
                Write("#");
                SetCursorPosition(wall.RightBottom[0],i);
                Write("#");
            }
        }

        /// <summary>
        /// метод при завершении игры
        /// </summary>
        public void EndGame()
        {
            // завершить цикл отрисовки 
            GameContinue = false;
            // вывести сообщение о счете и завершении
            SetCursorPosition(2, 2);
            ForegroundColor = ConsoleColor.Red;
            Write("ИГРА ОКОНЧЕНА");
            SetCursorPosition(2, 3);
            Write($"ДОСТИГНУТЫЙ СЧЕТ: {Score}");
            Logger.SaveScore(Score);

            if (Score > BestScore)
            {
                SetCursorPosition(2, 5);
                Write($"ПОЗДРАВЛЯЮ! НОВЫЙ ЛУЧШИЙ СЧЕТ: {Score}");
            }
        }

        /// <summary>
        /// Отрисовка информациии справа (статическое)
        /// </summary>
        void DrawInfo()
        {
            ForegroundColor = ConsoleColor.Cyan;

            SetCursorPosition(0, 1);
            Write($"{Best}");

            ForegroundColor = ConsoleColor.DarkCyan;

            SetCursorPosition(35, 3);
            Write("ЗМЕЯ");

            SetCursorPosition(35, 5);
            Write("* Управление стрелочками");

            SetCursorPosition(35, 7);
            Write("* Нужно есть красные фрукты");

            SetCursorPosition(35, 9);
            Write("* Каждый фрукт увеличивает скорость ");
            SetCursorPosition(35, 10);
            Write("потому-что это кофейное зерно");

            SetCursorPosition(35, 12);
            Write("* Можно переходить через себя");

            SetCursorPosition(35, 14);
            Write("* От удара о стену змея дохнет");

            SetCursorPosition(35, 16);
            Write("* Использовано ООП, делегаты, многопоточность");

            SetCursorPosition(35, 18   );
            Write("* Сохраняет рядом с экзешником файл хайскора");



        }
    }
}

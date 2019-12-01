using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrunkSnake
{
    /// <summary>
    /// Змея
    /// </summary>
    class Snake
    {
        /// <summary>
        /// Длиннна змеи
        /// </summary>
        public int lenth { get; set; }

        /// <summary>
        /// 0-левый  1 - верх   2 - право   3- низ
        /// </summary>
        Wall wall { get; set; }

        /// <summary>
        /// внутри 0 значение это ширина  1 е значение высота
        /// </summary>
        public List<int[]>  position { get; set; }

        /// <summary>
        /// сбытие при умирании 
        /// </summary>
        public event OnHandle Dying;

        /// <summary>
        /// Зоздание змеи
        /// </summary>
        /// <param name="lenth">ее длинна</param>
        /// <param name="stW">ширина начала</param>
        /// <param name="stH">высота начала</param>
        /// <param name="wall">экземпляр стены</param>
        /// <param name="onHandle">метод для события умирания</param>
        public Snake(int lenth, int stW, int stH, Wall wall, OnHandle onHandle)
        {
            this.wall = wall;
            Dying += onHandle; // обработчик из field для связи с классом для остановки игры

            position = new List<int[]>(3); // ячейки змеи

            this.lenth = lenth;
            for (int i = 0; i < lenth; i++)
            {
                position.Add(new int[2] { stW-i, stH }); // располагаем ее гоизонально головой направо
            }
        }
        /// <summary>
        /// Движение
        /// </summary>
        /// <param name="up">-1 вверх, 0 тотже, 1 вниз </param>
        /// <param name="right"></param>

        public void Move(int up, int right)
        {
            // нужно затереть исмвол на последней сеции чтоб не перерисовывать весь экран
            Console.SetCursorPosition(position[position.Count - 1][0], position[position.Count - 1][1]);
            Console.Write(" ");

            //удаляем хвост
            position.RemoveAt(position.Count - 1);
            int newW, newH;
            //координаты для новой головы
            newW = position[0][0] + right;
            newH = position[0][1] + up;

            if ((position[0][0] + right) >= wall.RightBottom[0]) // при пересечении границы 
                Die();
            if ((position[0][0] + right) <= wall.LeftTop[0])   // горизонталь
                Die();

            if ((position[0][1] + up) >= wall.RightBottom[1])  //вертикаль
                Die();
            if ((position[0][1] + up) <= wall.LeftTop[1])
                Die();
            //если не умерла, то вставляем новую голову в тело
            position.Insert(0, (new int[] { newW, newH }));
        }

        /// <summary>
        /// РОст змеи на 1
        /// </summary>
        public void Grow()
        {
            position.Add(new int[2] { position[position.Count - 1][0], position[position.Count - 1][1] });
        }

        /// <summary>
        /// метод при умирании
        /// </summary>
        public void Die()
        {
            Dying.Invoke(); // вызов метода из field
        }

    }
}

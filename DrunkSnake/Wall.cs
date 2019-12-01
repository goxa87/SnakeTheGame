using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrunkSnake
{
    /// <summary>
    /// этот класс границы игрового поля
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// 0-w: 1-H
        /// </summary>
        public int[] LeftTop { get; }

        /// <summary>
        /// 0-w: 1-H
        /// </summary>
        public int[] RightBottom { get; }

        /// <summary>
        /// Инициализация класса стены 0-w: 1-H
        /// </summary>
        /// <param name="TL">левый верхний 0-w: 1-H</param>
        /// <param name="RB">правй нижний 0-w: 1-H</param>
        public Wall(int[] TL, int[] RB)
        {
            LeftTop = TL;
            RightBottom = RB;
        }
    }
}

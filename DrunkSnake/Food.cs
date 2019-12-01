using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrunkSnake
{
    class Food
    {
        /// <summary>
        /// координтат по ширине 
        /// </summary>
        public int W { get; set; }

        /// <summary>
        /// Координата по высоте 
        /// </summary>
        public int H { get; set; }        
       
        /// <summary>
        /// стена
        /// </summary>
        Wall wall { get; }
        /// <summary>
        /// рандомизатор
        /// </summary>
        Random rnd { get; }

        //readonly int scrW = Console.WindowWidth;
        //readonly int scrH= Console.WindowHeight;

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="w">максимальная ширина</param>
        /// <param name="h">максимальная высота</param>
        public Food(Wall wall, Snake S)
        {
            rnd = new Random();
            this.wall = wall;
            FindFreePosition(wall, S);                         
        }

        /// <summary>
        /// поиск пока не найдет свободное место на экране
        /// </summary>
        /// <param name="w">макс ширина</param>
        /// <param name="h">макс высота</param>
        /// <param name="S">змея</param>
        /// <returns></returns>
        void FindFreePosition(Wall wall, Snake S)
        {            
            bool flag = false;

            //пока не найдет
            while(!flag)
            {
                //генерация от бортов стены
                W = rnd.Next(wall.LeftTop[0] + 1, wall.RightBottom[0] - 1);
                H = rnd.Next(wall.LeftTop[1] + 1, wall.RightBottom[1] - 1);
                //перебор змеи
                foreach (var e in S.position)
                {
                    if (W == e[0] && H == e[1]) // если та же позиция чтои ячейка змеи
                    {
                        break;
                    }
                    flag = true; // если ни разу не брейкануло значит совпадений не было, значит можно выйти                                         
                }
            }    
        }

        /// <summary>
        /// Смена позиции фрукта
        /// </summary>
        /// /// <param name="S">змея</param>
        public void CahgePosition(Snake S)
        {
            FindFreePosition(wall,S);
        }
    }
}

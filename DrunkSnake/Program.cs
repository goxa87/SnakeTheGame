using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrunkSnake
{
    public delegate void OnHandle();

    class Program
    {        
        static void Main(string[] args)
        {
            //граница поля
            Wall wall = new Wall(new int[2] { 3, 3 }, new int[2] { 30, 23 }); // cсдеать переачу
            //поле
            Field field = new Field(100,wall);
            //запуск игры
            field.Start();
            Console.ReadKey();
        }
    }
}

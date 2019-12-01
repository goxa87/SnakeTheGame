using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrunkSnake
{
    /// <summary>
    /// аргументы для передачи из логера
    /// </summary>
    public class LoggerArgs
    {
        /// <summary>
        /// счет
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// имя пользователя
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// дата установки рекорда
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Инициализация для передачи параметров
        /// </summary>
        /// <param name="c">счет</param>
        /// <param name="s">имя</param>
        /// <param name="date">дата</param>
        public LoggerArgs(int c, string s, DateTime date)
        {
            Count = c;
            Name = s;
            Date = date;
        }

        /// <summary>
        /// вывод содержимого в виде строки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Лучший: {Count} установлен {Date.ToShortDateString()} пользователем {Name}";
        }
    }
}

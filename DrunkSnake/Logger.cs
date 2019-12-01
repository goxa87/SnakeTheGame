using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DrunkSnake
{
    /// <summary>
    /// Занимается загрузкой и сохранением результатов счета
    /// </summary>
    public static class Logger
    {
        readonly static string path = "SnakeBestScore.txt";

        /// <summary>
        /// Сохраняет текщий результат в файл 
        /// </summary>
        /// <param name="score"></param>
        public static void SaveScore(int score )
        {

            StringBuilder SB = new StringBuilder();  // собираем строку для записи
            SB.Append(score.ToString());
            SB.Append(" Достигнут ");
            SB.Append(DateTime.Now.ToShortDateString());
            SB.Append(" Пользователь ");
            SB.Append(Environment.UserName);
            string toAdd = SB.ToString();

            try
            {
                using (StreamWriter SW = new StreamWriter(path, true, Encoding.UTF8))
                {
                    SW.WriteLine(toAdd);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Полчучение лучшей записи из фалйа сохранения
        /// </summary>
        /// <returns></returns>
        public static LoggerArgs GetBest()
        {
            List<string> array = new List<string>(); // список строк
            string s; // переменная челнок для ридера
            if (File.Exists(path)) // если файл существует
            {
                try
                {
                    using (StreamReader SR = new StreamReader(path, Encoding.UTF8))
                    {
                        while ((s = SR.ReadLine()) != null) // читаем по строек и добавляем  в списое
                        {
                            array.Add(s);
                        }
                    }
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
            }
            int best = 0;  // результат если ничего нет вернет 0
            LoggerArgs arg = null;
            try
            {
                foreach (var e in array) // перебираем
                {
                    var spl = e.Split(' ');
                    string num = spl[0]; // делим строку пробелами и берем первый элемент - это число мы ищем
                    bool flag = int.TryParse(num, out int current); // могут быть ошибки
                    if (flag != false && current > best) // если больше добавляем
                    {
                        best = current;
                        // собираем дату
                         // это неэффективно. переделать
                        var dat = spl[2].Split('.');
                        int.TryParse(dat[0], out int day);
                        int.TryParse(dat[1], out int month);
                        int.TryParse(dat[2], out int year);

                        arg = new LoggerArgs(current, spl[4], new DateTime(year, month, day));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return arg;
        }

    }
}

using System;
using System.Globalization;
using System.Text;

namespace NumericalDifferentiation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine(
            "------------------------------------------------------------\n" +
            "Для функции f (x), заданной в виде таблицы в пяти узлах xi, i = 0, 1, 2, 3, 4,\n" +
                "найти значения ее 1-й и 2-й производных в первых четырех узлах,\n" +
                "используя формулы численного дифференцирования\n" +
            "------------------------------------------------------------\n");

            // Настройка культуры, чтобы точка и запятая воспринимались адекватно
            CultureInfo culture = CultureInfo.InvariantCulture;

            double[] x = new double[4];
            double[] y = new double[4];

            Console.WriteLine("=== Ввод данных для численного дифференцирования (первые 4 узла) ===");
            Console.WriteLine("Введите значения через точку или запятую");
            Console.WriteLine();

            // 1. Ввод данных пользователем
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Введите x[{i}]: ");
                x[i] = double.Parse(Console.ReadLine().Replace(',', '.'), culture);

                Console.Write($"Введите y[{i}]: ");
                y[i] = double.Parse(Console.ReadLine().Replace(',', '.'), culture);
                Console.WriteLine("-------------------------");
            }

            // Автоматический расчет шага h
            double h = x[1] - x[0];
            double h2 = Math.Pow(h, 2);

            Console.Clear();
            Console.WriteLine($"Результаты расчетов (Шаг h = {h:F4}):");
            Console.WriteLine("------------------------------------------------------------");

            // 2. Вычисление первой производной f'(x)
            double[] d1 = new double[4];
            double coeff1 = 1 / (6 * h);
            d1[0] = coeff1 * (-11 * y[0] + 18 * y[1] - 9 * y[2] + 2 * y[3]);
            d1[1] = coeff1 * (-2 * y[0] - 3 * y[1] + 6 * y[2] - y[3]);
            d1[2] = coeff1 * (y[0] - 6 * y[1] + 3 * y[2] + 2 * y[3]);
            d1[3] = coeff1 * (-2 * y[0] + 9 * y[1] - 18 * y[2] + 11 * y[3]);

            // 3. Вычисление второй производной f''(x)
            double[] d2 = new double[4];
            d2[0] = (1 / h2) * (2 * y[0] - 5 * y[1] + 4 * y[2] - y[3]);
            d2[1] = (1 / h2) * (y[0] - 2 * y[1] + y[2]);
            d2[2] = (1 / h2) * (y[1] - 2 * y[2] + y[3]);
            d2[3] = (1 / h2) * (-y[0] + 4 * y[1] - 5 * y[2] + 2 * y[3]);

            // 4. Вывод результатов в таблицу
            Console.WriteLine("{0,-10} | {1,-10} | {2,-12} | {3,-12}", "x", "y", "f'(x)", "f''(x)");
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("{0,-10:F4} | {1,-10:F5} | {2,-12:F5} | {3,-12:F5}",
                    x[i], y[i], d1[i], d2[i]);
            }

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
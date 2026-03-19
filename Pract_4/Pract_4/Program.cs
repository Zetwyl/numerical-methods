using System;
using System.Globalization;
using System.Text;

namespace Pract_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine(
            "------------------------------------------------------------\n" +
            "Для функции f (x), заданной таблично в пяти узлах xi, i = 0, 1, 2, 3, 4, \n" +
            "приближенно вычислить определенный интеграл на отрезке [x0; x4], \n" +
            "используя формулы Ньютона – Котеса, прямоугольников, трапеций и Симпсона.\n" +
            "------------------------------------------------------------\n");

            // Настройка культуры, чтобы точка и запятая воспринимались адекватно
            CultureInfo culture = CultureInfo.InvariantCulture;

            double[] x = new double[5];
            double[] y = new double[5];

            Console.WriteLine("=== Ввод данных ===");
            Console.WriteLine("Введите значения через точку или запятую");
            Console.WriteLine();

            // 1. Ввод данных пользователем
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Введите x[{i}]: ");
                x[i] = double.Parse(Console.ReadLine().Replace(',', '.'), culture);

                Console.Write($"Введите y[{i}]: ");
                y[i] = double.Parse(Console.ReadLine().Replace(',', '.'), culture);
                Console.WriteLine("-------------------------");
            }

            // Автоматический расчет шага h
            double h = x[1] - x[0];

            Console.Clear();
            Console.WriteLine($"Результаты расчетов (Шаг h = {h:F4}):");
            Console.WriteLine("------------------------------------------------------------\n");

            // 1. Формула прямоугольников (левых)
            double resultRectangles = NumericalIntegrator.CalculateLeftRectangles(y, h);
            Console.WriteLine($"Метод прямоугольников:    {resultRectangles:F5}");

            // 2. Формула трапеций
            double resultTrapezoidal = NumericalIntegrator.CalculateTrapezoidal(y, h);
            Console.WriteLine($"Метод трапеций:           {resultTrapezoidal:F5}");

            // 3. Формула Симпсона (парабол)
            double resultSimpson = NumericalIntegrator.CalculateSimpson(y, h);
            Console.WriteLine($"Метод Симпсона:           {resultSimpson:F5}");

            // 4. Формула Ньютона — Котеса (для m=4)
            double a = x[0];
            double b = x[x.Length - 1];
            double resultNewtonCotes = NumericalIntegrator.CalculateNewtonCotes(y, a, b);
            Console.WriteLine($"Метод Ньютона — Котеса:   {resultNewtonCotes:F5}");

            Console.WriteLine("\nВывод: Наиболее точный результат дают формулы Ньютона — Котеса и Симпсона.");
            Console.ReadLine();
        }
    }

    public static class NumericalIntegrator
    {
        public static double CalculateLeftRectangles(double[] y, double h)
        {
            // I ≈ h * (y0 + y1 + y2 + y3)
            return h * (y[0] + y[1] + y[2] + y[3]);
        }

        public static double CalculateTrapezoidal(double[] y, double h)
        {
            // I ≈ h * ((y0 + y4)/2 + y1 + y2 + y3)
            return h * ((y[0] + y[4]) / 2.0 + y[1] + y[2] + y[3]);
        }

        public static double CalculateSimpson(double[] y, double h)
        {
            // I ≈ (h/3) * [ (y0 + y4) + 4*(y1 + y3) + 2*y2 ]
            return (h / 3.0) * ((y[0] + y[4]) + 4.0 * (y[1] + y[3]) + 2.0 * y[2]);
        }

        public static double CalculateNewtonCotes(double[] y, double a, double b)
        {
            // I ≈ ((b-a)/90) * (7*y0 + 32*y1 + 12*y2 + 32*y3 + 7*y4)
            return ((b - a) / 90.0) * (7.0 * y[0] + 32.0 * y[1] + 12.0 * y[2] + 32.0 * y[3] + 7.0 * y[4]);
        }
    }
}
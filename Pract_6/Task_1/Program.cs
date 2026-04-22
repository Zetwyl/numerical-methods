using System;

namespace Task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 4; // Количество неизвестных
            double epsilon = 0.001;

            double[,] C = new double[n, n];
            double[] f = new double[n];

            Console.WriteLine("\nШаг 1. Подготовка системы к виду x = Cx + f");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Строка {i + 1} (введите {n} коэф. перед x через пробел): ");
                string[] input = Console.ReadLine().Split(new[] { ' ' });
                for (int j = 0; j < n; j++) C[i, j] = double.Parse(input[j]);
            }

            Console.Write("Введите свободные члены f через пробел: ");
            string[] fInput = Console.ReadLine().Split(new[] { ' ' });
            for (int i = 0; i < n; i++) f[i] = double.Parse(fInput[i]);


            Console.WriteLine("\nШаг 2. Проверка условия сходимости |C| < 1");

            // Считаем норму по строкам
            double qRows = 0;
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++) sum += Math.Abs(C[i, j]);
                if (sum > qRows) qRows = sum;
            }

            // Считаем норму по столбцам
            double qCols = 0;
            for (int j = 0; j < n; j++)
            {
                double sum = 0;
                for (int i = 0; i < n; i++) sum += Math.Abs(C[i, j]);
                if (sum > qCols) qCols = sum;
            }

            Console.WriteLine($"- Норма по строкам  (||C||_inf): {qRows:F4}");
            Console.WriteLine($"- Норма по столбцам (||C||_1):   {qCols:F4}");

            double q;
            if (qRows < 1 && (qRows <= qCols || qCols >= 1))
            {
                q = qRows;
                Console.WriteLine($"Используем норму по строкам (q = {q:F4} < 1).");
            }
            else if (qCols < 1)
            {
                q = qCols;
                Console.WriteLine($"Используем норму по столбцам (q = {q:F4} < 1).");
            }
            else
            {
                q = Math.Min(qRows, qCols);
                Console.WriteLine($"ВНИМАНИЕ: q = {q:F4} >= 1. Условие не выполнено!");
            }

            Console.WriteLine("\nШаг 3. Начальное приближение x(0) = f");
            double[] xOld = (double[])f.Clone();
            PrintVector(0, xOld);

            Console.WriteLine("\nШаг 4 и 5. Выполнение итераций и проверка точности");
            double[] xNew = new double[n];
            int k = 0;
            double factor = q < 1 ? q / (1 - q) : 1.0;

            while (k < 100)
            {
                k++;
                // Считаем новый вектор x
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++) sum += C[i, j] * xOld[j];
                    xNew[i] = sum + f[i];
                }

                // Считаем Delta (макс. отклонение)
                double maxDiff = 0;
                for (int i = 0; i < n; i++)
                {
                    double diff = Math.Abs(xNew[i] - xOld[i]);
                    if (diff > maxDiff) maxDiff = diff;
                }

                // Оценка погрешности
                double error = factor * maxDiff;

                // Вывод каждой итерации
                PrintVector(k, xNew);
                Console.WriteLine($"   Delta = {maxDiff:F6} | Оценка погрешности: {error:F6}");

                if (error < epsilon)
                {
                    Console.WriteLine($"\nКритерий точности достигнут: {error:F6} < {epsilon}");
                    break;
                }

                Array.Copy(xNew, xOld, n);
            }

            Console.WriteLine("\n--- РЕЗУЛЬТАТ ---");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"x{i + 1} ≈ {xNew[i]:F4}");
            }
            Console.ReadKey();
        }

        static void PrintVector(int k, double[] x)
        {
            Console.Write($"x({k}) = ( ");
            for (int i = 0; i < x.Length; i++)
            {
                Console.Write($"{x[i]:F4}" + (i == x.Length - 1 ? "" : "; "));
            }
            Console.WriteLine(" )");
        }
    }
}
using System;

namespace Pract_5
{
    // Для красоты вывода
    class Format
    {
        // Вспомогательный метод для красивого отображения коэффициентов (скрывает 1)
        public static string Coeff(double val, bool isFirst = false)
        {
            if (val == 1) return isFirst ? "" : " ";
            if (val == -1) return "-";
            return val.ToString();
        }

        // Вспомогательный метод для красивого отображения x со степенью
        public static string Power(double power)
        {
            if (power == 1) return "x";
            return $"x^{power}";
        }
    }

    class Program
    {
        // Основная функция уравнения
        static double F(double x, int type, double a, double b, double c, double d, int s1, int s2)
        {
            double funcVal = 0;
            if (type == 1) funcVal = Math.Log(x);
            else if (type == 2) funcVal = Math.Sin(x);
            else if (type == 3) funcVal = Math.Cos(x);
            else if (type == 4) funcVal = Math.Pow(Math.Log(x), 2);

            return a * funcVal + s1 * b * Math.Pow(x, c) + s2 * d;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                // БЛОК ВВОДА
                Console.Clear();
                Console.WriteLine("=== ШАГ 1: Коэффициент A ===");
                Console.Write("Введите число перед функцией: ");
                double A = Convert.ToDouble(Console.ReadLine());

                Console.Clear();
                string displayA = (A == 1) ? "[?]" : (A == -1 ? "- [?]" : $"{A} * [?]");
                Console.WriteLine($"Текущее: {displayA}");
                Console.WriteLine("=== ШАГ 2: Выбор функции ===");
                Console.WriteLine("1 - ln(x)\n2 - sin(x)\n3 - cos(x)\n4 - ln^2(x)");
                int type = Convert.ToInt32(Console.ReadLine());
                string fName = type == 1 ? "ln(x)" : type == 2 ? "sin(x)" : type == 3 ? "cos(x)" : "ln^2(x)";

                Console.Clear();
                Console.WriteLine($"Текущее: {Format.Coeff(A, true)}{fName} [?]");
                Console.WriteLine("=== ШАГ 3: Первый знак ===");
                Console.Write("Введите знак (+ или -): ");
                int s1 = Console.ReadLine() == "+" ? 1 : -1;

                Console.Clear();
                Console.WriteLine($"Текущее: {Format.Coeff(A, true)}{fName} {(s1 == 1 ? "+" : "-")} [B]x");
                Console.WriteLine("=== ШАГ 4: Коэффициент B ===");
                Console.Write("Введите число B: ");
                double B = Convert.ToDouble(Console.ReadLine());

                Console.Clear();
                Console.WriteLine($"Текущее: {Format.Coeff(A, true)}{fName} {(s1 == 1 ? "+" : "-")} {Format.Coeff(B, true)}x^[C]");
                Console.WriteLine("=== ШАГ 5: Степень C ===");
                Console.Write("Введите степень x: ");
                double C = Convert.ToDouble(Console.ReadLine());

                Console.Clear();
                Console.WriteLine($"Текущее: {Format.Coeff(A, true)}{fName} {(s1 == 1 ? "+" : "-")} {Format.Coeff(B, true)}{Format.Power(C)} [?]");
                Console.WriteLine("=== ШАГ 6: Второй знак ===");
                Console.Write("Введите знак (+ или -): ");
                int s2 = Console.ReadLine() == "+" ? 1 : -1;

                Console.Clear();
                Console.WriteLine($"Текущее: {Format.Coeff(A, true)}{fName} {(s1 == 1 ? "+" : "-")} {Format.Coeff(B, true)}{Format.Power(C)} {(s2 == 1 ? "+" : "-")} [D]");
                Console.WriteLine("=== ШАГ 7: Число D ===");
                Console.Write("Введите свободное число D: ");
                double D = Convert.ToDouble(Console.ReadLine());

                // БЛОК ВЫВОДА
                Console.Clear();
                string partA = A == 1 ? "" : (A == -1 ? "-" : A.ToString());
                string partB = B == 1 ? "" : B.ToString();
                string partX = Format.Power(C);
                string finalEq = $"{partA}{fName} {(s1 == 1 ? "+" : "-")} {partB}{partX} {(s2 == 1 ? "+" : "-")} {D} = 0";

                Console.WriteLine("====================================================");
                Console.WriteLine("УРАВНЕНИЕ ГОТОВО: " + finalEq);
                Console.WriteLine("====================================================");

                // 1. Отделение корней
                Console.WriteLine("\n\t1. ОТДЕЛЕНИЕ КОРНЕЙ");
                double rootA = 0, rootB = 0;
                for (double x = 0.5; x < 10; x += 0.5)
                {
                    double y1 = F(x, type, A, B, C, D, s1, s2);
                    double y2 = F(x + 0.5, type, A, B, C, D, s1, s2);
                    if (y1 * y2 < 0)
                    {
                        rootA = x; rootB = x + 0.5;
                        Console.WriteLine($"f({x:F1}) = {y1:F4}");
                        Console.WriteLine($"f({x + 0.5:F1}) = {y2:F4}");
                        Console.WriteLine($"Корень лежит на отрезке [{rootA:F2}, {rootB:F2}].");
                        break;
                    }
                }

                if (rootB == 0) { Console.WriteLine("Корни не найдены."); Console.ReadKey(); continue; }

                // 2. Метод дихотомии
                Console.WriteLine("\n\t2. МЕТОД ДИХОТОМИИ (ε = 0.01)");
                double da = rootA, db = rootB, dx = 0;
                for (int i = 1; i <= 3; i++)
                {
                    dx = (da + db) / 2;
                    double dfx = F(dx, type, A, B, C, D, s1, s2);
                    Console.WriteLine($"Ит. {i}: x_mid = {dx:F4}; f(x_mid) = {dfx:F4} {(dfx < 0 ? "< 0" : "> 0")}");
                    if (F(da, type, A, B, C, D, s1, s2) * dfx < 0) db = dx; else da = dx;
                    Console.WriteLine($"Новый отрезок [{da:F4}, {db:F4}]");
                }
                Console.WriteLine($"ОТВЕТ: x ≈ {dx:F5}");

                // 3. Метод Ньютона
                Console.WriteLine("\n\t3. МЕТОД НЬЮТОНА (ε = 0.001)");
                double xn = rootB;
                Console.WriteLine($"Начальное приближение x0 = {xn}");
                for (int i = 1; i <= 2; i++)
                {
                    double fx = F(xn, type, A, B, C, D, s1, s2);
                    double dfx = (F(xn + 0.0001, type, A, B, C, D, s1, s2) - fx) / 0.0001;
                    double xNext = xn - fx / dfx;
                    Console.WriteLine($"Ит. {i}: x{i} = {xn:F5} - ({fx:F4}) / ({dfx:F4}) ≈ {xNext:F5}");
                    xn = xNext;
                }
                Console.WriteLine($"ОТВЕТ: x ≈ {xn:F5}");

                // 4. Метод хорд
                Console.WriteLine("\n\t4. МЕТОД ХОРД");
                double hX0 = rootA, hX1 = rootB;
                int iterH = 0;
                for (int i = 1; i <= 2; i++)
                {
                    iterH++;
                    double f0 = F(hX0, type, A, B, C, D, s1, s2);
                    double f1 = F(hX1, type, A, B, C, D, s1, s2);
                    double xNext = hX0 - f0 * (hX1 - hX0) / (f1 - f0);
                    Console.WriteLine($"Ит. {i}: x = {hX0:F4} - {f0:F4} * ({hX0 - hX1:F2}) / ({f0 - f1:F4}) ≈ {xNext:F5}");
                    hX0 = xNext;
                }
                double resH = hX0;
                Console.WriteLine($"ОТВЕТ: x ≈ {resH:F5}");

                // 5. Метод секущих
                Console.WriteLine("\n\t5. МЕТОД СЕКУЩИХ");
                double sX0 = rootA, sX1 = rootB;
                for (int i = 1; i <= 2; i++)
                {
                    double f0 = F(sX0, type, A, B, C, D, s1, s2);
                    double f1 = F(sX1, type, A, B, C, D, s1, s2);
                    double xNext = sX1 - f1 * (sX1 - sX0) / (f1 - f0);
                    Console.WriteLine($"Ит. {i}: x = {sX1:F4} - {f1:F4} * ({sX1 - sX0:F2}) / ({f1 - f0:F4}) ≈ {xNext:F5}");
                    sX0 = sX1; sX1 = xNext;
                }
                Console.WriteLine($"ОТВЕТ: x ≈ {sX1:F5}");

                // 6. Метод итераций
                Console.WriteLine("\n\t6. МЕТОД ИТЕРАЦИЙ");
                double xMidI = (rootA + rootB) / 2;
                double der = (F(xMidI + 0.0001, type, A, B, C, D, s1, s2) - F(xMidI, type, A, B, C, D, s1, s2)) / 0.0001;
                double lambda = -1.0 / der;
                Console.WriteLine($"λ = -1 / f'({xMidI:F2}) ≈ {lambda:F4}");
                double xi = xMidI;
                for (int i = 1; i <= 2; i++)
                {
                    double xiNext = xi + lambda * F(xi, type, A, B, C, D, s1, s2);
                    Console.WriteLine($"Ит. {i}: x{i} ≈ {xiNext:F5}");
                    xi = xiNext;
                }
                Console.WriteLine($"ОТВЕТ: x ≈ {xi:F5}");

                Console.WriteLine("\nНажмите любую клавишу для составления нового уравнения...");
                Console.ReadKey();
            }
        }
    }
}
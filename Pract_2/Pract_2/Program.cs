using System;
using System.Globalization;
using System.Text;

class LagrangeLab
{
    // f(x) = (ln x)^(p/q)
    static double F(double x, double p, double q)
    {
        return Math.Pow(Math.Log(x), p / q);
    }

    // Lagrange value at x using nodes xi, yi (degree 2)
    static double LagrangeAt(double x, double[] xi, double[] yi)
    {
        double sum = 0.0;
        int n = xi.Length;
        for (int i = 0; i < n; i++)
        {
            double term = yi[i];
            for (int j = 0; j < n; j++)
            {
                if (j == i) continue;
                term *= (x - xi[j]) / (xi[i] - xi[j]);
            }
            sum += term;
        }
        return sum;
    }

    // Numerical 3rd derivative via central finite-difference (symmetric)
    static double Deriv3(double x, double p, double q)
    {
        double h = 1e-3; // small step
        // formula: (f(x+2h) - 2 f(x+h) + 2 f(x-h) - f(x-2h)) / (2 h^3)
        return (F(x + 2 * h, p, q) - 2 * F(x + h, p, q) + 2 * F(x - h, p, q) - F(x - 2 * h, p, q)) / (2 * Math.Pow(h, 3));
    }

    // Safe parsing that accepts both comma and dot as decimal separator
    static double ReadDouble(string prompt, double defaultValue)
    {
        Console.Write(prompt);
        string s = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(s)) return defaultValue;
        s = s.Trim().Replace(',', '.');
        return double.Parse(s, CultureInfo.InvariantCulture);
    }

    static void Main(string[] args)
    {

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Параметры функции f(x) = (ln x)^(p/q) ===");

        double p = ReadDouble("Введите числитель p (например 13): ", 13.0);
        double q = ReadDouble("Введите знаменатель q (например 4): ", 4.0);

        Console.WriteLine("\n=== Ввод узлов xi (distinct) ===");
        double[] x = new double[3];
        for (int i = 0; i < 3; i++)
            x[i] = ReadDouble($"x{i} = ", 2.0 + i);

        double a = ReadDouble("\nВведите точку интерполирования a (например 2.5): ", 2.5);

        // Вычисляем yi = f(xi)
        double[] y = new double[3];
        for (int i = 0; i < 3; i++)
            y[i] = F(x[i], p, q);

        // Вывод y0,y1,y2 как в Mathcad
        Console.WriteLine("\n--- Узлы и значения функции ---");
        for (int i = 0; i < 3; i++)
            Console.WriteLine($"x{i} = {x[i]:G6}    y{i} = f(x{i}) = {y[i]:F6}");

        // Вычислим отдельные члены полинома в точке a (A,B,C)
        double A = y[0] * ((a - x[1]) * (a - x[2])) / ((x[0] - x[1]) * (x[0] - x[2]));
        double B = y[1] * ((a - x[0]) * (a - x[2])) / ((x[1] - x[0]) * (x[1] - x[2]));
        double C = y[2] * ((a - x[0]) * (a - x[1])) / ((x[2] - x[0]) * (x[2] - x[1]));
        double L2 = A + B + C;

        double f_a = F(a, p, q);
        double error = Math.Abs(f_a - L2);

        Console.WriteLine("\n--- Полином в точке a и его составные члены ---");
        Console.WriteLine($"a = {a:G6}");
        Console.WriteLine($"A = {A:F6}");
        Console.WriteLine($"B = {B:F6}");
        Console.WriteLine($"C = {C:F6}");
        Console.WriteLine($"L2(a) = {L2:F6}");
        Console.WriteLine($"F(a)  = {f_a:F6}");
        Console.WriteLine($"Точная погрешность |F(a)-L2(a)| = {error:E6}");

        // Нахождение M3 на отрезке [min(x0,x2), max(x0,x2)]
        double xmin = Math.Min(x[0], Math.Min(x[1], x[2]));
        double xmax = Math.Max(x[0], Math.Max(x[1], x[2]));

        double M3 = 0.0;
        double xAtM3 = xmin;
        double step = 0.01;

        for (double xi = xmin; xi <= xmax; xi += step)
        {
            double val = Math.Abs(Deriv3(xi, p, q));
            if (val > M3)
            {
                M3 = val; xAtM3 = xi;
            }
        }

        double omega = Math.Abs((a - x[0]) * (a - x[1]) * (a - x[2]));
        double theoreticalError = (M3 / 6.0) * omega;

        Console.WriteLine("\n--- Теоретическая оценка погрешности ---");

        Console.WriteLine($"Максимальное значение третьей производной на интервале [{xmin}, {xmax}]");
        Console.WriteLine($"M3 ≈ {M3:E6} (достигается примерно при x ≈ {xAtM3:F4})");

        Console.WriteLine($"\nω(a) = |(a-x0)(a-x1)(a-x2)|");
        Console.WriteLine($"ω(a) ≈ {omega:E6}");

        Console.WriteLine($"\nОценка погрешности:");
        Console.WriteLine($"R2(a) ≈ (M3 / 6) * ω(a)");
        Console.WriteLine($"R2(a) ≈ {theoreticalError:E6}");

        // Таблица для графика (x, f(x), L2(x))
        Console.WriteLine("\n--- Таблица значений (шаг 0.2) ---");
        Console.WriteLine(" x \t   f(x) \t\t L2(x)");
        for (double xi = xmin; xi <= xmax + 1e-9; xi += 0.2)
        {
            double fx = F(xi, p, q);
            double Li = LagrangeAt(xi, x, y);
            Console.WriteLine($"{xi,5:F2} \t {fx,10:F6} \t {Li,10:F6}");
        }
    }
}
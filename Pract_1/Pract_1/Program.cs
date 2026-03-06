using System;
using System.Text;


namespace Pract_1
{
    public class Rounder
    {
        public void ShowDetailedRounding(double x, double delta, double sigmaPercent)
        {
            // Расчет дельты если ее нет
            double currentDelta = (delta == 0 && sigmaPercent > 0)
                ? Math.Abs(x) * (sigmaPercent / 100.0)
                : delta;

            // Поиск m (через условие Δ < 10^-m)
            int m = 0;
            for (int i = 5; i >= -5; i--)
            {
                if (currentDelta < Math.Pow(10, -i)) { m = i; break; }
            }

            Console.WriteLine($"\nStep 1: Check limit absolute error");
            Console.WriteLine($"Δ = {currentDelta:F6}");
            Console.WriteLine($"\nStep 2: Find m where Δ < 10^-m");
            Console.WriteLine($"{currentDelta} < {Math.Pow(10, -m)} => m = {m}");
            Console.WriteLine($"\nStep 3: Round the number to {m} decimal places");
            Console.WriteLine($"\n- Answer: {Math.Round(x, m)}; {Math.Pow(10, -m)}");
        }

        public double RoundErrorUp(double error)
        {
            if (error == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(error)));
            return Math.Ceiling(error / scale) * scale;
        }
    }

    public class PrecisionComparer
    {
        private double SmartRoundSigma(double sigma)
        {
            if (sigma == 0) return 0;

            // Находим позицию первой значащей цифры (например, для 0.00084 это -4)
            int power = (int)Math.Floor(Math.Log10(sigma));

            // Если следующая цифра < 5, округляем вниз (0.00084 -> 0.0008)
            // Если следующая цифра >= 5, округляем вверх (0.0087 -> 0.009)
            // Используем AwayFromZero, чтобы 5 всегда округлялась вверх.
            return Math.Round(sigma, Math.Abs(power), MidpointRounding.AwayFromZero);
        }

        public (int num, int den, double rootVal) GetInputs()
        {
            Console.WriteLine("--- Task A: Precision Comparison ---");
            Console.Write("Enter numerator (a): ");
            int num = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter denominator (a): ");
            int den = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter root number (b): ");
            int temp = Convert.ToInt32(Console.ReadLine());

            double rootVal = temp;
            return (num, den, rootVal);
        }

        public void ShowDetailedComparison(int num, int den, double rootVal)
        {
            var rounder = new Rounder();

            double exactA = (double)num / den;
            double exactB = Math.Sqrt(rootVal);

            double approxA = Math.Round(exactA, 3, MidpointRounding.AwayFromZero);
            double approxB = Math.Round(exactB, 3, MidpointRounding.AwayFromZero);

            Console.WriteLine($"\n--- Step 1: High-precision calculations ---");
            Console.WriteLine($"Exact a = {num}/{den} = {exactA:F6}...");
            Console.WriteLine($"Exact b = √{rootVal} = {exactB:F6}...");

            // Абсолютные погрешности
            double rawDeltaA = Math.Abs(exactA - approxA);
            double rawDeltaB = Math.Abs(exactB - approxB);
            double deltaA = rounder.RoundErrorUp(rawDeltaA);
            double deltaB = rounder.RoundErrorUp(rawDeltaB);

            Console.WriteLine($"\n--- Step 2: Limit absolute errors (Δ) ---");
            Console.WriteLine($"Δa = {rawDeltaA:F6} ≤ {deltaA}");
            Console.WriteLine($"Δb = {rawDeltaB:F6} ≤ {deltaB}");

            // Относительные погрешности
            double rawSigmaA = deltaA / Math.Abs(approxA);
            double rawSigmaB = deltaB / Math.Abs(approxB);

            double sigmaA = SmartRoundSigma(rawSigmaA);
            double sigmaB = SmartRoundSigma(rawSigmaB);

            Console.WriteLine($"\n--- Step 3: Limit relative errors (δ) ---");
            Console.WriteLine($"δa = {deltaA} / {approxA} ≈ {rawSigmaA:F5} ≈ {sigmaA * 100}%");
            Console.WriteLine($"δb = {deltaB} / {approxB} ≈ {rawSigmaB:F5} ≈ {sigmaB * 100}%");

            Console.WriteLine($"\n--- Conclusion ---");
            if (sigmaA < sigmaB)
            {
                Console.WriteLine($"Result: {sigmaA * 100}% < {sigmaB * 100}%. Equality (a) is more precise.");
                Console.WriteLine($"- Answer: {num}/{den}");
            }
            else
            {
                Console.WriteLine($"Result: {sigmaB * 100}% < {sigmaA * 100}%. Equality (b) is more precise.");
                Console.WriteLine($"\n- Answer: √{rootVal}");
            }
        }
    }

    public class PrecisionRounder
    {
        public (double approxNum, double delta, double sigmaPercent) GetInputs()
        {
            Console.WriteLine("--- Task B: Data Input ---");
            Console.Write("Enter approximate number (x): ");
            double approxNum = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter absolute error (delta Δ, or 0 if unknown): ");
            double delta = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter relative error in % (sigma δ%, or 0 if unknown): ");
            double sigmaPercent = Convert.ToDouble(Console.ReadLine());
            return (approxNum, delta, sigmaPercent);
        }

        public void ShowConclusion(double approxNum, int m, double calculatedDelta)
        {
            double roundedNum = Math.Round(approxNum, m, MidpointRounding.AwayFromZero);
            double limitError = Math.Pow(10, -m);
            Console.WriteLine($"\nCalculated absolute error: {calculatedDelta}");
            Console.WriteLine($"Condition check: {calculatedDelta} < {limitError} (10^-{m})");
            Console.WriteLine($"Number of valid decimal places: {m}");
            Console.WriteLine($"\n- Answer: {roundedNum}");
        }
    }

    public class PrecisionEstimator
    {
        public string GetInput()
        {
            Console.WriteLine("--- Task C: Data Input ---");
            Console.Write("Enter approximate number (f): ");
            return Console.ReadLine();
        }

        public void ShowConclusion(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return;

            string normalizedInput = strInput.Replace(',', '.');
            double f;

            try
            {
                f = Convert.ToDouble(normalizedInput);
            }
            catch
            {
                Console.WriteLine("Error: incorrect form of number.");
                return;
            }

            int m = normalizedInput.Contains(".") ? normalizedInput.Split('.')[1].Length : 0;

            // Предельная абсолютная погрешность (Δ)
            // если все цифры верны, Δ = 10^-m
            double deltaF = Math.Pow(10, -m);

            // Предельная относительная погрешность (δ)
            double rawSigmaF = deltaF / Math.Abs(f);

            double power = Math.Floor(Math.Log10(rawSigmaF));
            double scale = Math.Pow(10, power);
            double firstDigit = rawSigmaF / scale;

            double sigmaF;
            if (firstDigit >= 4.5)
            {
                sigmaF = Math.Pow(10, power + 1);
            }
            else
            {
                sigmaF = Math.Round(firstDigit, 0, MidpointRounding.AwayFromZero) * scale;
            }

            Console.WriteLine($"\n--- Step 1: Identify valid digits ---");
            Console.WriteLine($"Approximate number f = {f}");
            Console.WriteLine($"Number of decimal places m = {m}");
            Console.WriteLine($"\n--- Step 2: Limit absolute error (Δf) ---");
            Console.WriteLine($"Δf = 10^-{m} = {deltaF:G10}");
            Console.WriteLine($"\n--- Step 3: Limit relative error (δf) ---");
            Console.WriteLine($"δf = Δf / |f| = {deltaF} / {Math.Abs(f)} ≈ {rawSigmaF:F6} ≈ {sigmaF * 100:G10}%");
            Console.WriteLine($"\n--- Conclusion ---");
            Console.WriteLine($"- Answer:" +
                $"\nΔf = {deltaF:G10}" +
                $"\nδf ≈ {sigmaF * 100:G10}%");
        }
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("1. Task A: Compare precision of two equalities");
                Console.WriteLine("2. Task B: Round doubtful digits");
                Console.WriteLine("3. Task C: Find errors for valid digits");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect a task (0-3): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": RunA(); break;
                    case "2": RunB(); break;
                    case "3": RunC(); break;
                    case "0": isRunning = false; break;
                }
            }
        }

        static void RunA()
        {
            Console.Clear();
            var pc = new PrecisionComparer();

            try
            {
                var (num, den, rootVal) = pc.GetInputs();
                pc.ShowDetailedComparison(num, den, rootVal);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid format entered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError:\n-------------------" +
                    $"\n{ex}" +
                    $"\n-------------------");
            }
            finally
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void RunB()
        {
            Console.Clear();
            var r = new Rounder();
            var pr = new PrecisionRounder();

            try
            {
                var (approxNum, delta, sigmaPercent) = pr.GetInputs();
                r.ShowDetailedRounding(approxNum, delta, sigmaPercent);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid format entered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError:\n-------------------" +
                    $"\n{ex}" +
                    $"\n-------------------");
            }
            finally
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void RunC()
        {
            Console.Clear();
            var pe = new PrecisionEstimator();

            try
            {
                string input = pe.GetInput();
                pe.ShowConclusion(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid format entered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError:\n-------------------" +
                    $"\n{ex}" +
                    $"\n-------------------");
            }
            finally
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }
}

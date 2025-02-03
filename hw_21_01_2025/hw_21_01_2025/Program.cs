namespace hw_21_01_2025
{

    /// <summary>
    /// Основна програма, яка виконує генерацію простих чисел і чисел Фібоначчі в окремих потоках.
    /// </summary>
    internal class Program
    {

        /// <summary>
        /// Глобальна змінна, яка керує виконанням потоків.
        /// </summary>
        private static bool running = true;

        static void Main()
        {
            int lowerBound = 2;
            int? upperBound = null;

            Console.Write("Введіть нижню межу або натисніть Enter (за замовчуванням 2): ");
            if (int.TryParse(Console.ReadLine(), out int lowerInput))
            {
                Validation(lowerInput);
                lowerBound = lowerInput;
            }

            Console.Write("Введіть верхню межу або натисніть Enter (для безкінечного генерування): ");
            if (int.TryParse(Console.ReadLine(), out int upperInput))
            {
                Validation(upperInput);
                upperBound = upperInput;
            }

            Console.WriteLine("Натисніть будь-яку клавішу для завершення додатка.");

            Thread primeThread = new Thread(() => GeneratePrimes(lowerBound, upperBound));
            Thread fibonacciThread = new Thread(GenerateFibonacci);

            primeThread.Start();
            fibonacciThread.Start();

            Console.ReadKey();
            running = false;
            primeThread.Join();
            fibonacciThread.Join();
        }

        /// <summary>
        /// Генерує числа Фібоначчі в окремому потоці доти, доки змінна <see cref="running"/> равна true.
        /// </summary>
        private static void GenerateFibonacci()
        {
            int a = 0, b = 1;

            while (running)
            {
                Console.WriteLine($"Число Фібоначчі: {a}");
                int next = a + b;
                a = b;
                b = next;
                Thread.Sleep(200);
            }

            Console.WriteLine("Генерацію чисел Фібоначчі завершено.");
        }

        /// <summary>
        /// Генерує прості числа в заданому діапазоні в окремому потоці доти, доки змінна <see cref="running"/> равна true.
        /// </summary>
        /// <param name="lowerBound">Нижня межа діапазону для генерації.</param>
        /// <param name="upperBound">Верхня межа діапазону для генерації. Якщо null, генерація буде нескінченною.</param>
        private static void GeneratePrimes(int lowerBound, int? upperBound)
        {
            Random random = new Random();

            try
            {
                while (running)
                {
                    int candidate = random.Next(lowerBound, upperBound ?? int.MaxValue);

                    if (IsPrime(candidate))
                    {
                        Console.WriteLine(candidate);
                        Thread.Sleep(100);
                    }
                }

                Console.WriteLine("Генерацію завершено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Перевіряє, чи є число простим.
        /// </summary>
        /// <param name="number">Число для перевірки.</param>
        /// <returns>Повертає true, якщо число просте; інакше false.</returns>
        private static bool IsPrime(int number)
        {
            if (number < 2) return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0) return false;

            return true;
        }

        /// <summary>
        /// Перевіряє вхідне число на коректність.
        /// </summary>
        /// <param name="number">Число для перевірки.</param>
        /// <exception cref="ArgumentException">Викидається, якщо число від'ємне.</exception>
        private static void Validation(int number)
        {
            if (number < 0)
                throw new ArgumentException("Число не може бути від'ємним.");
        }
    }
}
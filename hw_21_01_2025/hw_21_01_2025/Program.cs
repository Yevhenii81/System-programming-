namespace hw_21_01_2025
{

    /// <summary>
    /// Основная программа, выполняющая генерацию простых чисел и чисел Фибоначчи в отдельных потоках.
    /// </summary>
    internal class Program
    {

        /// <summary>
        /// Глобальная переменная, управляющая выполнением потоков.
        /// </summary>
        private static bool running = true;

        static void Main()
        {
            int lowerBound = 2;
            int? upperBound = null;

            Console.Write("Введите нижнюю границу или нажмите Enter (по умолчанию 2): ");
            if (int.TryParse(Console.ReadLine(), out int lowerInput))
            {
                Validation(lowerInput);
                lowerBound = lowerInput;
            }

            Console.Write("Введите верхнюю границу или нажмите Enter (для бесконечной генерации): ");
            if (int.TryParse(Console.ReadLine(), out int upperInput))
            {
                Validation(upperInput);
                upperBound = upperInput;
            }

            Console.WriteLine("Нажмите любую клавишу для завершения приложения.");

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
        /// Генерирует числа Фибоначчи в отдельном потоке, пока переменная <see cref="running"/> равна true.
        /// </summary>
        private static void GenerateFibonacci()
        {
            int a = 0, b = 1;

            while (running)
            {
                Console.WriteLine($"Число Фибоначчи: {a}");
                int next = a + b;
                a = b;
                b = next;
                Thread.Sleep(200);
            }

            Console.WriteLine("Генерация чисел Фибоначчи завершена.");
        }

        /// <summary>
        /// Генерирует простые числа в заданном диапазоне в отдельном потоке, пока переменная <see cref="running"/> равна true.
        /// </summary>
        /// <param name="lowerBound">Нижняя граница диапазона генерации.</param>
        /// <param name="upperBound">Верхняя граница диапазона генерации. Если null, генерация будет бесконечной.</param>
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

                Console.WriteLine("Генерация завершена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Проверяет, является ли число простым.
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>Возвращает true, если число простое; иначе false.</returns>
        private static bool IsPrime(int number)
        {
            if (number < 2) return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0) return false;

            return true;
        }

        /// <summary>
        /// Проверяет входное число на корректность.
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если число отрицательное.</exception>
        private static void Validation(int number)
        {
            if (number < 0)
                throw new ArgumentException("Число не может быть отрицательным");
        }
    }
}

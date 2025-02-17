namespace hw_28_01_2025
{
    class Program
    {
        static Mutex mutex1 = new Mutex();
        static Mutex mutex2 = new Mutex();

        static void Main()
        {
            Thread thread1 = new Thread(GenerateNumbers);
            Thread thread2 = new Thread(FilterPrimes);
            Thread thread3 = new Thread(FilterPrimesEndingWith7);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            Console.WriteLine("Обработка завершена.");
        }

        static void GenerateNumbers()
        {
            mutex1.WaitOne();
            Random random = new Random();
            using (StreamWriter writer = new StreamWriter("itstep.txt"))
            {
                for (int i = 0; i < 100; i++)
                {
                    writer.WriteLine(random.Next(1, 1000));
                }
            }
            Console.WriteLine("Сгенерированы числа в файле itstep.txt");
            mutex1.ReleaseMutex();
        }

        static void FilterPrimes()
        {
            mutex1.WaitOne();
            mutex2.WaitOne();
            if (File.Exists("itstep.txt"))
            {
                var primes = File.ReadAllLines("itstep.txt")
                    .Select(int.Parse)
                    .Where(IsPrime)
                    .ToList();

                File.WriteAllLines("itstep.txt", primes.Select(n => n.ToString()));
                Console.WriteLine("Простые числа записаны в itstep.txt");
            }
            mutex1.ReleaseMutex();
            mutex2.ReleaseMutex();
        }

        static void FilterPrimesEndingWith7()
        {
            mutex2.WaitOne();
            if (File.Exists("itstep.txt"))
            {
                var primes7 = File.ReadAllLines("itstep.txt")
                    .Select(int.Parse)
                    .Where(n => n % 10 == 7)
                    .ToList();

                File.WriteAllLines("itstep_26.txt", primes7.Select(n => n.ToString()));
                Console.WriteLine("Простые числа, оканчивающиеся на 7, записаны в itstep_26.txt");
            }
            mutex2.ReleaseMutex();
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}

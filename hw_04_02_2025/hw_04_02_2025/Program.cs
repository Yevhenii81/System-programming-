namespace hw_04_02_2025
{
    internal class Program
    {
        private static readonly object lockObject = new object();
        private static Random random = new Random();
        private static int peopleAtStop = 0;
        private const int maxBusCapacity = 30;
        private const int totalBuses = 10;
        private static AutoResetEvent busArrived = new AutoResetEvent(false);

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        static void Main()
        {
            try
            {
                Thread passengerThread = new Thread(GeneratePassengers);
                Thread busThread = new Thread(OperateBuses);

                passengerThread.Start();
                busThread.Start();

                passengerThread.Join();
                busThread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в главном потоке: {ex.Message}");
            }
        }

        /// <summary>
        /// Генерирует пассажиров, которые прибывают на остановку.
        /// </summary>
        private static void GeneratePassengers()
        {
            try
            {
                for (int i = 0; i < totalBuses; i++)
                {
                    lock (lockObject)
                    {
                        int newPassengers = random.Next(5, 20);
                        peopleAtStop += newPassengers;
                        Console.WriteLine($"Прибыло {newPassengers} новых пассажиров. Всего на остановке: {peopleAtStop}");
                    }
                    Thread.Sleep(random.Next(1000, 3000));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при генерации пассажиров: {ex.Message}");
            }
        }

        /// <summary>
        /// Обрабатывает прибытие автобусов и посадку пассажиров.
        /// </summary>
        private static void OperateBuses()
        {
            try
            {
                for (int i = 1; i <= totalBuses; i++)
                {
                    Thread.Sleep(random.Next(2000, 5000));
                    lock (lockObject)
                    {
                        int boardingPassengers = Math.Min(peopleAtStop, maxBusCapacity);
                        peopleAtStop -= boardingPassengers;
                        Console.WriteLine($"Автобус №175 приехал. Взято пассажиров: {boardingPassengers}. Осталось на остановке: {peopleAtStop}");
                    }
                    busArrived.Set();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в работе автобусов: {ex.Message}");
            }
        }
    }
}

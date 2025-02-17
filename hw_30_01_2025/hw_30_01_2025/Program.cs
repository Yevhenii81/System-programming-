namespace hw_30_01_2025
{

    /// <summary>
    /// Класс для имитации работы автобусной конечной остановки.
    /// </summary>
    internal class Program
    {
        static readonly object locker = new object();
        static int peopleAtStop = 0;
        static readonly Random random = new Random();
        static readonly int busCapacity = 30;
        static readonly int totalBuses = 10;
        static AutoResetEvent busArrived = new AutoResetEvent(false);

        /// <summary>
        /// Точка входа в программу.
        /// Запускает генерацию пассажиров и прибытие автобусов.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                Thread generatorThread = new Thread(GeneratePassengers);
                generatorThread.Start();

                for (int i = 0; i < totalBuses; i++)
                {
                    Thread busThread = new Thread(BusArrives);
                    busThread.Start(i + 1);
                    Thread.Sleep(random.Next(1000, 5000));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в выполнении программы: {ex.Message}");
            }
        }

        /// <summary>
        /// Генерирует случайное количество пассажиров на остановке.
        /// </summary>
        static void GeneratePassengers()
        {
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    int newPeople = random.Next(5, 15);
                    lock (locker)
                    {
                        peopleAtStop += newPeople;
                        Console.WriteLine($"Прибыло {newPeople} человек. Всего на остановке: {peopleAtStop}");
                    }
                    Thread.Sleep(random.Next(2000, 4000));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при генерации пассажиров: {ex.Message}");
            }
        }

        /// <summary>
        /// Обрабатывает прибытие автобуса и посадку пассажиров.
        /// </summary>
        /// <param name="busNumber">Номер автобуса.</param>
        static void BusArrives(object busNumber)
        {
            try
            {
                lock (locker)
                {
                    Console.WriteLine($"Автобус №{busNumber} прибыл. Людей на остановке: {peopleAtStop}");
                    int passengersToBoard = Math.Min(peopleAtStop, busCapacity);
                    peopleAtStop -= passengersToBoard;
                    Console.WriteLine($"Автобус №{busNumber} забрал {passengersToBoard} пассажиров. Осталось на остановке: {peopleAtStop}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в работе автобуса №{busNumber}: {ex.Message}");
            }
        }
    }
}

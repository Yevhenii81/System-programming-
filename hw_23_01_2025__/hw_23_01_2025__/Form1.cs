namespace hw_23_01_2025__
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Список для зберігання прогрес-барів, які представляють коней у гонці.
        /// </summary>
        private List<ProgressBar> progressBars = new List<ProgressBar>();

        /// <summary>
        /// Кількість коней, які беруть участь у гонці.
        /// </summary>
        private const int NumberOfHorses = 5;

        /// <summary>
        /// Генератор випадкових чисел для моделювання швидкості коней.
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Список потоків для кожного коня.
        /// </summary>
        private List<Thread> horseThreads = new List<Thread>();

        /// <summary>
        /// Конструктор форми. Додає прогрес-бари та ініціалізує компоненти форми.
        /// </summary>
        public Form1()
        {
            AddProgressBar();
            InitializeComponent();
        }

        /// <summary>
        /// Додає прогрес-бари для кожного коня до форми.
        /// </summary>
        private void AddProgressBar()
        {
            for (int i = 0; i < NumberOfHorses; i++)
            {
                var progressBar = new ProgressBar
                {
                    Name = $"Horse{i + 1}",
                    Minimum = 0,
                    Maximum = 100,
                    Value = 0,
                    Location = new Point(50, 30 + i * 50),
                    Size = new Size(400, 30)
                };
                progressBars.Add(progressBar);
                this.Controls.Add(progressBar);
            }
        }

        /// <summary>
        /// Обробник події для кнопки старту. Запускає гонку.
        /// </summary>
        /// <param name="sender">Джерело події.</param>
        /// <param name="e">Дані події.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            resultLabel.Text = "Гонка триває...";
            horseThreads.Clear();

            List<(int horse, int time)> results = new List<(int horse, int time)>();
            object lockObject = new object();

            for (int i = 0; i < NumberOfHorses; i++)
            {
                int horseIndex = i;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        int timeTaken = RunRace(horseIndex);
                        lock (lockObject)
                        {
                            results.Add((horseIndex + 1, timeTaken));
                            if (results.Count == NumberOfHorses)
                            {
                                Invoke(new Action(() => DisplayResults(results)));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => MessageBox.Show($"Помилка: {ex.Message}")));
                    }
                });
                thread.IsBackground = true;
                horseThreads.Add(thread);
                thread.Start();
            }
        }

        /// <summary>
        /// Моделює гонку для одного коня.
        /// </summary>
        /// <param name="horseIndex">Індекс коня у списку.</param>
        /// <returns>Час, за який кінь фінішував.</returns>
        private int RunRace(int horseIndex)
        {
            int progress = 0;
            int totalTime = 0;

            while (progress < 100)
            {
                int speed = random.Next(1, 5);
                progress += speed;
                totalTime++;

                Invoke(new Action(() =>
                {
                    if (progressBars[horseIndex].IsDisposed) return;
                    progressBars[horseIndex].Value = Math.Min(progress, 100);
                }));

                Thread.Sleep(50);
            }

            return totalTime;
        }

        /// <summary>
        /// Відображає результати гонки після її завершення.
        /// </summary>
        /// <param name="results">Список результатів кожного коня.</param>
        private void DisplayResults(List<(int horse, int time)> results)
        {
            results.Sort((x, y) => x.time.CompareTo(y.time));

            string resultText = "Результати:\n";
            for (int i = 0; i < results.Count; i++)
            {
                resultText += $"{i + 1}. Кінь {results[i].horse} - {results[i].time} одиниць\n";
            }

            resultLabel.Text = resultText;
            startButton.Enabled = true;
        }
    }
}

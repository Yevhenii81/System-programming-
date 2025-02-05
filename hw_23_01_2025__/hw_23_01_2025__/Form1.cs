namespace hw_23_01_2025__
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// ������ ��� ��������� �������-����, �� ������������� ����� � �����.
        /// </summary>
        private List<ProgressBar> progressBars = new List<ProgressBar>();

        /// <summary>
        /// ʳ������ �����, �� ������ ������ � �����.
        /// </summary>
        private const int NumberOfHorses = 5;

        /// <summary>
        /// ��������� ���������� ����� ��� ����������� �������� �����.
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// ������ ������ ��� ������� ����.
        /// </summary>
        private List<Thread> horseThreads = new List<Thread>();

        /// <summary>
        /// ����������� �����. ���� �������-���� �� �������� ���������� �����.
        /// </summary>
        public Form1()
        {
            AddProgressBar();
            InitializeComponent();
        }

        /// <summary>
        /// ���� �������-���� ��� ������� ���� �� �����.
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
        /// �������� ��䳿 ��� ������ ������. ������� �����.
        /// </summary>
        /// <param name="sender">������� ��䳿.</param>
        /// <param name="e">��� ��䳿.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            resultLabel.Text = "����� �����...";
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
                        Invoke(new Action(() => MessageBox.Show($"�������: {ex.Message}")));
                    }
                });
                thread.IsBackground = true;
                horseThreads.Add(thread);
                thread.Start();
            }
        }

        /// <summary>
        /// ������� ����� ��� ������ ����.
        /// </summary>
        /// <param name="horseIndex">������ ���� � ������.</param>
        /// <returns>���, �� ���� ��� ��������.</returns>
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
        /// ³������� ���������� ����� ���� �� ����������.
        /// </summary>
        /// <param name="results">������ ���������� ������� ����.</param>
        private void DisplayResults(List<(int horse, int time)> results)
        {
            results.Sort((x, y) => x.time.CompareTo(y.time));

            string resultText = "����������:\n";
            for (int i = 0; i < results.Count; i++)
            {
                resultText += $"{i + 1}. ʳ�� {results[i].horse} - {results[i].time} �������\n";
            }

            resultLabel.Text = resultText;
            startButton.Enabled = true;
        }
    }
}

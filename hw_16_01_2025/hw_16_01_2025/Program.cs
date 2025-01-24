using System.Diagnostics;

namespace hw_16_01_2025
{
    public class Program
    {
        static void Main(string[] args)
        {
            Process process = null;

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = @"C:\Users\USER\source\repos\hw_16_01_2025\ChildProcess\bin\Debug\net8.0\ChildProcess.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            try
            {
                process = Process.Start(processStartInfo);
                Console.WriteLine("The child process has been launched");
                Console.WriteLine("ID: " + process.Id);
                Console.WriteLine("Start Time: " + process.StartTime);

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                Console.WriteLine("Result: " + output);
                Console.WriteLine("Exit Code: " + process.ExitCode);
                Console.WriteLine("Execution Time: " + process.TotalProcessorTime);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error: " + error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (process != null)
                {
                    process.Dispose();
                }
            }
        }
    }
}

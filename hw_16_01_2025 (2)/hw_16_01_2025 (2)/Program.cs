using System;
using System.Diagnostics;

namespace hw_16_01_2025__2_
{
    public class Program
    {
        static void Main(string[] args)
        {
            Process process = null;

            try
            {
                ProcessStartInfo processStartInfo = CreateProcessStartInfo();
                process = StartProcess(processStartInfo);
                DisplayProcessInfo(process);

                string choice = GetUserChoice();

                HandleUserChoice(choice, process);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                DisposeProcess(process);
            }
        }

        private static ProcessStartInfo CreateProcessStartInfo()
        {
            return new ProcessStartInfo()
            {
                FileName = @"C:\Users\USER\source\repos\hw_16_01_2025\ChildProcess\bin\Debug\net8.0\ChildProcess.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
        }

        private static Process StartProcess(ProcessStartInfo processStartInfo)
        {
            var process = Process.Start(processStartInfo);
            if (process == null)
                throw new InvalidOperationException("Failed to start the process");
            return process;
        }

        private static void DisplayProcessInfo(Process process)
        {
            Console.WriteLine("The child process has been launched");
            Console.WriteLine("ID: " + process.Id);
            Console.WriteLine("Start Time: " + process.StartTime);
        }

        private static string GetUserChoice()
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Wait for the child process to finish");
            Console.WriteLine("2. Terminate the child process");
            Console.Write("Enter your choice (1 or 2): ");
            return Console.ReadLine();
        }

        private static void HandleUserChoice(string choice, Process process)
        {
            switch (choice)
            {
                case "1":
                    WaitForProcessToFinish(process);
                    break;

                case "2":
                    TerminateProcess(process);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Exiting program");
                    break;
            }
        }

        private static void WaitForProcessToFinish(Process process)
        {
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

        private static void TerminateProcess(Process process)
        {
            Console.WriteLine("Terminating the child process...");
            if (!process.HasExited)
            {
                process.Kill();
                process.WaitForExit();
                Console.WriteLine("Child process terminated");
            }
            else
            {
                Console.WriteLine("The child process has already exited");
            }
        }

        private static void DisposeProcess(Process process)
        {
            process?.Dispose();
        }
    }
}
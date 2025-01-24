using System.Diagnostics;

namespace ChildProcess
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The child process works.");
            Thread.Sleep(2000);
            Console.WriteLine("Finish!");
            //throw new Exception();
        }
    }
}
using System;
using System.Runtime.InteropServices;

namespace hw_14_01_2025
{
    internal class Program
    {
        private const int MB_OK = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, int type);

        private static void ShowMessage(string text, string caption)
        {
            int result = MessageBox(IntPtr.Zero, text, caption, MB_OK);
            if (result == 0)
            {
                Console.WriteLine($"Error displaying message box: {Marshal.GetLastWin32Error()}");
            }
        }

        static void Main(string[] args)
        {
            ShowMessage("Name: Yevhenii", "Name");
            ShowMessage("Lastname: Shostak", "Lastname");
            ShowMessage("Date of birth: 11.08.05", "Birthday");
            ShowMessage("Education: \r\nOdesa Branch of IT STEP", "Education");
            ShowMessage("Specialty: \r\nSoftware Development", "Specialty");
            ShowMessage("Start and end date: \r\nNov 22 - May 2026 (Expected)", "Date");
        }
    }
}

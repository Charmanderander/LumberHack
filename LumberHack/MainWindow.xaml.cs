using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LumberHack
{
    public partial class MainWindow : Window
    {
        #region Imports
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(int hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const Int32 WM_VSCROLL = 0x0115;
        private const Int32 SB_PAGEDOWN = 0x1;
        #endregion

        private Process ieProcess;

        public MainWindow()
        {
            InitializeComponent();
            GetProcess();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Left(object sender, RoutedEventArgs e)
        {
            SendKey("LEFT");
        }

        private void Button_Right(object sender, RoutedEventArgs e)
        {
            SendKey("RIGHT");
        }

        private void Button_Space(object sender, RoutedEventArgs e)
        {
            SendKey("SPACE");
        }

        private void SendCommand(string command)
        {
            GetProcess();
            Console.WriteLine(command);
            SetForegroundWindow((int)ieProcess.MainWindowHandle);
            SendKeys.SendWait(command);
            SendKeys.Flush();
        }

        private void SendKey(string command)
        {
            switch (command)
            {
                case "LEFT":
                    SendCommand("{LEFT}");
                    break;

                case "RIGHT":
                    SendCommand("{RIGHT}");
                    break;

                case "SPACE":
                    SendCommand(" ");
                    break;

                default:
                    break;
            }
        }

        private void GetProcess()
        {
            Process[] ie = Process.GetProcessesByName("iexplore");
            if (ie.Length == 0) return;

            ieProcess = ie[0];

            if (ieProcess != null)
            {
                Console.WriteLine("IE found!");
            }
            else
            {
                Console.WriteLine("IE not found!");
                return;
            }
        }

    }
}

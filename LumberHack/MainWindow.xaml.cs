using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using AForge.Imaging.Filters;
using AForge.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Size = System.Drawing.Size;
using System.Threading.Tasks;

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

        #endregion

        private Process ieProcess;

        private Bitmap noBranchRightBmp;
        private Bitmap noBranchLeftBmp;

        private Bitmap leftBranchAboveBmp;
        private Bitmap rightBranchAboveBmp;

        private Bitmap leftBranchDirectAboveBmp;
        private Bitmap rightBranchDirectAboveBmp;
               
        private Bitmap leftBranchBmp;
        private Bitmap rightBranchBmp;

        
        public MainWindow()
        {
            InitializeComponent();

            Bootstrap();
        }

        // Does the initial dirty work
        private void Bootstrap()
        {
            this.noBranchRightBmp = (Bitmap)Bitmap.FromFile("nobranchright.jpg");
            this.noBranchLeftBmp = (Bitmap)Bitmap.FromFile("nobranchleft.jpg");

            this.leftBranchAboveBmp = (Bitmap)Bitmap.FromFile("leftbranchabove.jpg");
            this.rightBranchAboveBmp = (Bitmap)Bitmap.FromFile("rightbranchabove.jpg");

            this.leftBranchDirectAboveBmp = (Bitmap)Bitmap.FromFile("leftbranchdirectabove.jpg");
            this.rightBranchDirectAboveBmp = (Bitmap)Bitmap.FromFile("rightbranchdirectabove.jpg");

            this.leftBranchBmp = (Bitmap)Bitmap.FromFile("leftbranch.jpg");
            this.rightBranchBmp = (Bitmap)Bitmap.FromFile("rightbranch.jpg");

            myTextBlock.Background = Brushes.LightYellow;

            myTextBlock.Text += "Press start to find the LumberJack!\n";

            // Get the process of the browser
            GetProcess();
        }

        // When the start button is pressed
        private async void Button_Space(object sender, RoutedEventArgs e)
        {
            int[] coordinates = new int[2];

            UpdateTextBlock("Finding LumberJack...\n");
            
            await Task.Run(() => FindStartingBranch(ref coordinates));

            UpdateTextBlock("Found Lumberjack! Dont move your window!\n");

            ChoppingLogic(coordinates[0], coordinates[1]);

            UpdateTextBlock("Game Over!");
        }


        private void UpdateTextBlock(string text)
        {
            myTextBlock.Text += text;
        }

        private void ChoppingLogic(int x, int y)
        {
            float[] similarity = new float[8];
            int maxIndex;
            float maxValue;
            int step = 1;

            var bmpScreen = new Bitmap(280, 160, PixelFormat.Format16bppGrayScale);

            do
            {
                bmpScreen = GetCurrentScreen(x, y);
                //bmpScreen.Save("ss"+step+".jpg", ImageFormat.Jpeg);

                similarity[0] = GetSimilarity(bmpScreen, noBranchRightBmp);
                similarity[1] = GetSimilarity(bmpScreen, noBranchLeftBmp);

                similarity[2] = GetSimilarity(bmpScreen, leftBranchAboveBmp);
                similarity[3] = GetSimilarity(bmpScreen, rightBranchAboveBmp);

                similarity[4] = GetSimilarity(bmpScreen, leftBranchDirectAboveBmp);
                similarity[5] = GetSimilarity(bmpScreen, rightBranchDirectAboveBmp);

                similarity[6] = GetSimilarity(bmpScreen, leftBranchBmp);
                similarity[7] = GetSimilarity(bmpScreen, rightBranchBmp);

                maxValue = similarity.Max();
                maxIndex = Array.IndexOf(similarity, maxValue);

                switch (maxIndex)
                {
                    // no branch right
                    case 0:
                        Console.WriteLine("nobranchright");
                        SendCommand("{LEFT}");
                        break;

                    // no branch left
                    case 1:
                        Console.WriteLine("nobranchleft");
                        SendCommand("{RIGHT}");
                        break;

                    // left branch above
                    case 2:
                        Console.WriteLine("leftbranchabove");
                        SendCommand("{RIGHT}");
                        break;

                    // right branch above
                    case 3:
                        Console.WriteLine("rightbranchabove");
                        SendCommand("{LEFT}");
                        break;

                    // left branch direct above
                    case 4:
                        Console.WriteLine("leftbranchdirectabove");
                        SendCommand("{RIGHT}");
                        break;

                    // right branch direct above
                    case 5:
                        Console.WriteLine("rightbranchdirectabove");
                        SendCommand("{LEFT}");
                        break;

                    // left branch
                    case 6:
                        Console.WriteLine("leftbranch");
                        SendCommand("{RIGHT}");
                        break;

                    // right branch
                    case 7:
                        Console.WriteLine("rightbranch");
                        SendCommand("{LEFT}");
                        break;

                    default:
                        break;
                }
                step++;
                Thread.Sleep(145);
                
            } while (similarity.Max() > 0.90 );
        }

        // Sends command to the browser
        private void SendCommand(string command)
        {
            GetProcess();
            Console.WriteLine(command);
            SetForegroundWindow((int)ieProcess.MainWindowHandle);
            SendKeys.SendWait(command);
            SendKeys.Flush();
        }

        // Get the process of the browser so we can send comamnds to it
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

        // This function scans the entire screen of 600x100 to find the game screen coordinates
        private void FindStartingBranch(ref int[] coordinates)
        {
            
            Console.WriteLine("Finding starting Branch...");
            var totalWidth = Screen.PrimaryScreen.Bounds.Width;
            var totalHeight = Screen.PrimaryScreen.Bounds.Height;

            var bmpScreen = new Bitmap(280, 160, PixelFormat.Format24bppRgb);

            float nobranchrightSim;
            float nobranchleftSim;

            int i=0;
            int j=0;

            for (i = 0; i < totalHeight; i+=15)
            {
                for (j = 0; j < totalWidth; j+=15)
                {
                    bmpScreen = GetCurrentScreen(i, j);

                    nobranchrightSim = GetSimilarity(bmpScreen, noBranchRightBmp);
                    nobranchleftSim = GetSimilarity(bmpScreen, noBranchLeftBmp);
                    
                    if (nobranchrightSim > 0.98 || nobranchleftSim > 0.98)
                    {
                        
                        Console.WriteLine("{0} x {1}", i, j);
                        Console.WriteLine("No Branch similarity: {0}", nobranchrightSim);
                        Console.WriteLine("--- Next Image ---");
                        goto Found;
                    }

                }
            }

            
        Found:
            Console.WriteLine("using coordinates {0} x {1}", i, j);
            coordinates[0] = i;
            coordinates[1] = j;
        }

        // Takes a screenshot, and returns the bitmap
        private Bitmap GetCurrentScreen(int x, int y)
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(280, 160, PixelFormat.Format24bppRgb);
            Size s = new Size(bmpScreenshot.Width, bmpScreenshot.Height);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            
            gfxScreenshot.CopyFromScreen(x, y, 0, 0, s);

            
            //bmpScreenshot.Save("ss"+x+""+y+".jpg", ImageFormat.Jpeg);

            return bmpScreenshot;
        }

        // Calculates similarity between two images
        private float GetSimilarity(Bitmap currentBranch, Bitmap targetBranch)
        {
            float score;

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);
            
            // Compare two images
            TemplateMatch[] matchings = tm.ProcessImage(currentBranch, targetBranch);

            score = matchings[0].Similarity;

            return score;
        }

    }
}

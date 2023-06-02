using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace TwitchFlashbang
{
    public partial class Flashbang : Form
    {
        #region Imports
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_TRANSPARENT = 0x20;
        #endregion

        private double flashDuration = 1.88;
        private double fadeDuration = 2.99;
        private double opacityDecrementPerIteration;
        private double updateRate = 255;
        private int numIterations;
        private bool forceAbort = false;
        private bool isBlinding = false;
        private bool isFading = false;
        private bool testMode = false;

        ConcurrentQueue<FlashbangData> flashbangs = new ConcurrentQueue<FlashbangData>();
        SoundPlayer player = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flashbang.wav"));
        Graphics g;
        Bitmap screenshot;
        Rectangle captureRectangle = Screen.PrimaryScreen.Bounds;

        TwitchBot twitchBot = new TwitchBot();

        private DebugWindow dbgWindow;
        private Keys flashKey = Keys.F;

        public bool CanThreadsRun = true;
        private double initialOpacity = 1.0;
        private double finalOpacity = 0.0;

        private bool enableAfterimage = false;
        private Dictionary<string, dynamic> perAppOpacity = new();

        public Flashbang()
        {
            InitializeComponent();

            notifyIcon1.ContextMenuStrip = new()
            {
                Items =
                {
                    new ToolStripMenuItem("Exit", null, Exit)
                }
            };

            notifyIcon1.Text = "Twitch Flashbang";

            testMode = Convert.ToBoolean(AppConfig.Configuration["testMode"]);
            enableAfterimage = Convert.ToBoolean(AppConfig.Configuration["enableAfterimage"]);

            try
            {
                var perAppOpacitySection = AppConfig.Configuration.GetSection("perAppOpacity");
                var perAppOpacityValues = perAppOpacitySection.GetChildren();

                foreach (var oValue in perAppOpacityValues)
                {
                    perAppOpacity.Add(oValue.Key.ToLower(), Convert.ToDouble(oValue.Value));
                }
            }
            catch (NullReferenceException) { }

            Task.Run(twitchBot.MainAsync);

            twitchBot.OnFlashbangData += TwitchBot_OnFlashbangData;

            Debug.WriteLine($"Handle: {Handle:x}");

            FormBorderStyle = FormBorderStyle.None;
            if (testMode)
            {
                Size = new Size(500, 500);
                Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - Size.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - Size.Height / 2);

                dbgWindow = new();
                dbgWindow.Show();
            }
            else
            {
                Size = new(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Location = Screen.PrimaryScreen.WorkingArea.Location;
            }

            screenshot = new(Screen.PrimaryScreen?.Bounds.Width ?? 0, Screen.PrimaryScreen?.Bounds.Height ?? 0);
            g = Graphics.FromImage(screenshot);

            numIterations = (int)(fadeDuration * updateRate);

            TransparencyKey = BackColor;

            int exStyle = GetWindowLong(Handle, GWL_EXSTYLE);

            TopMost = true;
            SetWindowLong(Handle, GWL_EXSTYLE, exStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
            SetLayeredWindowAttributes(Handle, 0, (byte)0, 0x2);
            UpdateWindow(Handle);

            if (testMode)
                new Thread(t_GetAsyncKeyState).Start();

            new Thread(t_ProcessFlashbangs).Start();
        }

        private void Exit(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TwitchBot_OnFlashbangData(FlashbangData obj)
        {
            flashbangs.Enqueue(obj);
        }

        private void t_ProcessFlashbangs()
        {
            while (CanThreadsRun)
            {
                if (testMode)
                    Invoke(() => dbgWindow.SetQueueCounter(flashbangs.Count));

                if (!isBlinding && !flashbangs.IsEmpty)
                {
                    FlashbangData fd;
                    if (flashbangs.TryDequeue(out fd))
                    {
                        forceAbort = true;

                        new Thread(() => Flash(fd)).Start();
                    }
                }
                else
                    forceAbort = false;

                if (forceAbort)
                    Thread.Sleep(RandomNumberGenerator.GetInt32(1300, 1900));
                else
                    Thread.Sleep(10);
            }
        }

        private void t_GetAsyncKeyState()
        {
            bool wasKeyPressed = false;

            while (CanThreadsRun)
            {
                if ((GetAsyncKeyState(flashKey) & 0x8000) != 0 && !wasKeyPressed)
                {
                    QueueTestFlash();
                    wasKeyPressed = true;
                }
                else if ((GetAsyncKeyState(flashKey) & 0x8000) == 0)
                {
                    wasKeyPressed = false;
                }

                Thread.Sleep(10);
            }
        }

        public void QueueTestFlash()
        {
            FlashbangData fd = new FlashbangData(GenerateID(6), GenerateID(6));
            Debug.WriteLine($"{fd.ID} enqueueing..");
            flashbangs.Enqueue(fd);
        }

        private void Flash(FlashbangData fd)
        {
            if (!isFading && enableAfterimage)
            {
                TakeScreenshot();
                Debug.WriteLine($"Taking screenshot, isFading: {isFading}");
            }

            isBlinding = true;
            pictureBox1.Image = null;

            int delayMilliseconds = (int)Math.Round(1000 / updateRate);

            Debug.WriteLine($"[{fd.ID}] starting");

            initialOpacity = 1;
            string? foregroundWnd = Path.GetFileNameWithoutExtension(GetForegroundWindowExecutableName())?.ToLower();
            if (foregroundWnd != null)
            {
                Debug.WriteLine($"foreground wnd: {foregroundWnd}");
                foreach (var app in perAppOpacity)
                {
                    if (foregroundWnd == Path.GetFileNameWithoutExtension(app.Key))
                    {
                        initialOpacity = app.Value;
                        break;
                    }
                }
            }
            opacityDecrementPerIteration = (initialOpacity - finalOpacity) / numIterations;
            double _Opacity = initialOpacity * 255;

            if (!testMode)
                player.Play();

            DateTime dtStart = DateTime.Now;
            fd.flashingStopwatch.Start();

            BeginInvoke(() =>
            {
                SetLayeredWindowAttributes(Handle, 0, (byte)_Opacity, 0x2);
                UpdateWindow(Handle);
            });

            while (fd.flashingStopwatch.ElapsedMilliseconds < flashDuration * 1000) { Thread.Sleep(1); }
            Debug.WriteLine($"[{fd.ID}] flash: {fd.flashingStopwatch.ElapsedMilliseconds}ms");
            isBlinding = false;

            /// ******
            /// Fading
            /// ******

            isFading = true;
            fd.fadingStopwatch.Start();

            Debug.WriteLine($"[{fd.ID}] fading from {initialOpacity} to {finalOpacity} with step={opacityDecrementPerIteration}");

            for (int i = 0; i < numIterations && _Opacity > 0; i++)
            {
                _Opacity -= opacityDecrementPerIteration * 255;

                if (pictureBox1.Image == null && enableAfterimage)
                    pictureBox1.Image = GenerateOverlay(screenshot);

                BeginInvoke(() =>
                {
                    SetLayeredWindowAttributes(Handle, 0, (byte)_Opacity, 0x2);
                    UpdateWindow(Handle);
                });

                while ((fd.fadingStopwatch.Elapsed.TotalSeconds < (i + 1) * (1.0 / updateRate)) && !forceAbort)
                    Thread.Sleep(1);

                if (forceAbort)
                {
                    DateTime aborted_dtStop = DateTime.Now;
                    TimeSpan aborted_timeSpan = dtStart - aborted_dtStop;

                    Debug.WriteLine($"[{fd.ID}] aborting at {Math.Abs(aborted_timeSpan.TotalMilliseconds)}ms");

                    pictureBox1.Image = null;

                    //forceAbort = false;
                    return;
                }
            }

            DateTime dtStop = DateTime.Now;
            TimeSpan timeSpan = dtStart - dtStop;

            Debug.WriteLine($"[#{fd.ID}] done in {Math.Abs(timeSpan.TotalMilliseconds)}ms");
            fd.fadingStopwatch.Stop();
            isFading = false;

            fd.Dispose();
        }

        public static string? GetForegroundWindowExecutableName()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
            {
                return null;
            }

            StringBuilder titleBuilder = new StringBuilder(256);
            GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);

            GetWindowThreadProcessId(hWnd, out uint processId);
            Process process = Process.GetProcessById((int)processId);

            string? executableName = process?.MainModule?.FileName;
            if (string.IsNullOrEmpty(executableName))
            {
                return null;
            }

            return executableName;
        }

        private void TakeScreenshot()
        {
            g.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
        }

        private Bitmap GenerateOverlay(Bitmap bmp)
        {
            // Create a new bitmap with the same dimensions as the original
            Bitmap resultBitmap = new(bmp.Width, bmp.Height);

            // Create a graphics object from the result bitmap
            using (Graphics graphics = Graphics.FromImage(resultBitmap))
            {
                // Create an image attributes object for color transformation
                using (ImageAttributes imageAttributes = new ImageAttributes())
                {
                    // Create a color matrix for the color transformation
                    ColorMatrix colorMatrix = new ColorMatrix(
                        new float[][]
                        {
                            new float[] { 2f, 0f, 0f, 0f, 0f },             // Red component
                            new float[] { 0f, 2f, 0f, 0f, 0f },             // Green component
                            new float[] { 0f, 0f, 2f, 0f, 0f },             // Blue component
                            new float[] { 0f, 0f, 0f, 0.2f, 0f },           // Alpha component
                            new float[] { -0.1f, -0.1f, -0.1f, 0f, 1f }     // Brightness adjustment
                        });

                    // Set the color matrix to the image attributes
                    imageAttributes.SetColorMatrix(colorMatrix);

                    // Draw the original bitmap onto the result bitmap with the color transformation
                    graphics.DrawImage(bmp, new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }

            return resultBitmap;
        }

        private static string GenerateID(int length)
        {
            byte[] buffer = new byte[length / 2];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return BitConverter.ToString(buffer).Replace("-", "").Substring(0, length).ToLower();
        }

        private void Flashbang_FormClosed(object sender, FormClosedEventArgs e)
        {
            CanThreadsRun = false;
            Application.Exit();
        }
    }
}

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Media;
using System.Security.Cryptography;
using System.Text;

namespace TwitchFlashbang
{
    public partial class Flashbang : Form
    {
        private readonly double flashDuration = 1.88;
        private readonly double fadeDuration = 2.99;
        private double opacityDecrementPerIteration;
        private readonly double updateRate = 255;
        private readonly int numIterations;
        private bool forceAbort = false;
        private bool isBlinding = false;
        private bool isFading = false;
        private readonly bool testMode = false;
        private readonly ConcurrentQueue<FlashbangData> flashbangs = new();
        private readonly SoundPlayer player = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flashbang.wav"));
        private readonly Graphics g;
        private readonly Bitmap screenshot;
        private Rectangle? captureRectangle = Screen.PrimaryScreen?.Bounds;
        private readonly Afterimage afterimage;
        private readonly TwitchBot twitchBot = new();

        private readonly Keys flashKey;

        public bool CanThreadsRun = true;
        private double initialOpacity = 1.0;
        private readonly double finalOpacity = 0.0;

        private readonly bool enableAfterimage = false;
        private readonly Dictionary<string, dynamic> perAppOpacity = new();

        public Flashbang()
        {
            InitializeComponent();

            notifyIcon1.ContextMenuStrip = new()
            {
                Items = { new ToolStripMenuItem("Exit", null, Exit) }
            };

            notifyIcon1.Text = "Twitch Flashbang";

            testMode = Convert.ToBoolean(AppConfig.Configuration["testMode"]);
            enableAfterimage = Convert.ToBoolean(AppConfig.Configuration["enableAfterimage"]);

            KeysConverter kcv = new();
            flashKey = (Keys?)kcv.ConvertFrom(AppConfig.Configuration["testKey"] ?? string.Empty) ?? Keys.None;

            if (enableAfterimage)
            {
                afterimage = new();
                afterimage.Show();
            }

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

            Debug.WriteLine($"Handle for Flashbang: {Handle:x}");

            if (Screen.PrimaryScreen == null)
            {
                return;
            }
            else
            {
                Size s;
                Point l;
                if (testMode)
                {
                    s = new Size(500, 500);
                    l = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (500 / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (500 / 2));

                }
                else
                {
                    s = new(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    l = Screen.PrimaryScreen.WorkingArea.Location;
                }

                Size = s;
                Location = l;
                if (enableAfterimage && afterimage != null)
                {
                    afterimage.SetSize(s);
                    afterimage.SetLocation(l);
                }
                screenshot = new(Screen.PrimaryScreen?.Bounds.Width ?? 0, Screen.PrimaryScreen?.Bounds.Height ?? 0);
                g = Graphics.FromImage(screenshot);

                numIterations = (int)(fadeDuration * updateRate);
            }

            WinAPI.CreateMagicWindow(this);

            Input.InstallHook();
            Input.KeyPressed += Input_KeyPressed;

            new Thread(t_ProcessFlashbangs).Start();
        }

        private void Input_KeyPressed(Keys k)
        {
            if (k == flashKey)
            {
                QueueTestFlash();
            }
            Debug.WriteLine(k.ToString());
        }

        private void Exit(object? sender, EventArgs e)
        {
            Input.UninstallHook();
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
                if (!isBlinding && !flashbangs.IsEmpty)
                {
                    Debug.WriteLine($"\ncurrent queue length: {flashbangs.Count - 1}");
                    FlashbangData? fd;
                    if (flashbangs.TryDequeue(out fd))
                    {
                        forceAbort = true;

                        new Thread(() => Flash(fd)).Start();
                    }
                }
                else
                {
                    forceAbort = false;
                }

                if (forceAbort)
                {
                    Thread.Sleep(RandomNumberGenerator.GetInt32(1300, 1900));
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void QueueTestFlash()
        {
            FlashbangData fd = new(GenerateID(6), GenerateID(6));
            Debug.WriteLine($"{fd.ID} enqueueing..");
            flashbangs.Enqueue(fd);
        }

        private void Flash(FlashbangData fd)
        {
            if (!isFading && enableAfterimage)
            {
                TakeScreenshot();
                afterimage.SetImage(screenshot);
                Debug.WriteLine($"Taking screenshot, isFading: {isFading}");
            }

            isBlinding = true;

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
            {
                player.Play();
            }

            DateTime dtStart = DateTime.Now;
            fd.flashingStopwatch.Start();

            BeginInvoke(() =>
            {
                WinAPI.SetLayeredWindowAttributes(Handle, 0, (byte)_Opacity, 0x2);
                WinAPI.UpdateWindow(Handle);
            });

            if (enableAfterimage)
            {
                afterimage.SetOpacity((byte)_Opacity);
            }

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

                BeginInvoke(() =>
                {
                    WinAPI.SetLayeredWindowAttributes(Handle, 0, (byte)_Opacity, 0x2);
                    WinAPI.UpdateWindow(Handle);
                });

                if (enableAfterimage)
                {
                    afterimage.SetOpacity((byte)_Opacity);
                }

                while ((fd.fadingStopwatch.Elapsed.TotalSeconds < (i + 1) * (1.0 / updateRate)) && !forceAbort)
                {
                    Thread.Sleep(1);
                }

                if (forceAbort)
                {
                    DateTime aborted_dtStop = DateTime.Now;
                    TimeSpan aborted_timeSpan = dtStart - aborted_dtStop;

                    Debug.WriteLine($"[{fd.ID}] aborting at {Math.Abs(aborted_timeSpan.TotalMilliseconds)}ms");

                    return;
                }
            }

            DateTime dtStop = DateTime.Now;
            TimeSpan timeSpan = dtStart - dtStop;

            Debug.WriteLine($"[#{fd.ID}] done in {Math.Abs(timeSpan.TotalMilliseconds)}ms");
            fd.fadingStopwatch.Stop();
            isFading = false;

            fd.Dispose();

            BeginInvoke(() =>
            {
                WinAPI.SetLayeredWindowAttributes(Handle, 0, 0, 0x2);
                WinAPI.UpdateWindow(Handle);
            });

            return;
        }

        public static string? GetForegroundWindowExecutableName()
        {
            IntPtr hWnd = WinAPI.GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
            {
                return null;
            }

            StringBuilder titleBuilder = new(256);
            WinAPI.GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);

            WinAPI.GetWindowThreadProcessId(hWnd, out uint processId);
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
            if (captureRectangle.HasValue)
            {
                g.CopyFromScreen(captureRectangle.Value.Left, captureRectangle.Value.Top, 0, 0, captureRectangle.Value.Size);
            }
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

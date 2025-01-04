using System.Runtime.InteropServices;

namespace TwitchFlashbang
{
    public partial class UsedFlashbangUI : UserControl
    {
        public UsedFlashbangUI(TriggeredFlashbangData f, int N)
        {
            InitializeComponent();

            flashingTimeLabel.Text = f.FlashTime + "s";
            flashingTimeLabel.ForeColor = Color.FromArgb(66, 66, 66);
            fadingTimeLabel.Text = f.FadingTime + "s";
            fadingTimeLabel.ForeColor = Color.FromArgb(66, 66, 66);

            IDLabel.Text = $"[{f.ID}]";

            if (f.Aborted)
                abortedLabel.Visible = true;
            else abortedLabel.Visible = false;

            NthLabel.Text = N.ToString();

            foreach (Control c in Controls)
            {
                if (c == abortedLabel && !f.Aborted)
                    continue;

                c.Visible = true;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Region = Region.FromHrgn(WinAPI.CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
        }
    }
}

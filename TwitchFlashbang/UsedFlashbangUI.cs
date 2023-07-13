namespace TwitchFlashbang
{
    public partial class UsedFlashbangUI : UserControl
    {
        public UsedFlashbangUI(FlashbangData f, int N)
        {
            InitializeComponent();

            flashbangTime.Text = f.flashingStopwatch.Elapsed.TotalSeconds + "s";
            flashbangTime.ForeColor = Color.FromArgb(66, 66, 66);

            NthLabel.Text = N.ToString();

            foreach (Control c in Controls)
            {
                c.Visible = true;
            }
        }
    }
}

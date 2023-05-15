namespace TwitchFlashbang
{
    public partial class DebugWindow : Form
    {

        public DebugWindow()
        {
            InitializeComponent();
        }

        private void DebugWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        internal void SetQueueCounter(int count)
        {
            flashbangCountLabel.Text = count.ToString();
        }
    }
}

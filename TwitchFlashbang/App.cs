using System.Diagnostics;
using TwitchFlashbang.Twitch;

namespace TwitchFlashbang
{
    public partial class App : Form
    {
        private readonly TwitchAPI? _twitchAPI;
        private readonly Flashbang _flashbang;
        private ConfigManager _configManager;
        private List<RewardsData> _rewardsData;
        private int N = 0;

        public App()
        {
            Application.EnableVisualStyles();
            InitializeComponent();

            _configManager = ConfigManager.Load();

            if (!_configManager.TestMode)
            {
                _twitchAPI = new TwitchAPI();
                _twitchAPI.OnTwitchCredentialsToBeSet += _twitchAPI_OnTwitchCredentialsToBeSet;
                _twitchAPI.OnBotInitialized += _twitchAPI_OnBotInitialized;
                _twitchAPI.OnRewardsData += _twitchAPI_OnRewardsData;
            }

            viewSwitcher.WarningFired += ViewSwitcher_WarningFired;

            _flashbang = new(_twitchAPI);
            _flashbang.Show();

            _flashbang.OnFlashbangTriggered += _flashbang_OnFlashbangTriggered;

            progressBar1.Visible = true;

            redirectUriLabel.Text = _configManager.Twitch.RedirectUri;
        }

        private void _flashbang_OnFlashbangTriggered(object? _, TriggeredFlashbangData f)
        {
            UsedFlashbangUI flashbangUI = new(f, ++N);
            flowLayoutPanel1.Controls.Add(flashbangUI);
            flowLayoutPanel1.Controls.SetChildIndex(flashbangUI, 0);
            Invoke(() => totalFlashbangsLabel.Text = N.ToString());
        }

        private void ViewSwitcher_WarningFired(string w)
        {
            Invoke(() =>
            {
                warningLabel.Text = w;
                progressBar1.Visible = false;
            });

        }

        private void _twitchAPI_OnRewardsData(List<RewardsData> rewards)
        {
            _rewardsData = rewards;
            foreach (var reward in rewards)
            {
                Invoke(() => checkedListBox1.Items.Add($"{reward.Name}"));
            }
        }

        private void InitiateTwitchFlow()
        {
            TwitchAuthHandler.OnCodeReceived += TwitchAuthHandler_OnCodeReceived;
            TwitchAuthHandler.OnTwitchCredentialsSet += TwitchAuthHandler_OnTwitchCredentialsSet;

            if (_twitchAPI is not null)
                Task.Run(_twitchAPI.MainAsync);
        }

        private void _twitchAPI_OnBotInitialized(object? sender, EventArgs e)
        {
            _configManager = ConfigManager.Load();

            if (_configManager.Twitch.RewardIDs.Count == 0)
            {
                viewSwitcher.Switch(ViewManager.View.Rewards);
            }
            else
            {
                viewSwitcher.Switch(ViewManager.View.AppStatus);
            }
        }

        private void LoadCredentialsIntoUI()
        {
            _configManager = ConfigManager.Load();
            textBox_ClientID.Text = _configManager.Twitch.ClientID;
            textBox_ClientSecret.Text = _configManager.Twitch.ClientSecret;
            textBox_channelName.Text = _configManager.Twitch.ChannelName;
        }

        private void TwitchAuthHandler_OnTwitchCredentialsSet(object? sender, EventArgs e)
        {
            _configManager = ConfigManager.Load();

            if (_configManager.Twitch.RewardIDs.Count == 0)
            {
                viewSwitcher.Switch(ViewManager.View.Rewards);
            }
            else
            {
                viewSwitcher.Switch(ViewManager.View.AppStatus);
            }
        }

        private void _twitchAPI_OnTwitchCredentialsToBeSet(ViewManager.MissingCredentials c)
        {
            viewSwitcher.Switch(ViewManager.View.TwitchAuthentication, c);
        }

        private void TwitchAuthHandler_OnCodeReceived(object? sender, string e)
        {
            Debug.WriteLine($"Received:\n{e}");
        }

        private void App_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(linkLabel1.Text) { UseShellExecute = true });
        }

        private void button_saveCredentials_Click(object sender, EventArgs e)
        {
            SaveCredentials();
            _configManager = ConfigManager.Load();

            var mc = AreConfigCredentialsValid();
            if (mc == 0)
            {
                if (_configManager.Twitch.RewardIDs.Count == 0) //for first time
                {
                    viewSwitcher.Switch(ViewManager.View.Rewards);
                }
                else
                {
                    viewSwitcher.Switch(ViewManager.View.AppStatus);
                }

                InitiateTwitchFlow();
            }
            else
            {
                viewSwitcher.Switch(ViewManager.View.TwitchAuthentication, mc);
            }
        }

        private ViewManager.MissingCredentials AreConfigCredentialsValid()
        {
            _configManager = ConfigManager.Load();

            ViewManager.MissingCredentials mc = ViewManager.MissingCredentials.None;

            if (string.IsNullOrEmpty(_configManager.Twitch.ClientID))
            {
                mc |= ViewManager.MissingCredentials.MissingClientID;
            }

            if (string.IsNullOrEmpty(_configManager.Twitch.ClientSecret))
            {
                mc |= ViewManager.MissingCredentials.MissingClientSecret;
            }

            if (string.IsNullOrEmpty(_configManager.Twitch.ChannelName))
            {
                mc |= ViewManager.MissingCredentials.MissingChannelName;
            }

            return mc;
        }

        private void SaveCredentials()
        {
            _configManager = ConfigManager.Load();

            if (!string.IsNullOrEmpty(textBox_ClientID.Text) &&
                !string.IsNullOrEmpty(textBox_channelName.Text) &&
                !string.IsNullOrEmpty(textBox_ClientSecret.Text))
            {
                _configManager.Twitch.ClientID = textBox_ClientID.Text;
                _configManager.Twitch.ClientSecret = textBox_ClientSecret.Text;
                _configManager.Twitch.ChannelName = textBox_channelName.Text;
                _configManager.Save();
            }
        }

        private void App_Load(object sender, EventArgs e)
        {
            Location = Screen.PrimaryScreen.WorkingArea.Location;
            LoadCredentialsIntoUI();

            var mc = AreConfigCredentialsValid();
            if (mc == 0)
            {
                SaveCredentials();
                InitiateTwitchFlow();
            }
            else
            {
                viewSwitcher.Switch(ViewManager.View.TwitchAuthentication, mc);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _configManager = ConfigManager.Load();
            foreach (var reward in _rewardsData)
            {
                if (reward.Selected)
                {
                    _configManager.Twitch.RewardIDs.Add(reward.ID);
                }
            }

            _configManager.Save();
            viewSwitcher.Switch(ViewManager.View.AppStatus);
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            button1.Enabled = e.NewValue == CheckState.Checked;

            _rewardsData[e.Index].Selected = e.NewValue == CheckState.Checked;
        }

        protected override void OnLoad(EventArgs e)
        {
            _configManager = ConfigManager.Load();
            if (_configManager.TestMode)
            {
                viewSwitcher.Switch(ViewManager.View.AppStatus);
                Location = new Point(0, 0);
            }

            base.OnLoad(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_configManager.Twitch.RedirectUri);
        }
    }
}
using System.Diagnostics;
using System.Net;
using TwitchLib.Client;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace TwitchFlashbang.Twitch
{
    public class TwitchAPI
    {
        private readonly List<string> Scopes = new()
        {
            "channel:read:redemptions",
            "bits:read"
        };

        private TwitchPubSub PubSub;
        private TwitchLib.Api.TwitchAPI API = new();

        public event EventHandler OnBotInitialized;
        public event Action<FlashbangData> OnFlashbangData;
        public event Action<ViewManager.MissingCredentials> OnTwitchCredentialsToBeSet;
        public event Action<List<RewardsData>> OnRewardsData;

        private ConfigManager _configManager;
        private int minFlashbangBits = 0;

        public async void MainAsync()
        {
            _configManager = ConfigManager.Load();

            TwitchAuthHandler.OnTwitchCredentialsSet += TwitchAuthHandler_OnTwitchCredentialsSet;

            Scopes.ForEach(s => WebUtility.UrlEncode(s));
            string _scopes = string.Join("+", Scopes);

            PubSub = new();
            PubSub.OnListenResponse += OnListenResponse;
            PubSub.OnPubSubServiceConnected += OnPubSubServiceConnected;
            PubSub.OnPubSubServiceClosed += OnPubSubServiceClosed;
            PubSub.OnPubSubServiceError += OnPubSubServiceError;

            if (int.TryParse(_configManager.Twitch.MinFlashbangBits, out minFlashbangBits) && minFlashbangBits > 0)
            {
                PubSub.OnBitsReceivedV2 += PubSub_OnBitsReceivedV2;
            }

            if (string.IsNullOrEmpty(_configManager.Twitch.Token) || string.IsNullOrEmpty(_configManager.Twitch.RefreshToken))
            {
                if (string.IsNullOrEmpty(_configManager.Twitch.Token) || string.IsNullOrEmpty(_configManager.Twitch.RefreshToken))
                {
                    var tokens = await TwitchAuthHandler.GetAuthCode(_configManager.Twitch.ClientID, _configManager.Twitch.ClientSecret, _configManager.Twitch.RedirectUri, Scopes);
                    if (tokens is not null)
                    {
                        _configManager.Twitch.Token = tokens.AccessToken;
                        _configManager.Twitch.RefreshToken = tokens.RefreshToken;
                        _configManager.Save();
                    }
                }
                else if
                    (string.IsNullOrEmpty(_configManager.Twitch.ClientID))
                {
                    OnTwitchCredentialsToBeSet?.Invoke(ViewManager.MissingCredentials.MissingClientID);
                }
                else if
                    (string.IsNullOrEmpty(_configManager.Twitch.ClientSecret))
                {
                    OnTwitchCredentialsToBeSet?.Invoke(ViewManager.MissingCredentials.MissingClientSecret);
                }
            }
            else
            {
                UpdateTwitchCredentials().Wait();
            }
        }

        private async void TwitchAuthHandler_OnTwitchCredentialsSet(object? sender, EventArgs e)
        {
            await UpdateTwitchCredentials();
        }

        private async Task UpdateTwitchCredentials()
        {
            Debug.WriteLine("Twitch credentials are set");
            _configManager = ConfigManager.Load();
            API.Settings.ClientId = _configManager.Twitch.ClientID;
            API.Settings.AccessToken = _configManager.Twitch.Token;

            if (await RetrieveRewards())
            {
                OnBotInitialized?.Invoke(this, EventArgs.Empty);
                await Task.Delay(Timeout.Infinite);
            }
        }

        private async Task<bool> RetrieveRewards()
        {
            _configManager = ConfigManager.Load();
            var tokenValid = await API.Auth.ValidateAccessTokenAsync(API.Settings.AccessToken);

            if (tokenValid is null)
            {
                Debug.WriteLine("Token needs refreshing..");

                API.Settings.AccessToken = API.Auth.RefreshAuthTokenAsync(_configManager.Twitch.RefreshToken, _configManager.Twitch.ClientSecret, _configManager.Twitch.ClientID).Result.AccessToken;
                _configManager.Twitch.Token = API.Settings.AccessToken;
                _configManager.Save();
            }

            if (string.IsNullOrEmpty(_configManager.Twitch.ChannelID))
            {
                var channel_data = await API.Helix.Users.GetUsersAsync(null, new List<string>() { _configManager.Twitch.ChannelName }, API.Settings.AccessToken);
                _configManager.Twitch.ChannelID = channel_data.Users[0].Id;
                _configManager.Save();
            }

            try
            {
                var rewards = await API.Helix.ChannelPoints.GetCustomRewardAsync(_configManager.Twitch.ChannelID);
                if (rewards is not null)
                {
                    List<RewardsData> rewards_data = new();
                    foreach (var reward in rewards.Data)
                    {
                        Debug.WriteLine($"{reward.Title} - {reward.Id}");
                        rewards_data.Add(new RewardsData { ID = reward.Id, Name = reward.Title });
                    }

                    OnRewardsData?.Invoke(rewards_data);
                }
            }
            catch (TwitchLib.Api.Core.Exceptions.BadScopeException)
            {
                OnTwitchCredentialsToBeSet?.Invoke(ViewManager.MissingCredentials.MissingChannelName);
                return false;
            }

            PubSub.Connect();
            ListenToRewards(_configManager.Twitch.ChannelID);
            return true;
        }

        #region Reward Events

        private void ListenToRewards(string channelId)
        {
            PubSub.OnChannelPointsRewardRedeemed += PubSub_OnChannelPointsRewardRedeemed;
            PubSub.ListenToChannelPoints(channelId);
            PubSub.ListenToBitsEventsV2(channelId);
        }

        private void PubSub_OnChannelPointsRewardRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            _configManager = ConfigManager.Load();
            if (_configManager.Twitch is null)
            {
                throw new ArgumentNullException(nameof(_configManager.Twitch));
            }

            var IDs = _configManager.Twitch.RewardIDs;

            foreach (var ID in IDs)
            {
                if (e.RewardRedeemed.Redemption.Reward.Id == ID)
                {
                    OnFlashbangData?.Invoke(new FlashbangData(e.RewardRedeemed.Redemption.Id.Substring(e.RewardRedeemed.Redemption.Id.Length - 6, 6), e.ChannelId));
                }
            }
        }

        private void PubSub_OnBitsReceivedV2(object? sender, OnBitsReceivedV2Args e)
        {
            Debug.WriteLine($"Received {e.BitsUsed} bits from {e.UserName}");
            if (e.BitsUsed >= minFlashbangBits)
            {
                OnFlashbangData?.Invoke(new FlashbangData(e.Context, e.ChannelId));
            }
        }

        #endregion

        #region Pubsub events

        private void OnPubSubServiceError(object sender, OnPubSubServiceErrorArgs e)
        {
            Debug.WriteLine($"{e.Exception.Message}");
        }

        private void OnPubSubServiceClosed(object sender, EventArgs e)
        {
            Debug.WriteLine($"Connection closed to pubsub server");
        }

        private void OnPubSubServiceConnected(object sender, EventArgs e)
        {
            Debug.WriteLine($"Connected to pubsub server");
            PubSub.SendTopics(_configManager.Twitch.Token);
        }

        private void OnListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
            {
                Debug.WriteLine($"Failed to listen! Response: {e.Response.Error}");
            }
        }

        internal string GetClientID()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

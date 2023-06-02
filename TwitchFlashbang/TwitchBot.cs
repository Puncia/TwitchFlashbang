using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace TwitchFlashbang
{
    public class TwitchBot
    {
        public TwitchPubSub PubSub;

        public ITwitchAPI? API;

        public event Action<FlashbangData> OnFlashbangData;

        public async Task MainAsync()
        {
            var channelId = AppConfig.Configuration["twitchConfig:channelID"];

            API = new TwitchAPI();
            API.Settings.ClientId = AppConfig.Configuration["twitchConfig:clientID"];
            API.Settings.Secret = AppConfig.Configuration["twitchConfig:secret"];

            PubSub = new TwitchPubSub();
            PubSub.OnListenResponse += OnListenResponse;
            PubSub.OnPubSubServiceConnected += OnPubSubServiceConnected;
            PubSub.OnPubSubServiceClosed += OnPubSubServiceClosed;
            PubSub.OnPubSubServiceError += OnPubSubServiceError;

            ListenToRewards(channelId);

            PubSub.Connect();

            await Task.Delay(Timeout.Infinite);
        }

        

        #region Reward Events

        private void ListenToRewards(string channelId)
        {
            PubSub.OnChannelPointsRewardRedeemed += PubSub_OnChannelPointsRewardRedeemed;
            PubSub.ListenToChannelPoints(channelId);
        }
        private async void PubSub_OnChannelPointsRewardRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            var IDs = AppConfig.Configuration.GetSection("twitchConfig:rewardIDs").GetChildren();

            foreach (var ID in IDs)
            {
                if (e.RewardRedeemed.Redemption.Reward.Id == ID.Value)
                {
                    OnFlashbangData?.Invoke(new FlashbangData(e.RewardRedeemed.Redemption.Id, e.ChannelId));
                }
            }
        }

        #endregion

        #region Bits Events

        private void ListenToBits(string channelId)
        {
            PubSub.OnBitsReceived += PubSub_OnBitsReceived;
            PubSub.ListenToBitsEventsV2(channelId);
        }

        private void PubSub_OnBitsReceived(object sender, OnBitsReceivedArgs e)
        {
            Console.WriteLine($"{e.Username} trowed {e.TotalBitsUsed} bits");
        }

        #endregion

        #region Pubsub events

        private void OnPubSubServiceError(object sender, OnPubSubServiceErrorArgs e)
        {
            Console.WriteLine($"{e.Exception.Message}");
        }

        private void OnPubSubServiceClosed(object sender, EventArgs e)
        {
            Console.WriteLine($"Connection closed to pubsub server");
        }

        private void OnPubSubServiceConnected(object sender, EventArgs e)
        {
            Console.WriteLine($"Connected to pubsub server");
            var oauth = "su4mqtvnxo2f10jturvk83v83e4sc9";
            PubSub.SendTopics(oauth);
        }

        private void OnListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
            {
                Console.WriteLine($"Failed to listen! Response{e.Response.Error}");
            }
        }

        #endregion
    }
}

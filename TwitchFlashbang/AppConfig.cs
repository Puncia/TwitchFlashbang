using Microsoft.Extensions.Configuration;

namespace TwitchFlashbang
{
    internal class AppConfig
    {
        public static IConfiguration Configuration { get; set; }
        public TwitchConfig TwitchConfig { get; set; } = new TwitchConfig();
        
    }

    internal class TwitchConfig
    {
        public string channelID { get; set; } = string.Empty;
        public string clientID { get; set; } = string.Empty;
        public string clientSecret { get; set; } = string.Empty;
        public string[] rewardIDs { get; set; } = new string[0];
    }
}

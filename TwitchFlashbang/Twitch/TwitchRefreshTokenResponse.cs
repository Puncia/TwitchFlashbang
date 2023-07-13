using Newtonsoft.Json;

namespace TwitchFlashbang.Twitch
{
    internal class TwitchRefreshTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public List<string> Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}

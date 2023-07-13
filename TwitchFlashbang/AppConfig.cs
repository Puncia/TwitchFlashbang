using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TwitchFlashbang
{
    public class ConfigManager : AppConfig
    {
        private readonly object saveLock = new();
        private static readonly object readLock = new();

        [JsonIgnore]
        public string FileName { get => fileName; set => fileName = value; }
        private string fileName;

        public void Save()
        {
            lock (saveLock)
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FileName, json);
            }
        }

        public static ConfigManager Load(string? fileName = null)
        {
            lock (readLock)
            {
                if (string.IsNullOrEmpty(fileName))
                {
#if DEBUG
                    fileName = @$"{Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName}\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.json";
#else
                    fileName = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.json";
#endif
                }

                if (!File.Exists(fileName))
                {
                    return Defaults(fileName);
                }

                var json = File.ReadAllText(fileName);
                if (!IsValidJson(json))
                {
                    Debug.WriteLine($"Found config file {fileName} but its content is invalid. Overwriting..");
                    File.Delete(fileName);
                    return Defaults(fileName);
                }
                try
                {
                    ConfigManager cm = JsonSerializer.Deserialize<ConfigManager>(json)!;
                    cm.FileName = fileName;
                    return cm;
                }
                catch (JsonException)
                {
                    return Defaults(fileName);
                }
            }
        }

        private static ConfigManager Defaults(string fileName) =>
            new()
            {
                EnableAfterimage = true,
                FileName = fileName,
                Twitch = new TwitchConfig { RedirectUri = "http://localhost:8758/" }
            };

        private static bool IsValidJson(string jsonContent)
        {
            try
            {
                // Attempt to parse the JSON content without throwing an exception
                JsonDocument.Parse(jsonContent);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }

    public class AppConfig
    {
        private TwitchConfig twitch;
        private List<PerAppOpacity> perAppOpacity;
        private string testKey;

        public bool EnableAfterimage { get; set; }
        public List<PerAppOpacity> PerAppOpacity { get => perAppOpacity ?? new List<PerAppOpacity>(); set => perAppOpacity = value; }
        public string TestKey { get => testKey ?? string.Empty; set => testKey = value; }
        public bool TestMode { get; set; }
        public TwitchConfig Twitch { get => twitch ?? new TwitchConfig(); set => twitch = value; }
    }

    public class TwitchConfig
    {
        private List<string> rewardIDs;
        private string channelID;
        private string clientID;
        private string clientSecret;
        private string redirectUri;
        private string refreshToken;
        private string token;
        private string channelName;
        private string minFlashbangBits;

        public string MinFlashbangBits { get => minFlashbangBits ?? string.Empty; set => minFlashbangBits = value; }
        public string ChannelName { get => channelName ?? string.Empty; set => channelName = value; }
        public string ChannelID { get => channelID ?? string.Empty; set => channelID = value; }
        public string ClientID { get => clientID ?? string.Empty; set => clientID = value; }
        public string ClientSecret { get => clientSecret ?? string.Empty; set => clientSecret = value; }
        public string RedirectUri { get => redirectUri ?? string.Empty; set => redirectUri = value; }
        public string RefreshToken { get => refreshToken ?? string.Empty; set => refreshToken = value; }
        public List<string> RewardIDs { get => rewardIDs ?? new List<string>(); set => rewardIDs = value; }
        public string Token { get => token ?? string.Empty; set => token = value; }
    }

    public class PerAppOpacity
    {
        private string appName;

        public string AppName { get => appName ?? string.Empty; set => appName = value; }
        public double Opacity { get; set; }
    }
}

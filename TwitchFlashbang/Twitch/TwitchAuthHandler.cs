using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TwitchFlashbang.Twitch
{
    public static class TwitchAuthHandler
    {
        public static event EventHandler<string> OnCodeReceived;
        public static event EventHandler OnTwitchCredentialsSet;

        private static readonly HttpClient client = new();
        private static HttpListener listener;
        private static HttpListenerContext context;

        public static async Task<TwitchAuthFlowTokensResponse?> GetAuthCode(string ClientID, string ClientSecret, string RedirectURI, List<string> Scopes)
        {
            byte[] bytes = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            string state = new Guid(bytes).ToString().Replace("-", "");

            Scopes.ForEach(s => WebUtility.UrlEncode(s));
            string encoded_scopes = string.Join("+", Scopes);

            string auth_url = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientID}&redirect_uri={RedirectURI}&scope={encoded_scopes}&state={state}";

            Process.Start(new ProcessStartInfo(auth_url) { UseShellExecute = true });

            if (listener is null)
            {
                listener = new();

                listener.Prefixes.Add($"{RedirectURI}");
                listener.Start();
            }
            string str_result = "Authentication failure.";

            if (context is null)
            {
                context = listener.GetContext();
            }

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            TwitchAuthFlowTokensResponse? tokensResponse = new();

            if (request.RawUrl is not null && request.RawUrl.Contains("code"))
            {
                //retrieve url paramters
                Dictionary<string, string> postParams = new();
                string[] raw = request.RawUrl.Split('&');
                foreach (string param in raw)
                {
                    string[] kvPair = param.Split('=');
                    string key = kvPair[0].Replace("/?", "");
                    string value = HttpUtility.UrlDecode(kvPair[1]);
                    postParams.Add(key, value);
                }

                Debug.WriteLine($"{postParams["state"]}, {state}");

                if (postParams["state"] != state)
                {
                    return null;
                }

                int i = request.RawUrl.IndexOf("code") + 5;
                string code = request.RawUrl.Substring(i, request.RawUrl.IndexOf('&') - i);
                OnCodeReceived?.Invoke(null, code);
                tokensResponse = await GetOAuthToken(code, ClientID, ClientSecret, RedirectURI);

                str_result = "Authenticated! You can close this page.";
            }

            string str_response = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Twitch Flashbang</title>\r\n</head>\r\n<body>\r\n    <p>{str_result}</p><p style=\"font-size: 12px\">state: {state}</p>\r\n</body>\r\n</html>";

            byte[] buffer = Encoding.UTF8.GetBytes(str_response);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;

            output.Write(buffer, 0, buffer.Length);
            output.Close();
            listener.Close();

            OnTwitchCredentialsSet?.Invoke(null, new EventArgs());
            return tokensResponse;
        }

        private static async Task<TwitchAuthFlowTokensResponse?> GetOAuthToken(string code, string ClientID, string ClientSecret, string RedirectUri)
        {
            string tokenEndpoint = "https://id.twitch.tv/oauth2/token";
            string clientId = ClientID;
            string clientSecret = ClientSecret;
            string grantType = "authorization_code";
            string redirectUri = RedirectUri;

            var parameters = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "grant_type", grantType },
                { "redirect_uri", redirectUri }
            };

            var content = new FormUrlEncodedContent(parameters);

            HttpResponseMessage response = await client.PostAsync(tokenEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                TwitchAuthFlowTokensResponse? str = JsonConvert.DeserializeObject<TwitchAuthFlowTokensResponse>(responseContent);

                if (str is null)
                {
                    return null;
                }

                return str;
            }
            else
            {
                Debug.WriteLine($"Request failed with status code {response.StatusCode}");
                return null;
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft
{
    public class MojangXboxApi
    {
        public static readonly string RelyingParty = "rp://api.minecraftservices.com/";
        private readonly HttpClient httpClient;
        private readonly ILogger<MojangXboxApi>? logger;

        public MojangXboxApi(HttpClient client, ILoggerFactory? logFactory = null)
        {
            this.httpClient = client;
            this.logger = logFactory?.CreateLogger<MojangXboxApi>();
        }

        public async Task<MojangXboxLoginResponse> LoginWithXbox(string uhs, string xstsToken)
        {
            if (uhs == null)
                throw new ArgumentNullException(nameof(uhs));
            if (xstsToken == null)
                throw new ArgumentNullException(nameof(xstsToken));

            logger?.LogTrace("LoginWithXbox");

            var res = await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.minecraftservices.com/authentication/login_with_xbox"),
                Content = new StringContent($"{{\"identityToken\": \"XBL3.0 x={uhs};{xstsToken}\"}}", Encoding.UTF8, "application/json"),
            });

            var resContent = await res.Content.ReadAsStringAsync();
            logger?.LogTrace(resContent);
            var resObj = JsonSerializer.Deserialize<MojangXboxLoginResponse>(resContent)
                ?? new MojangXboxLoginResponse();
            resObj.ExpiresOn = DateTime.Now.AddSeconds(resObj.ExpiresIn);
            return resObj;
        }

        public async Task<bool> CheckGameOwnership(string bearerToken)
        {
            if (string.IsNullOrEmpty(bearerToken))
                throw new ArgumentNullException(nameof(bearerToken));

            var req = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.minecraftservices.com/entitlements/mcstore"),
            };
            req.Headers.Add("Authorization", "Bearer " + bearerToken);

            logger?.LogTrace("CheckGameOwnership");

            var res = await httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode)
                return false;
            var resBody = await res.Content.ReadAsStringAsync();
            logger?.LogTrace(resBody);
            
            using var jsonDocument = JsonDocument.Parse(resBody);
            var root = jsonDocument.RootElement;

            if (root.TryGetProperty("items", out var items))
                return items.EnumerateArray().Any();
            else
                return false;
        }

        public async Task<MSession> GetProfileUsingToken(string bearerToken)
        {
            if (string.IsNullOrEmpty(bearerToken))
                throw new ArgumentNullException(nameof(bearerToken));

            logger?.LogTrace("GetProfileUsingToken");
            var req = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.minecraftservices.com/minecraft/profile")
            };
            req.Headers.Add("Authorization", "Bearer " + bearerToken);

            var res = await httpClient.SendAsync(req);
            var resBody = await res.Content.ReadAsStringAsync();
            logger?.LogTrace(resBody);
            res.EnsureSuccessStatusCode();

            using var jsonDocument = JsonDocument.Parse(resBody);
            var root = jsonDocument.RootElement;

            var session = new MSession();
            if (root.TryGetProperty("id", out var id))
                session.UUID = id.GetString();
            if (root.TryGetProperty("name", out var name))
                session.Username = name.GetString();

            session.AccessToken = bearerToken;
            session.UserType = "msa";
            return session;
        }
    }
}

using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.GameAuthenticators
{
    public class JEAuthenticationApi
    {
        public static readonly string RelyingParty = "rp://api.minecraftservices.com/";
        private readonly HttpClient httpClient;

        public JEAuthenticationApi(HttpClient client)
        {
            this.httpClient = client;
        }

        public async Task<JEAuthenticationToken> LoginWithXbox(string uhs, string xstsToken)
        {
            if (uhs == null)
                throw new ArgumentNullException(nameof(uhs));
            if (xstsToken == null)
                throw new ArgumentNullException(nameof(xstsToken));

            var res = await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.minecraftservices.com/authentication/login_with_xbox"),
                Content = new StringContent($"{{\"identityToken\": \"XBL3.0 x={uhs};{xstsToken}\"}}", Encoding.UTF8, "application/json"),
            });

            var resBody = await res.Content.ReadAsStringAsync();

            try
            {
                res.EnsureSuccessStatusCode();
                var resObj = JsonSerializer.Deserialize<JEAuthenticationToken>(resBody);

                if (resObj == null && res.IsSuccessStatusCode)
                    throw new MinecraftAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}");
                else if (resObj == null)
                    throw new MinecraftAuthException("Response was null");

                resObj.ExpiresOn = DateTime.UtcNow.AddSeconds(resObj.ExpiresIn);
                return resObj;
            }
            catch (Exception ex)
            {
                throw createException(ex, resBody, res);
            }
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

            var res = await httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode)
                return false;
            var resBody = await res.Content.ReadAsStringAsync();

            try
            {
                using var jsonDocument = JsonDocument.Parse(resBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("items", out var items))
                    return items.EnumerateArray().Any();
                else
                    return false;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public async Task<JEProfile> GetProfileUsingToken(string bearerToken)
        {
            if (string.IsNullOrEmpty(bearerToken))
                throw new ArgumentNullException(nameof(bearerToken));

            var req = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.minecraftservices.com/minecraft/profile")
            };
            req.Headers.Add("Authorization", "Bearer " + bearerToken);

            var res = await httpClient.SendAsync(req);
            var resBody = await res.Content.ReadAsStringAsync();

            try
            {
                res.EnsureSuccessStatusCode();
                var profile = JsonSerializer.Deserialize<JEProfile>(resBody);

                if (profile == null)
                    throw new JsonException();

                return profile;
            }
            catch (Exception ex)
            {
                throw createException(ex, resBody, res);
            }
        }

        private Exception createException(Exception ex, string resBody, HttpResponseMessage res)
        {
            if (ex is JsonException || ex is HttpRequestException)
            {
                try
                {
                    return MinecraftAuthException.FromResponseBody(resBody, (int)res.StatusCode);
                }
                catch (FormatException)
                {
                    return new MinecraftAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}");
                }
            }
            else
                return ex;
        }
    }
}
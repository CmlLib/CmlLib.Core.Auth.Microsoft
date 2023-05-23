using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CmlLib.Core.Bedrock.Auth
{
    // https://github.com/PrismarineJS/prismarine-auth/blob/master/src/TokenManagers/MinecraftBedrockTokenManager.js

    public class BEAuthenticationApi
    {
        public static readonly string RelyingParty = "https://multiplayer.minecraft.net/";
        private readonly HttpClient _httpClient;

        public BEAuthenticationApi(HttpClient client)
        {
            this._httpClient = client;
        }

        public async Task<BEToken[]> LoginWithXbox(string uhs, string xsts)
        {
            var req = JsonSerializer.Serialize(new
            {
                identityPublicKey = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEQUeCLz6XuGSZaLldVOoYSZhdT3F371zgus9VMJ5eQoTr1dPjNdN1MtYNOtN1KiWFwxWjqsxNQ4wVkFjCIufCsEYEJgje7Jh9xx37STA0Lq3W3njn8nbJuDUM866vDJAG"
            });

            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri("https://multiplayer.minecraft.net/authentication"),
                Content = new StringContent(req, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post
            };

            msg.Headers.Add("User-Agent", "MCPC/UWP");
            msg.Headers.Add("Authorization", $"XBL3.0 x={uhs};{xsts}");

            var res = await _httpClient.SendAsync(msg);
            var resStr = await res.Content.ReadAsStringAsync();

            try
            {
                res.EnsureSuccessStatusCode();

                using var doc = JsonDocument.Parse(resStr);
                var chains = doc.RootElement.GetProperty("chain").EnumerateArray();

                var result = chains
                    .Select(chain => new BEToken(chain.GetString()!))
                    .Where(chain => chain != null)
                    .ToArray();

                return result!;
            }
            catch (Exception ex)
            {
                throw createException(ex, resStr, res);
            }
        }

        private Exception createException(Exception ex, string resBody, HttpResponseMessage res)
        {
            if (ex is JsonException || ex is HttpRequestException)
            {
                try
                {
                    return BedrockAuthException.FromResponseBody(resBody, (int)res.StatusCode);
                }
                catch (FormatException)
                {
                    return new BedrockAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}\n{resBody}");
                }
            }
            else
                return ex;
        }
    }
}

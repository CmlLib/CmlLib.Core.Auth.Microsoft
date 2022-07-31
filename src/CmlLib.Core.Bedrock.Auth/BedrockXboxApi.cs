using CmlLib.Core.Bedrock.Auth.Models;
using System.Text;
using System.Text.Json;

namespace CmlLib.Core.Bedrock.Auth
{
    // 아래 소스코드를 상당 부분 참고하였음.
    // https://github.com/PrismarineJS/prismarine-auth/blob/master/src/TokenManagers/MinecraftBedrockTokenManager.js

    public class BedrockXboxApi : IBedrockXboxApi
    {
        private readonly HttpClient _httpClient;

        public BedrockXboxApi(HttpClient client)
        {
            this._httpClient = client;
        }

        public async Task<BedrockToken[]> LoginWithXbox(string uhs, string xsts)
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
                    .Select(chain => new BedrockToken(chain.GetString()))
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

using CmlLib.Core.Auth.Microsoft.Jwt;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.Mojang
{
    public class MojangXboxLoginResponse
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("roles")]
        public string[]? Roles { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("expires_on")]
        public DateTime ExpiresOn { get; set; }

        public MojangXboxAccessTokenPayload DecodeAccesTokenPayload()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
                throw new InvalidOperationException("AccessToken is null");

            return JwtDecoder.DecodePayload<MojangXboxAccessTokenPayload>(this.AccessToken!);
        }

        public bool CheckValidation()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
                return false;

            if (this.ExpiresOn <= DateTime.UtcNow || string.IsNullOrEmpty(this.AccessToken))
                return false;

            try
            {
                var payload = DecodeAccesTokenPayload();
                var exp = DateTimeOffset.FromUnixTimeSeconds(payload.Exp);

                if (exp <= DateTimeOffset.UtcNow)
                    return false;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}

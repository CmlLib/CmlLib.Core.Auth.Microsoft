using System.Text.Json;
using System.Text.Json.Serialization;
using XboxAuthNet.Game.Jwt;

namespace CmlLib.Core.Auth.Microsoft.Sessions;

public class JEToken
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

    /// <summary>
    /// decode jwt payload of AccessToken
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">AccessToken is null or empty</exception>
    /// <exception cref="FormatException">AccessToken is not valid jwt</exception>
    public MojangXboxAccessTokenPayload? DecodeAccesTokenPayload()
    {
        if (string.IsNullOrEmpty(this.AccessToken))
            throw new InvalidOperationException("this.AccessToken was empty.");

        return JwtDecoder.DecodePayload<MojangXboxAccessTokenPayload>(this.AccessToken!);
    }

    /// <summary>
    /// check if access token is valid
    /// </summary>
    /// <returns>validation result</returns>
    public bool Validate()
    {
        if (string.IsNullOrEmpty(this.AccessToken))
            return false;

        if (this.ExpiresOn <= DateTime.UtcNow || string.IsNullOrEmpty(this.AccessToken))
            return false;

        try
        {
            var payload = DecodeAccesTokenPayload();
            if (payload == null)
                return false;

            var exp = DateTimeOffset.FromUnixTimeSeconds(payload.Exp);
            if (exp <= DateTimeOffset.UtcNow)
                return false;
        }
        catch (FormatException)
        {
            // when jwt payload is not valid base64 string
            return false;
        }
        catch (JsonException)
        {
            // when jwt payload is not valid json string
            return false;
        }
        catch (ArgumentException)
        {
            // when exp of jwt is not valid unix timestamp
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        return this.AccessToken ?? string.Empty;
    }
}
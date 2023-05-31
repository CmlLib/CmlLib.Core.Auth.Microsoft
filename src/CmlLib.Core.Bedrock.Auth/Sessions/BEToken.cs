using XboxAuthNet.Game.Jwt;
using System.Text.Json;

namespace CmlLib.Core.Bedrock.Auth.Sessions;

public class BEToken
{
    public BEToken(string token)
    {
        this.Token = token;
    }

    public string Token { get; }

    public BETokenPayload? DecodeTokenPayload()
    {
        if (string.IsNullOrEmpty(Token))
            throw new InvalidOperationException("Token was empty");

        var payload = JwtDecoder.DecodePayload<BETokenPayload>(Token);
        return payload;
    }

    public bool CheckValidation()
    {
        if (string.IsNullOrEmpty(Token))
            return false;

        try
        {
            var payload = DecodeTokenPayload();
            if (payload == null)
                return false;

            var exp = DateTimeOffset.FromUnixTimeSeconds(payload.Expire);
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
}
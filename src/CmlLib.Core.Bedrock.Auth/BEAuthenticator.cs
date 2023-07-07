// reference
// https://github.com/PrismarineJS/prismarine-auth/blob/master/src/TokenManagers/MinecraftBedrockTokenManager.js

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuth;
using CmlLib.Core.Bedrock.Auth.Sessions;

namespace CmlLib.Core.Bedrock.Auth;

public class BEAuthenticator : SessionAuthenticator<BESession>
{
    public static readonly string RelyingParty = "https://multiplayer.minecraft.net/";
    private readonly ISessionSource<XboxAuthTokens> _xboxSessionSource;

    public BEAuthenticator(
        ISessionSource<XboxAuthTokens> xboxSessionSource,
        ISessionSource<BESession> sessionSource)
        : base(sessionSource) =>
        _xboxSessionSource = xboxSessionSource;

    protected override async ValueTask<BESession?> Authenticate(AuthenticateContext context)
    {
        var xboxTokens = _xboxSessionSource.Get(context.SessionStorage);
        var uhs = xboxTokens?.XstsToken?.XuiClaims?.UserHash;
        var xsts = xboxTokens?.XstsToken?.Token;

        if (string.IsNullOrEmpty(uhs) ||
            string.IsNullOrEmpty(xsts))
        {
            throw new BEAuthException("Cannot auth with null UserHash and null Token. Xbox authentication is required.");
        }

        context.Logger.LogInformation("Start BEAuthenticator");
        var tokens = await loginWithXbox(uhs, xsts, context.HttpClient);
        return new BESession()
        {
            Tokens = tokens
        };
    }

    public async Task<BEToken[]> loginWithXbox(string uhs, string xsts, HttpClient httpClient)
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

        var res = await httpClient.SendAsync(msg);
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
                return BEAuthException.FromResponseBody(resBody, (int)res.StatusCode);
            }
            catch (FormatException)
            {
                return new BEAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}\n{resBody}");
            }
        }
        else
            return ex;
    }
}
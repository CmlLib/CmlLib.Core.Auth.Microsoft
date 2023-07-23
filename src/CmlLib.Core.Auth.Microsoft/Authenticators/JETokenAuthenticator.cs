using System.Text;
using System.Text.Json;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuth;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JETokenAuthenticator : SessionAuthenticator<JEToken>
{
    private readonly ISessionSource<XboxAuthTokens> _xboxSessionSource;

    public JETokenAuthenticator(
        ISessionSource<XboxAuthTokens> xboxSessionSource,
        ISessionSource<JEToken> sessionSource)
        : base(sessionSource) =>
        _xboxSessionSource = xboxSessionSource;

    protected override async ValueTask<JEToken?> Authenticate(AuthenticateContext context)
    {
        context.Logger.LogJETokenAuthenticator();

        var xboxTokens = _xboxSessionSource.Get(context.SessionStorage);
        var uhs = xboxTokens?.XstsToken?.XuiClaims?.UserHash;
        var xsts = xboxTokens?.XstsToken?.Token;

        if (string.IsNullOrEmpty(uhs))
            throw new JEAuthException("UserHash was empty. Xbox authentication is required.");
        if (string.IsNullOrEmpty(xsts))
            throw new JEAuthException("XstsToken was empty. Xbox authentication is required.");

        return await requestToken(uhs, xsts, context.HttpClient);
    }

    private async ValueTask<JEToken> requestToken(string uhs, string xsts, HttpClient httpClient)
    {
        var res = await httpClient.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.minecraftservices.com/authentication/login_with_xbox"),
            Content = new StringContent($"{{\"identityToken\": \"XBL3.0 x={uhs};{xsts}\"}}", Encoding.UTF8, "application/json"),
        });

        var resBody = await res.Content.ReadAsStringAsync();

        try
        {
            res.EnsureSuccessStatusCode();
            var resObj = JsonSerializer.Deserialize<JEToken>(resBody);

            if (resObj == null && res.IsSuccessStatusCode)
                throw new JEAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}");
            else if (resObj == null)
                throw new JEAuthException("The response was null.");

            resObj.ExpiresOn = DateTime.UtcNow.AddSeconds(resObj.ExpiresIn);
            return resObj;
        }
        catch (Exception ex)
        {
            throw ExceptionHelper.CreateException(ex, resBody, res);
        }
    }
}
using System.Text.Json;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JEGameOwnershipChecker : IAuthenticator
{
    private readonly ISessionSource<JEToken> _sessionSource;

    public JEGameOwnershipChecker(ISessionSource<JEToken> sessionSource) =>
        _sessionSource = sessionSource;

    public async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        context.Logger.LogJEGameOwnershipChecker();
        var token = _sessionSource.Get(context.SessionStorage);
        
        if (string.IsNullOrEmpty(token?.AccessToken))
            throw new JEAuthException("JEToken.AccessToken was empty. JETokenAuthenticator must run first.");

        var result = await checkGameOwnership(context.HttpClient, token.AccessToken);
        if (!result)
            throw new JEAuthException("The user doesn't own the game.");
    }

    private async ValueTask<bool> checkGameOwnership(HttpClient httpClient, string token)
    {
        var req = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.minecraftservices.com/entitlements/mcstore"),
        };
        req.Headers.Add("Authorization", "Bearer " + token);

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
}
using System.Text.Json;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JEProfileAuthenticator : SessionAuthenticator<JEProfile>
{
    private readonly ISessionSource<JEToken> _jeSessionSource;

    public JEProfileAuthenticator(
        ISessionSource<JEToken> jeSessionSource,
        ISessionSource<JEProfile> sessionSource)
        : base(sessionSource) =>
        _jeSessionSource = jeSessionSource;

    protected override async ValueTask<JEProfile?> Authenticate(AuthenticateContext context)
    {
        context.Logger.LogJEProfileAuthenticator();

        var token = _jeSessionSource.Get(context.SessionStorage);
        if (string.IsNullOrEmpty(token?.AccessToken))
            throw new JEAuthException("JEToken.AccessToken was empty. JETokenAuthenticator must run first.");

        var profile = await requestProfile(token.AccessToken, context.HttpClient);
        if (string.IsNullOrEmpty(profile.UUID))
            throw new JEAuthException("The Mojang server returned empty UUID.");
        if (string.IsNullOrEmpty(profile.Username))
            throw new JEAuthException("The Mojang server returned empty username.");

        return profile;
    }

    private async ValueTask<JEProfile> requestProfile(string token, HttpClient httpClient)
    {
        var req = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.minecraftservices.com/minecraft/profile")
        };
        req.Headers.Add("Authorization", "Bearer " + token);

        var res = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
        var resBody = await res.Content.ReadAsStringAsync();

        try
        {
            res.EnsureSuccessStatusCode();
            using var json = JsonDocument.Parse(resBody);
            return JEProfile.ParseFromJson(json.RootElement);
        }
        catch (Exception ex)
        {
            throw ExceptionHelper.CreateException(ex, resBody, res);
        }
    }
}
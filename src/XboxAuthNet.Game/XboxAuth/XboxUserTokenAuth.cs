using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxUserTokenAuth : SessionAuthenticator<XboxAuthTokens>
{
    private readonly ISessionSource<MicrosoftOAuthResponse> _oauthSessionSource;

    public XboxUserTokenAuth(
        ISessionSource<MicrosoftOAuthResponse> oAuthSessionSource,
        ISessionSource<XboxAuthTokens> sessionSource)
        : base(sessionSource) =>
        _oauthSessionSource = oAuthSessionSource;

    protected override async ValueTask<XboxAuthTokens?> Authenticate()
    {
        var oAuthAccessToken = _oauthSessionSource
            .Get(Context.SessionStorage)?
            .AccessToken;

        if (string.IsNullOrEmpty(oAuthAccessToken))
            throw new XboxAuthException("OAuth access token was empty", 0);

        var xboxAuthClient = new XboxAuthClient(Context.HttpClient);
        var userToken = await xboxAuthClient.RequestUserToken(oAuthAccessToken);

        var tokens = GetSessionFromStorage() ?? new XboxAuthTokens();
        tokens.UserToken = userToken;
        return tokens;
    }
}
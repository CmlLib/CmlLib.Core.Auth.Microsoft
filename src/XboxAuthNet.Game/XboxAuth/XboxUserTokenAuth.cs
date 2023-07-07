using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
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

    protected override async ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        var oAuthAccessToken = _oauthSessionSource
            .Get(context.SessionStorage)?
            .AccessToken;

        if (string.IsNullOrEmpty(oAuthAccessToken))
            throw new XboxAuthException("OAuth access token was empty. Microsoft OAuth is required.", 0);

        context.Logger.LogXboxUserTokenAuth();
        var xboxAuthClient = new XboxAuthClient(context.HttpClient);
        var userToken = await xboxAuthClient.RequestUserToken(oAuthAccessToken);

        var tokens = GetSessionFromStorage() ?? new XboxAuthTokens();
        tokens.UserToken = userToken;
        return tokens;
    }
}
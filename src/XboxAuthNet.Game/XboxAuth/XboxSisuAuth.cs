using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Requests;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive.Crypto;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxSisuAuth : SessionAuthenticator<XboxAuthTokens>
{
    private readonly string _clientId;
    private readonly string _tokenPrefix;
    private readonly string _relyingParty;
    private readonly ISessionSource<MicrosoftOAuthResponse> _oAuthSessionSource;
    private readonly IXboxRequestSigner _signer;

    public XboxSisuAuth(
        string clientId,
        string tokenPrefix,
        string relyingParty,
        IXboxRequestSigner signer,
        ISessionSource<MicrosoftOAuthResponse> oauthSessionSource,
        ISessionSource<XboxAuthTokens> sessionSource)
        : base(sessionSource) =>
        (_clientId, _tokenPrefix, _relyingParty, _signer, _oAuthSessionSource) = 
        (clientId, tokenPrefix, relyingParty, signer, oauthSessionSource);

    protected override async ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        context.Logger.LogXboxSisu(_relyingParty);

        var oAuthAccessToken = _oAuthSessionSource
            .Get(context.SessionStorage)?
            .AccessToken;

        if (string.IsNullOrEmpty(oAuthAccessToken))
            throw new XboxAuthException("OAuth access token was empty. Microsoft OAuth is required.", 0);

        var xboxTokens = GetSessionFromStorage() ?? new XboxAuthTokens();

        var xboxAuthClient = new XboxSignedClient(_signer, context.HttpClient);
        var sisuResponse = await xboxAuthClient.SisuAuth(new XboxSisuAuthRequest
        {
            AccessToken = oAuthAccessToken,
            DeviceToken = xboxTokens.DeviceToken?.Token,
            TokenPrefix = _tokenPrefix,
            ClientId = _clientId,
            RelyingParty = _relyingParty,
        });

        return new XboxAuthTokens
        {
            UserToken = sisuResponse.UserToken,
            DeviceToken = xboxTokens.DeviceToken,
            TitleToken = sisuResponse.TitleToken,
            XstsToken = sisuResponse.AuthorizationToken,
        };
    }
}

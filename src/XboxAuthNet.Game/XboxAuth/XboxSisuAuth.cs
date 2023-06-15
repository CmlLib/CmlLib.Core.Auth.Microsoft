using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Requests;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxSisuAuth : SessionAuthenticator<XboxAuthTokens>
{
    private readonly string _clientId;
    private readonly string _tokenPrefix;
    private readonly string _relyingParty;

    public XboxSisuAuth(
        string clientId,
        string tokenPrefix,
        string relyingParty,
        ISessionSource<XboxAuthTokens> sessionSource)
        : base(sessionSource) =>
        (_clientId, _tokenPrefix, _relyingParty) = (clientId, tokenPrefix, relyingParty);

    protected override async ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        var xboxTokens = GetSessionFromStorage() ?? new XboxAuthTokens();

        context.Logger.LogXboxSisu();
        var xboxAuthClient = new XboxAuthClient(context.HttpClient);
        var sisuResponse = await xboxAuthClient.SisuAuth(new XboxSisuAuthRequest
        {
            AccessToken = xboxTokens.UserToken?.Token,
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

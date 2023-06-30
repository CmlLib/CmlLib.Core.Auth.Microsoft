using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.XboxLive;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxXuiClaimsAuth : SessionAuthenticator<XboxAuthTokens>
{
    public XboxXuiClaimsAuth(ISessionSource<XboxAuthTokens> sessionSource) : 
        base(sessionSource)
    {

    }

    protected async override ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        var xboxAuthTokens = GetSessionFromStorage() ?? new XboxAuthTokens();

        var tempXboxAuthToken = new XboxAuthTokens
        {
            UserToken = xboxAuthTokens.UserToken
        };
        var tempSessionSource = new InMemorySessionSource<XboxAuthTokens>(tempXboxAuthToken);

        var xstsAuth = new XboxXstsTokenAuth(XboxAuthConstants.XboxLiveRelyingParty, tempSessionSource);
        await xstsAuth.ExecuteAsync(context);
        tempXboxAuthToken = tempSessionSource.Get(context.SessionStorage);

        xboxAuthTokens.XstsToken ??= new XboxLive.Responses.XboxAuthResponse();
        xboxAuthTokens.XstsToken.XuiClaims = tempXboxAuthToken?.XstsToken?.XuiClaims;
        return xboxAuthTokens;
    }
}
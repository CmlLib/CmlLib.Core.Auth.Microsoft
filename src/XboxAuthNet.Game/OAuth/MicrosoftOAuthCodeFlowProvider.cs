using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthCodeFlowProvider : IAuthenticationProvider
{
    private readonly MicrosoftOAuthBuilder _oauth;

    public MicrosoftOAuthCodeFlowProvider(MicrosoftOAuthClientInfo clientInfo)
    {
        _oauth = new MicrosoftOAuthBuilder(clientInfo);
    }

    public IAuthenticator Authenticate() => _oauth.CodeFlow();
    public IAuthenticator AuthenticateInteractively() => _oauth.Interactive();
    public IAuthenticator AuthenticateSilently() => _oauth.Silent();
    public IAuthenticator ClearSession() => _oauth.Signout();
    public IAuthenticator Signout() => _oauth.SignoutWithBrowser();
    public ISessionValidator CreateSessionValidator() => _oauth.Validator();
}
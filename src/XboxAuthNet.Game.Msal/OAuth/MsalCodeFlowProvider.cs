using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalCodeFlowProvider : IAuthenticationProvider
{
    private readonly MsalOAuthBuilder _builder;

    public MsalCodeFlowProvider(IPublicClientApplication app)
    {
        _builder = new MsalOAuthBuilder(app);
    }

    public MsalCodeFlowProvider(MsalOAuthBuilder builder)
    {
        _builder = builder;
    }

    public IAuthenticator Authenticate() => _builder.CodeFlow();
    public IAuthenticator AuthenticateInteractively() => _builder.Interactive();
    public IAuthenticator AuthenticateSilently() => _builder.Silent();
    public ISessionValidator CreateSessionValidator() => StaticValidator.Invalid;
    public IAuthenticator ClearSession() => _builder.ClearSession();
    public IAuthenticator Signout() => _builder.ClearSession();
}

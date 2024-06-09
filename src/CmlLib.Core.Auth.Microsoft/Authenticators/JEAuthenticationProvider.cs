using XboxAuthNet.Game;
using XboxAuthNet.Game.Authenticators;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JEAuthenticationProvider : IAuthenticationProvider
{
    private readonly JEAuthenticatorBuilder _builder;

    public JEAuthenticationProvider()
    {
        _builder = new JEAuthenticatorBuilder();
    }

    public JEAuthenticationProvider(JEAuthenticatorBuilder builder)
    {
        _builder = builder;
    }

    public IAuthenticator Authenticate() => _builder.Build();
    public IAuthenticator AuthenticateInteractively() => _builder.Build();
    public IAuthenticator AuthenticateSilently() => _builder.Build();
    public IAuthenticator ClearSession() => _builder.SessionCleaner();
    public ISessionValidator CreateSessionValidator() => _builder.TokenValidator();
    public IAuthenticator Signout() => _builder.SessionCleaner();
}
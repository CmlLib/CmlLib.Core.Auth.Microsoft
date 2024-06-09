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

    public IAuthenticator CreateInteractiveAuthenticator() => _builder.Build();
    public ISessionValidator CreateSessionValidatorForInteractiveAuthenticator() => StaticValidator.Invalid;
    public ISessionValidator CreateSessionValidatorForSilentAuthenticator() => _builder.TokenValidator();
    public IAuthenticator CreateSilentAuthenticator() => _builder.Build();
    public IAuthenticator Signout() => _builder.SessionCleaner();
}
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game;

public class CompositeAuthenticationProvider : IAuthenticationProvider
{
    private readonly List<IAuthenticationProvider> _providers = new();
    private readonly Func<ICompositeAuthenticator> _authenticatorFactory;

    public CompositeAuthenticationProvider(Func<ICompositeAuthenticator> authenticatorFactory)
    {
        _authenticatorFactory = authenticatorFactory;
    }

    public void AddProvider(IAuthenticationProvider provider) => _providers.Add(provider);

    public IAuthenticator CreateInteractiveAuthenticator()
    {
        var authenticator = _authenticatorFactory();
        foreach (var provider in _providers)
        {
            authenticator.AddAuthenticator(
                provider.CreateSessionValidatorForInteractiveAuthenticator(), 
                provider.CreateInteractiveAuthenticator());
        }
        return authenticator;
    }

    public ISessionValidator CreateSessionValidatorForInteractiveAuthenticator()
    {
        return _providers
            .Select(provider => provider.CreateSessionValidatorForInteractiveAuthenticator())
            .LastOrDefault()
            ?? StaticValidator.Valid;
    }

    public IAuthenticator CreateSilentAuthenticator()
    {
        var authenticator = _authenticatorFactory();
        foreach (var provider in _providers)
        {
            authenticator.AddAuthenticator(
                provider.CreateSessionValidatorForSilentAuthenticator(),
                provider.CreateSilentAuthenticator());
        }
        return authenticator;
    }

    public ISessionValidator CreateSessionValidatorForSilentAuthenticator()
    {
        return _providers
            .Select(provider => provider.CreateSessionValidatorForSilentAuthenticator())
            .LastOrDefault()
            ?? StaticValidator.Valid;
    }

    public IAuthenticator Signout()
    {
        var authenticator = _authenticatorFactory();
        foreach (var provider in _providers)
        {
            authenticator.AddAuthenticatorWithoutValidator(provider.Signout());
        }
        return authenticator;
    }
}
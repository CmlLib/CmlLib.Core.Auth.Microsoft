using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.XboxAuth;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JEAuthenticatorBuilder
{
    public bool CheckGameOwnership { get; set; } = false;

    private ISessionSource<XboxAuthTokens>? _xboxSessionSource;
    public ISessionSource<XboxAuthTokens> XboxSessionSource
    {
        get => _xboxSessionSource ??= XboxAuthNet.Game.XboxAuth.XboxSessionSource.Default;
        set => _xboxSessionSource = value;
    }

    private ISessionSource<JEToken>? _tokenSource;
    public ISessionSource<JEToken> TokenSource
    {
        get => _tokenSource ??= JETokenSource.Default;
        set => _tokenSource = value;
    }

    private ISessionSource<JEProfile>? _profileSource;
    public ISessionSource<JEProfile> ProfileSource
    {
        get => _profileSource ??= JEProfileSource.Default;
        set => _profileSource = value;
    }

    public JEAuthenticatorBuilder WithGameOwnershipChecker() => 
        WithGameOwnershipChecker(true);
        
    public JEAuthenticatorBuilder WithGameOwnershipChecker(bool value)
    {
        CheckGameOwnership = value;
        return this;
    }

    public JEAuthenticatorBuilder WithXboxSessionSource(ISessionSource<XboxAuthTokens> sessionSource)
    {
        XboxSessionSource = sessionSource;
        return this;
    }

    public JEAuthenticatorBuilder WithProfileSource(ISessionSource<JEProfile> profileSource)
    {
        ProfileSource = profileSource;
        return this;
    }

    public JEAuthenticatorBuilder WithTokenSource(ISessionSource<JEToken> tokenSource)
    {
        TokenSource = tokenSource;
        return this;
    }

    public ISessionValidator ProfileValidator() =>
        new JEProfileValidator(ProfileSource);

    public ISessionValidator TokenValidator() =>
        new JETokenValidator(TokenSource);

    public IAuthenticator TokenAuthenticator() =>
        new JETokenAuthenticator(XboxSessionSource, TokenSource);

    public IAuthenticator ProfileAuthenticator() =>
        new JEProfileAuthenticator(TokenSource, ProfileSource);
    
    public IAuthenticator GameOwnershipChecker() =>
        new JEGameOwnershipChecker(TokenSource);

    public IAuthenticator Build()
    {
        var collection = new AuthenticatorCollection();
        collection.AddAuthenticator(StaticValidator.Invalid, TokenAuthenticator());
        collection.AddAuthenticator(StaticValidator.Invalid, ProfileAuthenticator());
        return collection;
    }
}
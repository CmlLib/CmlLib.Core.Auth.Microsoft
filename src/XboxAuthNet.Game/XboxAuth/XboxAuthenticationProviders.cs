using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.XboxAuth;

public abstract class XboxAuthenticationProviderBase : IAuthenticationProvider
{
    protected XboxAuthBuilder Builder { get; }

    public XboxAuthenticationProviderBase(string relyingParty)
    {
        Builder = new XboxAuthBuilder();
        Builder.WithRelyingParty(relyingParty);
    }

    public XboxAuthenticationProviderBase(XboxAuthBuilder builder)
    {
        Builder = builder;
    }

    protected abstract IAuthenticator CreateAuthenticator();

    public IAuthenticator Authenticate() => CreateAuthenticator();
    public IAuthenticator AuthenticateSilently() => CreateAuthenticator();
    public IAuthenticator AuthenticateInteractively() => CreateAuthenticator();
    public ISessionValidator CreateSessionValidator() => Builder.Validator();
    public IAuthenticator ClearSession() => Builder.ClearSession();
    public IAuthenticator Signout() => Builder.ClearSession();
}

public class BasicXboxProvider : XboxAuthenticationProviderBase
{
    public BasicXboxProvider(string relyingParty) : base(relyingParty)
    {
    }

    public BasicXboxProvider(XboxAuthBuilder builder) : base(builder)
    {
    }

    protected override IAuthenticator CreateAuthenticator() => Builder.Basic();
}

public class FullXboxProvider : XboxAuthenticationProviderBase
{
    public FullXboxProvider(string relyingParty) : base(relyingParty)
    {
    }

    public FullXboxProvider(XboxAuthBuilder builder) : base(builder)
    {
    }

    protected override IAuthenticator CreateAuthenticator() => Builder.Full();
}

public class SisuXboxProvider : XboxAuthenticationProviderBase
{
    private readonly string _clientId;

    public SisuXboxProvider(string relyingParty, string clientId) : base(relyingParty)
    {
        _clientId = clientId;
    }

    public SisuXboxProvider(XboxAuthBuilder builder, string clientId) : base(builder)
    {
        _clientId = clientId;
    }

    protected override IAuthenticator CreateAuthenticator() => Builder.Sisu(_clientId);
}
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

    public IAuthenticator CreateInteractiveAuthenticator() => CreateAuthenticator();
    public IAuthenticator CreateSilentAuthenticator() => CreateAuthenticator();
    public ISessionValidator CreateSessionValidatorForInteractiveAuthenticator() => StaticValidator.Invalid;
    public ISessionValidator CreateSessionValidatorForSilentAuthenticator() => Builder.Validator();
    public IAuthenticator Signout() => Builder.ClearSession();

    protected abstract IAuthenticator CreateAuthenticator();
}

public class BasicXbox : XboxAuthenticationProviderBase
{
    public BasicXbox(string relyingParty) : base(relyingParty)
    {
    }

    public BasicXbox(XboxAuthBuilder builder) : base(builder)
    {
    }

    protected override IAuthenticator CreateAuthenticator() => Builder.Basic();
}

public class FullXbox : XboxAuthenticationProviderBase
{
    public FullXbox(string relyingParty) : base(relyingParty)
    {
    }

    public FullXbox(XboxAuthBuilder builder) : base(builder)
    {
    }

    protected override IAuthenticator CreateAuthenticator() => Builder.Full();
}

public class SisuXbox : XboxAuthenticationProviderBase
{
    private readonly string _clientId;

    public SisuXbox(string relyingParty, string clientId) : base(relyingParty)
    {
        _clientId = clientId;
    }

    public SisuXbox(XboxAuthBuilder builder, string clientId) : base(builder)
    {
        _clientId = clientId;
    }

    protected override IAuthenticator CreateAuthenticator() => Builder.Sisu(_clientId);
}
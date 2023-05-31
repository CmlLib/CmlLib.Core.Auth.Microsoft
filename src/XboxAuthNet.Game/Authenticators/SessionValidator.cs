using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public abstract class SessionValidator<T> : ISessionValidator
{
    private readonly ISessionSource<T> _sessionSource;

    public SessionValidator(ISessionSource<T> sessionSource) 
    {
        _sessionSource = sessionSource;
    }

    public ValueTask<bool> Validate(AuthenticateContext context)
    {
        var session = _sessionSource.Get(context.SessionStorage);
        if (session == null)
            return new ValueTask<bool>(false);
        return Validate(context, session);
    }

    protected abstract ValueTask<bool> Validate(AuthenticateContext context, T session);
}
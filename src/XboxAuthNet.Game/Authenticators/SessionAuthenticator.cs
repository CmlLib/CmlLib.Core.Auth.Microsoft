using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public abstract class SessionAuthenticator<T> : IAuthenticator
{
    public ISessionSource<T> SessionSource { get; private set; }
    private AuthenticateContext? _context;

    public SessionAuthenticator(ISessionSource<T> sessionSource)
    {
        SessionSource = sessionSource;
    }
    
    public async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        _context = context;
        var result = await Authenticate(context);
        SessionSource.Set(context.SessionStorage, result);
    }

    protected abstract ValueTask<T?> Authenticate(AuthenticateContext context);

    protected T? GetSessionFromStorage()
    {
        if (_context == null)
            throw new InvalidOperationException("Call ExecuteAsync() first");
        return SessionSource.Get(_context.SessionStorage);
    }
}
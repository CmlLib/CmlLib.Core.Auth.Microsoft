using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public abstract class SessionAuthenticator<T> : IAuthenticator
{
    public ISessionSource<T> SessionSource { get; private set; }
    public AuthenticateContext Context { get; private set; }

    public SessionAuthenticator(ISessionSource<T> sessionSource)
    {
        SessionSource = sessionSource;
        Context = null!;
        SessionSource = null!;
    }

    public async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        Context = context;
        var result = await Authenticate();
        SessionSource.Set(context.SessionStorage, result);
    }

    protected abstract ValueTask<T?> Authenticate();

    protected T? GetSessionFromStorage()
    {
        return SessionSource.Get(Context.SessionStorage);
    }
}
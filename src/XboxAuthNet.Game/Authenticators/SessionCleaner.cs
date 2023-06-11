using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public class SessionCleaner<T> : SessionAuthenticator<T>
{
    public SessionCleaner(ISessionSource<T> sessionSource)
     : base(sessionSource)
    {

    }

    protected override ValueTask<T?> Authenticate(AuthenticateContext context)
    {
        return default;
    }
}
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Test.Authenticators;

public class ThrowingAuthenticator : IAuthenticator
{
    public ValueTask ExecuteAsync(AuthenticateContext context)
    {
        throw new TaskCanceledException();
    }
}
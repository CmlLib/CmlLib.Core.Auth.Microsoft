using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Test.Authenticators;

public class ThrowingAuthenticator : IAuthenticator
{
    private readonly Exception _ex;

    public ThrowingAuthenticator(Exception ex) => 
        _ex = ex;

    public ValueTask ExecuteAsync(AuthenticateContext context)
    {
        throw _ex;
    }
}
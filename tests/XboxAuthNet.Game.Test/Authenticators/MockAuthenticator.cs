using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Test.Authenticators;

public class MockAuthenticator : IAuthenticator
{
    public MockAuthenticator(bool expectedToBeExecuted)
    {
        ExpectedToBeExecuted = expectedToBeExecuted;
    }

    public bool ExpectedToBeExecuted { get; private set; }
    public bool IsExecuted { get; private set; } = false;

    public ValueTask ExecuteAsync(AuthenticateContext context)
    {
        IsExecuted = true;
        return new ValueTask();
    }
}
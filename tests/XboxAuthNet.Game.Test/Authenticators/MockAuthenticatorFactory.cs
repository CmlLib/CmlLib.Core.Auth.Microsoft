using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Authenticators;

public class MockAuthenticatorFactory
{
    private List<MockAuthenticator> authenticators = new();

    public IAuthenticator ExpectToBeExecuted()
    {
        var authenticator = new MockAuthenticator(true);
        authenticators.Add(authenticator);
        return authenticator;
    }

    public IAuthenticator ExpectToNotBeExecuted()
    {
        var authenticator = new MockAuthenticator(false);
        authenticators.Add(authenticator);
        return authenticator;
    }

    public IAuthenticator Throw() =>
        Throw(new TaskCanceledException());

    public IAuthenticator Throw(Exception ex)
    {
        var authenticator = new ThrowingAuthenticator(ex);
        return authenticator;
    }

    public void TestExpectations()
    {
        string toStr(bool v) => v ? "EXECUTED" : "NOT EXECUTED";

        var failIndex = authenticators.FindIndex(x => (x.IsExecuted != x.ExpectedToBeExecuted));
        if (failIndex > -1)
        {
            Assert.Fail($"Unexpected behaviour. Index number: {failIndex}, " + 
                        $"Expected: {toStr(authenticators[failIndex].ExpectedToBeExecuted)}, " + 
                        $"Actual: {toStr(authenticators[failIndex].IsExecuted)}");
        }
    }

    public AuthenticateContext CreateContext() => 
        CreateContext(new InMemorySessionStorage());

    public AuthenticateContext CreateContext(ISessionStorage sessionStorage)
    {
        return new AuthenticateContext(
            sessionStorage, 
            null!, 
            default, 
            NullLogger.Instance);
    }
}
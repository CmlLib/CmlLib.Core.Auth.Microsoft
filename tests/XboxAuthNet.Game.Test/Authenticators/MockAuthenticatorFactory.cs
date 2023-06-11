using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;

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

    public IAuthenticator Throw()
    {
        var authenticator = new ThrowingAuthenticator();
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
}
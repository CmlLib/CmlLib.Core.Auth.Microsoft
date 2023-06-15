using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Authenticators;

[TestFixture]
public class FallbackAuthenticatorTest
{
    [Test]
    public async Task TestFallback()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw());
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());

        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestThrowAndSkip()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw());
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());

        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestSkipValidAuthenticator()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        
        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestSkipAll()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.Throw());
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        
        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }
}
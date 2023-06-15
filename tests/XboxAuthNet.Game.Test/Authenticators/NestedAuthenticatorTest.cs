using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Test.Authenticators;

[TestFixture]
public class NestedAuthenticatorTest
{
    [Test]
    public async Task TestMixedAuthenticator1()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new NestedAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToNotBeExecuted());
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
    
        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestMixedAuthenticator2()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new AuthenticatorCollection();
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
    
        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestValidAuthenticator()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new NestedAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
    
        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestInvalidAuthenticator()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new AuthenticatorCollection();
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        authenticator.AddPostAuthenticator(mocks.ExpectToBeExecuted());
    
        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }
}
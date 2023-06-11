using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Authenticators;

[TestFixture]
public class AuthenticatorCollectionTest
{
    [Test]
    public async Task TestIsAllAuthenticatorExecuted()
    {
        var mocks = new MockAuthenticatorFactory();
        var collection = new AuthenticatorCollection();
        collection.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        collection.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        collection.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        collection.AddPostAuthenticator(mocks.ExpectToBeExecuted());

        await collection.ExecuteAsync(createMockContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestValidAuthenticator()
    {
        var mocks = new MockAuthenticatorFactory();
        var collection = new AuthenticatorCollection();
        collection.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        collection.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        collection.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        collection.AddPostAuthenticator(mocks.ExpectToBeExecuted());

        await collection.ExecuteAsync(createMockContext());
        mocks.TestExpectations();
    }

    [Test]
    public async Task TestMixedAuthenticator()
    {
        var mocks = new MockAuthenticatorFactory();
        var collection = new AuthenticatorCollection();
        collection.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        collection.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        collection.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());
        collection.AddAuthenticator(StaticValidator.Valid, mocks.ExpectToNotBeExecuted());
        collection.AddPostAuthenticator(mocks.ExpectToBeExecuted());
        collection.AddPostAuthenticator(mocks.ExpectToBeExecuted());

        await collection.ExecuteAsync(createMockContext());
        mocks.TestExpectations();
    }

    private static AuthenticateContext createMockContext()
    {
        return new AuthenticateContext(
            new InMemorySessionStorage(), 
            null!, // mock authenticator does not need real HttpClient 
            default);
    }
}
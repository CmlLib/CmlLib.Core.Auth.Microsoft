using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;

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

    [Test]
    public void TestThrowing()
    {
        var ex1 = new ArgumentException();
        var ex2 = new FormatException();
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw(ex1));
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw(ex2));
        
        var ex = Assert.ThrowsAsync<AggregateException>(async () => 
        {
            await authenticator.ExecuteAsync(mocks.CreateContext());
        });
        Assert.That(ex?.InnerExceptions.ToArray(), Is.EqualTo(new Exception[] { ex2, ex1 }));
    }

    [Test]
    public async Task TestCatchExceptionType()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator(new[] { typeof(IOException) });
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw(new IOException()));
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw(new FileNotFoundException()));
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToBeExecuted());

        await authenticator.ExecuteAsync(mocks.CreateContext());
        mocks.TestExpectations();
    }

    [Test]
    public void TestThrowExceptionType()
    {
        var mocks = new MockAuthenticatorFactory();
        var authenticator = new FallbackAuthenticator(new[] { typeof(FileNotFoundException) });
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.Throw(new IOException()));
        authenticator.AddAuthenticator(StaticValidator.Invalid, mocks.ExpectToNotBeExecuted());

        Assert.ThrowsAsync<IOException>(async () =>
        {
            await authenticator.ExecuteAsync(mocks.CreateContext());
        });
        mocks.TestExpectations();
    } 
}
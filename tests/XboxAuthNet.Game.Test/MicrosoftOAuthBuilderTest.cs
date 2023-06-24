using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class MicrosoftOAuthBuilderTest
{
    [Test]
    public void TestInteractive()
    {
        var builder = createBuilder();
        var authenticator = builder.Interactive();
        Assert.That(authenticator, Is.InstanceOf<InteractiveMicrosoftOAuth>());
    }

    [Test]
    public void TestSilent()
    {
        var builder = createBuilder();
        var authenticator = builder.Silent();
        Assert.That(authenticator, Is.InstanceOf<SilentMicrosoftOAuth>());
    }

    [Test]
    public void TestValidator()
    {
        var builder = createBuilder();
        var authenticator = builder.Validator();
        Assert.That(authenticator, Is.InstanceOf<MicrosoftOAuthValidator>());
    }

    public void TestSignout()
    {
        var builder = createBuilder();
        var authenticator = builder.Signout();
        Assert.That(authenticator, Is.InstanceOf<MicrosoftOAuthSignout>());
    }

    public void TestFromMicrosoftOAuthResponse()
    {
        var builder = createBuilder();
        var authenticator = builder.FromMicrosoftOAuthResponse(new MicrosoftOAuthResponse());
        Assert.That(authenticator, Is.InstanceOf<StaticSessionAuthenticator<MicrosoftOAuthResponse>>());
    }

    public MicrosoftOAuthBuilder createBuilder()
    {
        return new MicrosoftOAuthBuilder(new MicrosoftOAuthClientInfo("ClientId", "Scopes"));
    }
}
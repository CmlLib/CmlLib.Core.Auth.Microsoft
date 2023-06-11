using NUnit.Framework;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Authenticators;

[TestFixture]
public class StaticSessionAuthenticatorTest
{
    [Test]
    public async Task Test()
    {
        var testObject = new object();
        var sessionStorage = new InMemorySessionStorage();
        var sessionSource = new SessionFromStorage<object>("testKey");

        var authenticator = new StaticSessionAuthenticator<object>(testObject, sessionSource);
        await authenticator.ExecuteAsync(new AuthenticateContext(sessionStorage, null!, default));

        var stored = sessionSource.Get(sessionStorage);
        Assert.That(stored, Is.EqualTo(testObject));
    }
}
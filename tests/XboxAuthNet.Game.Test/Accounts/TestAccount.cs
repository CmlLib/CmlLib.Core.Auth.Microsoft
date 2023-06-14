using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Accounts;

public class TestAccount : XboxGameAccount
{
    public static TestAccount Create(string identifier) =>
        Create(identifier, DateTime.MinValue);

    public static TestAccount Create(string identifier, DateTime lastAccess)
    {
        var sessionStorage = new InMemorySessionStorage();
        sessionStorage.Set("identifier", identifier);
        LastAccessSource.Default.Set(sessionStorage, lastAccess);
        return new TestAccount(sessionStorage);
    }

    public TestAccount(ISessionStorage sessionStorage)
    : base(sessionStorage)
    {

    }

    protected override string? GetIdentifier()
    {
        return SessionStorage.Get<string>("identifier");
    }
}
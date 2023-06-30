using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Accounts;

public class TestAccount : XboxGameAccount
{
    public static TestAccount Create(string identifier) =>
        createInternal(identifier, DateTime.MinValue);

    public static TestAccount Create(string identifier, DateTime lastAccess) =>
        createInternal(identifier, lastAccess);

    public static TestAccount CreateNull() =>
        createInternal(null, DateTime.MinValue);

    private static TestAccount createInternal(string? identifier, DateTime lastAccess)
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
        SessionStorage.TryGetValue<string?>("identifier", out var value);
        return value;
    }
}
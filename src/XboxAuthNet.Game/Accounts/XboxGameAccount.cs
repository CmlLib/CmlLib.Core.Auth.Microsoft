using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuth;

namespace XboxAuthNet.Game.Accounts;

public class XboxGameAccount : IXboxGameAccount
{
    public static XboxGameAccount FromSessionStorage(ISessionStorage sessionStorage)
    {
        return new XboxGameAccount(sessionStorage);
    }

    public XboxGameAccount(ISessionStorage sessionStorage)
    {
        this.SessionStorage = sessionStorage;
    }

    public string? Identifier => GetIdentifier();
    public ISessionStorage SessionStorage { get; }
    public XboxAuthTokens? XboxTokens => XboxSessionSource.Default.Get(SessionStorage);
    public string? Gamertag => XboxTokens?.XstsToken?.XuiClaims?.Gamertag;
    public DateTime LastAccess => LastAccessSource.Default.Get(SessionStorage);

    protected virtual string? GetIdentifier()
    {
        var uhs = XboxTokens?.XstsToken?.XuiClaims?.UserHash;
        return uhs;
    }

    public int CompareTo(object? other)
    {
        // this < other: -1
        // this == other: 0
        // this > other: 1

        // IMPORTANT: non-null is greater than null
        // if not, Array.Sort will not work as expected
        if (other is null)
            return 1;

        if (other is not XboxGameAccount account)
            return -1;

        // last access is the highest priority
        var lastAccessCompare = -LastAccess.CompareTo(account.LastAccess);
        if (lastAccessCompare != 0)
        {
            return lastAccessCompare;
        }

        // if last access is the same, compare identifier
        // alphabetically
        var thisIdentifier = Identifier ?? "";
        var otherIdentifier = account.Identifier ?? "";
        return thisIdentifier.CompareTo(otherIdentifier);
    }

    public override bool Equals(object? obj)
    {
        if (obj is XboxGameAccount account)
        {
            return account.Identifier == Identifier;
        }
        else if (obj is string)
        {
            return obj.Equals(Identifier);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return Identifier?.GetHashCode() ?? 0;
    }

    public override string ToString()
    {
        return Identifier ?? string.Empty;
    }
}
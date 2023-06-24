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
    public DateTime LastAccess => LastAccessSource.Default.Get(SessionStorage);

    protected virtual string? GetIdentifier()
    {
        var uhs = XboxTokens?.XstsToken?.XuiClaims?.UserHash;
        return uhs;
    }

    public int CompareTo(object? other)
    {
        // -1: this instance precedes other
        //  0: same position
        //  1: this instance follows other or other is not a valid object

        if (other is not XboxGameAccount account)
            return 1;

        if (Equals(other))
            return LastAccess.CompareTo(account.LastAccess);
        else
        {
            var thisIdentifier = Identifier ?? "";
            var otherIdentifier = account.Identifier ?? "";
            return thisIdentifier.CompareTo(otherIdentifier);
        }
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
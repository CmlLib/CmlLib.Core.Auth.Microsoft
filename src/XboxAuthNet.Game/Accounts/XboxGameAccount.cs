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
        var uhs = XboxTokens?.XstsToken?.UserHash;
        return uhs;
    }

    public int CompareTo(object? obj)
    {
        var account = obj as XboxGameAccount;
        if (account == null)
            return 1;
        
        return this.LastAccess.CompareTo(account.LastAccess);
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
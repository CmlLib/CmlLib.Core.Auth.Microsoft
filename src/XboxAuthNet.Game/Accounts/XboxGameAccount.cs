using System;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

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
    public DateTime LastAccess => getLastAccess();

    protected virtual string? GetIdentifier()
    {
        var xboxSessionSource = new XboxSessionSource(SessionStorage);
        var xboxSession = xboxSessionSource.Get();
        var uhs = xboxSession?.XstsToken?.UserHash;
        return uhs;
    }

    private DateTime getLastAccess()
    {
        var lastAccess = SessionStorage.GetOrDefault<string>("lastAccess", DateTime.MinValue.ToString());
        if (DateTime.TryParse(lastAccess, out var parsedLastAccess))
            return parsedLastAccess;
        else
            return DateTime.MinValue;
    }

    public int CompareTo(object obj)
    {
        var account = obj as XboxGameAccount;
        if (account == null)
            return 1;
        
        return this.LastAccess.CompareTo(account.LastAccess);
    }

    public override bool Equals(object obj)
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
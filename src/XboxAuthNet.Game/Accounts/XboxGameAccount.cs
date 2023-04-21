using System;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Accounts;

public class XboxGameAccount
{
    public XboxGameAccount(ISessionStorage sessionStorage)
    {
        this.SessionStorage = sessionStorage;
    }

    public string? Identifier => GetIdentifier();
    public ISessionStorage SessionStorage { get; }
    public DateTime LastAccess => getLastAccess();

    protected virtual string? GetIdentifier()
    {
        return SessionStorage.GetHashCode().ToString();
    }

    private DateTime getLastAccess()
    {
        var lastAccess = SessionStorage.GetOrDefault<string>("lastAccess", DateTime.MinValue.ToString());
        if (DateTime.TryParse(lastAccess, out var parsedLastAccess))
            return parsedLastAccess;
        else
            return DateTime.MinValue;
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
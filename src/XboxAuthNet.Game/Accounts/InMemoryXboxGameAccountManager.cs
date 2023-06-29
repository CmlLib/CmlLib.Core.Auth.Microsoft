using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Accounts;

public class InMemoryXboxGameAccountManager : IXboxGameAccountManager
{
    private Func<ISessionStorage, IXboxGameAccount> _converter;

    public InMemoryXboxGameAccountManager(Func<ISessionStorage, IXboxGameAccount> converter)
    {
        _converter = converter;
        Accounts = new XboxGameAccountCollection();
    }

    public XboxGameAccountCollection Accounts { get; private set; }

    public XboxGameAccountCollection GetAccounts() => Accounts;

    public IXboxGameAccount GetDefaultAccount()
    {
        var account = Accounts.FirstOrDefault();
        return account ?? NewAccount();
    }

    public IXboxGameAccount NewAccount()
    {
        var sessionStorage = new InMemorySessionStorage();
        var account = _converter.Invoke(sessionStorage);
        return account;
    }

    public void ClearAccounts()
    {
        Accounts.Clear();
    }

    public void LoadAccounts()
    {
        // Accounts are already in memory
    }
    
    public void SaveAccounts()
    {
        foreach (var account in Accounts)
        {
            removeNoStore(account.SessionStorage);
        }
    }

    private void removeNoStore(ISessionStorage sessionStorage)
    {
        foreach (var key in sessionStorage.Keys)
        {
            if (sessionStorage.GetKeyMode(key) == SessionStorageKeyMode.NoStore)
                sessionStorage.Remove(key);
        }
    }
}
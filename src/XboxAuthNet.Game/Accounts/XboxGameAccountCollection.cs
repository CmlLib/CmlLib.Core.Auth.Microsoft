using System.Collections;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Accounts;

public class XboxGameAccountCollection : ICollection<IXboxGameAccount> 
{
    private readonly List<IXboxGameAccount> _accounts = new();

    public int Count => getAccounts().Count();
    public bool IsReadOnly => false;

    private IEnumerable<IXboxGameAccount> getAccounts()
    {
        // 1) Ignore accounts which has empty Identifier
        // 2) Remove duplicated accounts which has same Identifier
        // 2-1) If two accounts has same identifier, remove olds and take the most recent one
        // 3) Order by the most recently accessed account

        return _accounts
            .Where(account => !string.IsNullOrEmpty(account.Identifier))
            .GroupBy(account => account.Identifier)
            .Select(group => group.OrderByDescending(_ => _).First())
            .OrderBy(_ => _);
    }

    public IXboxGameAccount GetAccount(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
            throw new ArgumentNullException(nameof(identifier));

        var account = getAccount(identifier);
        if (account != null)
            return account;
        throw new KeyNotFoundException("Cannot find any account with the specified identifier: " + identifier);
    }

    public bool TryGetAccount(string identifier, out IXboxGameAccount account)
    {
        account = getAccount(identifier)!;
        return account != null;
    }

    private IXboxGameAccount? getAccount(string identifier)
    {
        return _accounts
            .Where(account => account.Identifier == identifier)
            .OrderByDescending(_ => _)
            .FirstOrDefault();
    }

    public void Add(IXboxGameAccount account)
    {
        _accounts.Add(account);
    }

    public bool Remove(IXboxGameAccount account)
    {
        return _accounts.Remove(account);
    }

    public bool RemoveAccount(string identifier)
    {
        var result = false;

        while (true)
        {
            var findIndex = _accounts.FindIndex(0, account => account.Identifier == identifier);
            if (findIndex == -1)
                break;

            _accounts.RemoveAt(findIndex);
            result = true;
        }

        return result;
    }

    public void Clear()
    {
        _accounts.Clear();
    }

    public bool Contains(IXboxGameAccount toFind)
    {
        return _accounts.Exists(account => account.Equals(toFind));
    }

    public void CopyTo(IXboxGameAccount[] array, int startIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "The start index cannot be negative.");
        
        var accounts = getAccounts().ToArray();
        if (startIndex + accounts.Length > array.Length)
            throw new ArgumentException("The number of elements in the source collection exceeds the available space in the array."); // generated by chatGPT
        
        Array.Copy(accounts, 0, array, startIndex, accounts.Length);
    }

    public IEnumerable<ISessionStorage> ToSessionStorages()
    {
        return getAccounts().Select(account => account.SessionStorage);
    }

    public IEnumerator<IXboxGameAccount> GetEnumerator()
    {
        return getAccounts().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
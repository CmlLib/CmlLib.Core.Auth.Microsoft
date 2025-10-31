using System.Collections;

namespace XboxAuthNet.Game.Accounts;

public class XboxGameAccountCollection : IReadOnlyCollection<IXboxGameAccount>
{
    public static XboxGameAccountCollection FromAccounts(IEnumerable<IXboxGameAccount> accounts)
    {
        var accountList = accounts
            .Where(account => !string.IsNullOrEmpty(account.Identifier))
            .GroupBy(account => account.Identifier)
            .Select(group => group.OrderBy(_ => _).First())
            .OrderBy(_ => _)
            .ToList();
        return new XboxGameAccountCollection(accountList);
    }
    
    private readonly IReadOnlyCollection<IXboxGameAccount> _accounts;
    
    private XboxGameAccountCollection(IReadOnlyCollection<IXboxGameAccount> accounts)
    {
        _accounts = accounts;
    }
    
    public int Count => _accounts.Count;

    public IXboxGameAccount GetAccount(string identifier)
    {
        if (TryGetAccount(identifier, out var account))
            return account!;
        else
            throw new KeyNotFoundException("Cannot find any account with the specified identifier: " + identifier);
    }

    public bool TryGetAccount(string identifier, out IXboxGameAccount? account)
    {
        account = _accounts.FirstOrDefault(account => account.Identifier == identifier);
        return account != null;
    }

    public bool Contains(IXboxGameAccount toFind)
    {
        if (string.IsNullOrEmpty(toFind.Identifier))
            return false;
        return _accounts.Any(account => account.Identifier == toFind.Identifier);
    }

    public void CopyTo(IXboxGameAccount[] array, int startIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "The start index cannot be negative.");
        
        if (startIndex + Count > array.Length)
            throw new ArgumentException("The number of elements in the source collection exceeds the available space in the array.");

        var result = this.ToArray();
        result.CopyTo(array, startIndex);
    }

    public IEnumerator<IXboxGameAccount> GetEnumerator()
    {
        return _accounts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
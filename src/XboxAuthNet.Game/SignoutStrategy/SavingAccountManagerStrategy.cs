using System.Threading.Tasks;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game.SignoutStrategy;

public class SavingAccountManagerStrategy : ISignoutStrategy
{
    private readonly IXboxGameAccountManager _accountManager;

    public SavingAccountManagerStrategy(IXboxGameAccountManager accountManager) =>
        _accountManager = accountManager;

    public ValueTask Signout()
    {
        _accountManager.SaveAccounts();
        return new ValueTask();
    }
}
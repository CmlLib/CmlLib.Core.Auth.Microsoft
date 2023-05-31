using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Accounts;

public class AccountSaver : IAuthenticator
{
    private readonly IXboxGameAccountManager _accountManager;

    public AccountSaver(IXboxGameAccountManager accountManager)
    {
        this._accountManager = accountManager;
    }

    public ValueTask ExecuteAsync(AuthenticateContext context)
    {
        _accountManager.SaveAccounts();
        return new ValueTask();
    }
}
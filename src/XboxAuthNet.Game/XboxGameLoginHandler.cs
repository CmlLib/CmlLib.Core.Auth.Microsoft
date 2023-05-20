using System.Net.Http;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game;

public class XboxGameLoginHandler
{
    private bool _accountLoaded;

    protected readonly HttpClient HttpClient;
    protected readonly IXboxGameAccountManager AccountManager;

    public XboxGameLoginHandler(
        HttpClient httpClient,
        IXboxGameAccountManager accountManager) =>
        (HttpClient, AccountManager) = (httpClient, accountManager);

    public XboxGameAccountCollection GetAccounts()
    {
        if (!_accountLoaded)
        {
            AccountManager.LoadAccounts();
            _accountLoaded = true;
        }
        return AccountManager.Accounts;
    }
}
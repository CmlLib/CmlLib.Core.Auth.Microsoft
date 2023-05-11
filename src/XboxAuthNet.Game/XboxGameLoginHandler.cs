using System.Net.Http;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game;

public class XboxGameLoginHandler
{
    protected readonly HttpClient HttpClient;
    protected readonly IXboxGameAccountManager AccountManager;

    public XboxGameLoginHandler(
        HttpClient httpClient,
        IXboxGameAccountManager accountManager) =>
        (HttpClient, AccountManager) = (httpClient, accountManager);

    public XboxGameAccountCollection GetAccounts()
    {
        return AccountManager.Accounts;
    }
}
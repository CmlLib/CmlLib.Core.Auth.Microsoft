using Microsoft.Extensions.Logging;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game;

public class XboxGameLoginHandler
{
    private readonly ILogger _logger;
    protected readonly HttpClient HttpClient;
    public IXboxGameAccountManager AccountManager { get; }

    public XboxGameLoginHandler(LoginHandlerParameters parameters)
    {
        _logger = parameters.Logger;
        HttpClient = parameters.HttpClient;
        AccountManager = parameters.AccountManager;
    }

    public NestedAuthenticator CreateAuthenticator(
        IXboxGameAccount account,
        CancellationToken cancellationToken)
    {
        var authenticator = new NestedAuthenticator();
        authenticator.Context = createContext(account, cancellationToken);
        authenticator.AddPostAuthenticator(LastAccessLogger.Default);
        authenticator.AddPostAuthenticator(new AccountSaver(AccountManager));
        return authenticator;
    }

    private AuthenticateContext createContext(
        IXboxGameAccount account,
        CancellationToken cancellationToken)
    {
        return new AuthenticateContext(
            account.SessionStorage,
            HttpClient,
            cancellationToken,
            _logger);
    }

    public NestedAuthenticator CreateAuthenticatorWithDefaultAccount(
        CancellationToken cancellationToken = default) =>
        CreateAuthenticator(AccountManager.GetDefaultAccount(), cancellationToken);

    public NestedAuthenticator CreateAuthenticatorWithNewAccount(
        CancellationToken cancellationToken = default) =>
        CreateAuthenticator(AccountManager.NewAccount(), cancellationToken);
}
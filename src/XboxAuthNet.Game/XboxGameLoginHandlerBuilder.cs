using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game;

public class XboxGameLoginHandlerBuilder : 
    XboxGameLoginHandlerBuilderBase<XboxGameLoginHandlerBuilder>
{

}

public abstract class XboxGameLoginHandlerBuilderBase<T>
    where T : XboxGameLoginHandlerBuilderBase<T>
{
    private HttpClient? _httpClient;
    public HttpClient HttpClient
    {
        get => _httpClient ??= HttpHelper.DefaultHttpClient.Value;
        set => _httpClient = value;
    }

    private IXboxGameAccountManager? _accountManager;
    public IXboxGameAccountManager AccountManager
    {
        get => _accountManager ??= CreateDefaultAccountManager();
        set => _accountManager = value;
    }

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public T WithHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
        return getThis();
    }

    public T WithAccountManager(IXboxGameAccountManager accountManager)
    {
        AccountManager = accountManager;
        return getThis();
    }

    public T WithLogger(ILogger logger)
    {
        Logger = logger;
        return getThis();
    }

    protected virtual IXboxGameAccountManager CreateDefaultAccountManager()
    {
        return new JsonXboxGameAccountManager("accounts.json");
    }

    private T getThis() => (T)this;

    public LoginHandlerParameters BuildParameters()
    {
        return new LoginHandlerParameters(
            httpClient: HttpClient,
            accountManager: AccountManager,
            logger: Logger
        );
    }
}
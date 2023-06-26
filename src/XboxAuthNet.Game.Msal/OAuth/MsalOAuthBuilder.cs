using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.Msal.OAuth;
using Microsoft.Identity.Client;
using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.Msal;

public class MsalOAuthBuilder
{
    private readonly IPublicClientApplication _app;
    public string[] Scopes { get; set; } = MsalClientHelper.XboxScopes;

    public MsalOAuthBuilder(IPublicClientApplication app) => _app = app;

    private ISessionSource<MicrosoftOAuthResponse>? _sessionSource;
    public ISessionSource<MicrosoftOAuthResponse> SessionSource
    {
        get => _sessionSource ??= MicrosoftOAuthSessionSource.Default;
        set => _sessionSource = value;
    }

    private ISessionSource<string>? _loginHintSource;
    public ISessionSource<string> LoginHintSource
    {
        get => _loginHintSource ??= new SessionFromStorage<string>("microsoftLoginHint");
        set => _loginHintSource = value;
    }

    public IAuthenticator Silent() => 
        new MsalSilentOAuth(createParameters());

    public IAuthenticator Interactive() =>
        new MsalInteractiveOAuth(createParameters());

    public IAuthenticator EmbeddedWebView()
    {
        var authenticator = new MsalInteractiveOAuth(createParameters());
        authenticator.UseDefaultWebViewOption = false;
        authenticator.UseEmbeddedWebView = true;
        return authenticator;
    }

    public IAuthenticator SystemBrowser()
    {
        var authenticator = new MsalInteractiveOAuth(createParameters());
        authenticator.UseDefaultWebViewOption = false;
        authenticator.UseEmbeddedWebView = false;
        return authenticator;
    }

    public IAuthenticator DeviceCode(Func<DeviceCodeResult, Task> deviceResultCallback) =>
        new MsalDeviceCodeOAuth(createParameters(), deviceResultCallback);

    public IAuthenticator FromResult(AuthenticationResult result) =>
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(
            MsalClientHelper.ToMicrosoftOAuthResponse(result), 
            SessionSource);

    private MsalOAuthParameters createParameters() => new(
        app: _app,
        scopes: Scopes,
        loginHintSource: LoginHintSource,
        sessionSource: SessionSource);
}
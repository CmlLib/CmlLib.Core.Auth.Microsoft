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

    public IAuthenticator Silent() => Silent(null);

    public IAuthenticator Silent(string? loginHint) => 
        new MsalSilentOAuth(_app, Scopes, loginHint, SessionSource);

    public IAuthenticator Interactive() =>
        new MsalInteractiveOAuth(_app, Scopes, SessionSource);

    public IAuthenticator EmbeddedWebView()
    {
        var authenticator = new MsalInteractiveOAuth(_app, Scopes, SessionSource);
        authenticator.UseDefaultWebViewOption = false;
        authenticator.UseEmbeddedWebView = true;
        return authenticator;
    }

    public IAuthenticator SystemBrowser()
    {
        var authenticator = new MsalInteractiveOAuth(_app, Scopes, SessionSource);
        authenticator.UseDefaultWebViewOption = false;
        authenticator.UseEmbeddedWebView = false;
        return authenticator;
    }

    public IAuthenticator DeviceCode(Func<DeviceCodeResult, Task> deviceResultCallback) =>
        new MsalDeviceCodeOAuth(_app, Scopes, SessionSource, deviceResultCallback);

    public IAuthenticator FromResult(AuthenticationResult result) =>
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(
            MsalClientHelper.ToMicrosoftOAuthResponse(result), 
            SessionSource);
}
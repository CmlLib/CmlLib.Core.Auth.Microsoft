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
        get => _loginHintSource ??= MicrosoftOAuthLoginHintSource.Default;
        set => _loginHintSource = value;
    }

    public ISessionValidator LoginHintValidator(bool throwWhenInvalid = false) =>
        new MicrosoftOAuthLoginHintValidator(throwWhenInvalid, LoginHintSource);

    public IAuthenticator Silent() => 
        new MsalSilentOAuth(createParameters(false));

    public IAuthenticator Interactive() =>
        Interactive(builderInvoker => { });

    public IAuthenticator Interactive(Action<AcquireTokenInteractiveParameterBuilder> builderInvoker) =>
        new MsalInteractiveOAuth(createParameters(false), builderInvoker);

    public IAuthenticator EmbeddedWebView()
    {
        var authenticator = new MsalInteractiveOAuth(createParameters(false), builder =>
        {
            builder.WithUseEmbeddedWebView(true);
        });
        return authenticator;
    }

    public IAuthenticator SystemBrowser()
    {
        var authenticator = new MsalInteractiveOAuth(createParameters(false), builder =>
        {
            builder.WithUseEmbeddedWebView(false);
        });
        return authenticator;
    }

    public IAuthenticator InteractiveWithSingleAccount()
    {
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticatorWithoutValidator(Interactive(builder =>
        {
            // NoPrompt with LoginHint will try 'none' and also 'login' mode.
            // Prompt.ForceLogin is unnecessary.
            builder.WithPrompt(Prompt.NoPrompt);
        }));
        return authenticator;
    }

    public IAuthenticator CodeFlow()
    {
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticatorWithoutValidator(Silent());
        authenticator.AddAuthenticator(LoginHintValidator(true), InteractiveWithSingleAccount());
        authenticator.AddAuthenticatorWithoutValidator(Interactive());
        return authenticator;
    }

    public IAuthenticator DeviceCode(Func<DeviceCodeResult, Task> deviceResultCallback) =>
        new MsalDeviceCodeOAuth(createParameters(false), deviceResultCallback);

    public IAuthenticator ClearSession()
    {
        var authenticator = new AuthenticatorCollection();
        authenticator.AddAuthenticatorWithoutValidator(new SessionCleaner<string>(LoginHintSource));
        authenticator.AddAuthenticatorWithoutValidator(new SessionCleaner<MicrosoftOAuthResponse>(SessionSource));
        return authenticator;
    }

    public IAuthenticator FromResult(AuthenticationResult result) =>
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(
            MsalClientHelper.ToMicrosoftOAuthResponse(result), 
            SessionSource);

    private MsalOAuthParameters createParameters(bool loginHint) => new(
        app: _app,
        scopes: Scopes,
        loginHintSource: LoginHintSource,
        throwWhenEmptyLoginHint: loginHint,
        sessionSource: SessionSource);
}
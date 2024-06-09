using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth.CodeFlow.Parameters;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthBuilder
{
    private readonly MicrosoftOAuthClientInfo _clientInfo;

    public MicrosoftOAuthBuilder(MicrosoftOAuthClientInfo clientInfo)
         => _clientInfo = clientInfo;
    
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

    private MicrosoftOAuthParameters createParameters() => 
        new MicrosoftOAuthParameters(_clientInfo, SessionSource, LoginHintSource);

    public ISessionValidator Validator() =>
        new MicrosoftOAuthValidator(SessionSource);

    public ISessionValidator LoginHintValidator(bool throwWhenInvalid = false) =>
        new MicrosoftOAuthLoginHintValidator(throwWhenInvalid, LoginHintSource);

    public IAuthenticator Silent() =>
        new SilentMicrosoftOAuth(createParameters());

    public IAuthenticator Interactive() =>
        Interactive(builder => {}, new CodeFlowAuthorizationParameter());

    public IAuthenticator Interactive(CodeFlowAuthorizationParameter parameters) =>
        Interactive(builder => {}, parameters);

    public IAuthenticator Interactive(Action<CodeFlowBuilder> builderInvoker) =>
        Interactive(builderInvoker, new CodeFlowAuthorizationParameter());

    public IAuthenticator Interactive(
        Action<CodeFlowBuilder> builderInvoker,
        CodeFlowAuthorizationParameter parameters) => 
        new InteractiveMicrosoftOAuth(createParameters(), builderInvoker, parameters);

    public IAuthenticator InteractiveWithSingleAccount() =>
        InteractiveWithSingleAccount(builderInvoker => { });

    public IAuthenticator InteractiveWithSingleAccount(Action<CodeFlowBuilder> builderInvoker)
    {
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticatorWithoutValidator(
            Interactive(
                builderInvoker, 
                new CodeFlowAuthorizationParameter
                {
                    Prompt = MicrosoftOAuthPromptModes.None
                }));
        authenticator.AddAuthenticatorWithoutValidator(
            Interactive(
                builderInvoker, 
                new CodeFlowAuthorizationParameter
                {
                    Prompt = MicrosoftOAuthPromptModes.Login
                }));
        return authenticator;
    }

    public IAuthenticator CodeFlow() =>
        CodeFlow(builderInvoker => { });

    public IAuthenticator CodeFlow(Action<CodeFlowBuilder> builderInvoker)
    {
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticatorWithoutValidator(Silent());
        authenticator.AddAuthenticator(LoginHintValidator(true), InteractiveWithSingleAccount(builderInvoker));
        authenticator.AddAuthenticatorWithoutValidator(Interactive(builderInvoker));
        return authenticator;
    }

    public IAuthenticator Signout()
    {
        var authenticator = new AuthenticatorCollection();
        authenticator.AddAuthenticatorWithoutValidator(new SessionCleaner<string>(LoginHintSource));
        authenticator.AddAuthenticatorWithoutValidator(new SessionCleaner<MicrosoftOAuthResponse>(SessionSource));
        return authenticator;
    }

    public IAuthenticator SignoutWithBrowser() =>
        SignoutWithBrowser(builder => { });

    public IAuthenticator SignoutWithBrowser(Action<CodeFlowBuilder> builderInvoker)
    {
        var authenticator = new AuthenticatorCollection();
        authenticator.AddAuthenticatorWithoutValidator(new SessionCleaner<string>(LoginHintSource));
        authenticator.AddAuthenticatorWithoutValidator(new SessionCleaner<MicrosoftOAuthResponse>(SessionSource));
        authenticator.AddAuthenticatorWithoutValidator(new MicrosoftOAuthSignout(createParameters(), builderInvoker));
        return authenticator;
    }

    public IAuthenticator FromMicrosoftOAuthResponse(MicrosoftOAuthResponse response) => 
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(response, SessionSource);
}
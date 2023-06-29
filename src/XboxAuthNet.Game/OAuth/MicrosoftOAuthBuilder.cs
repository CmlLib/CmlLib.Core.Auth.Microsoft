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
        get => _loginHintSource = MicrosoftOAuthLoginHintSource.Default;
        set => _loginHintSource = value;
    }

    private CodeFlowAuthorizationParameter createCodeFlowParameters()
    {
        var parameter = new CodeFlowParameterFactory().CreateAuthorizationParameter();
        parameter.Prompt = MicrosoftOAuthPromptModes.SelectAccount;
        return parameter;
    }

    private MicrosoftOAuthParameters createParameters() => 
        new MicrosoftOAuthParameters(_clientInfo, SessionSource, LoginHintSource);

    public ISessionValidator Validator() =>
        new MicrosoftOAuthValidator(SessionSource);

    public IAuthenticator Silent() =>
        new SilentMicrosoftOAuth(createParameters());

    public IAuthenticator Interactive() =>
        Interactive(builder => {}, createCodeFlowParameters());

    public IAuthenticator Interactive(CodeFlowAuthorizationParameter parameters) =>
        Interactive(builder => {}, parameters);

    public IAuthenticator Interactive(Action<CodeFlowBuilder> builderInvoker) =>
        Interactive(builderInvoker, createCodeFlowParameters());

    public IAuthenticator Interactive(
        Action<CodeFlowBuilder> builderInvoker,
        CodeFlowAuthorizationParameter parameters)
    {
        return new InteractiveMicrosoftOAuth(createParameters(), builderInvoker, parameters);
    }

    public IAuthenticator Signout() => 
        Signout(builder => {});

    public IAuthenticator Signout(Action<CodeFlowBuilder> builderInvoker)
    {
        return new MicrosoftOAuthSignout(createParameters(), builderInvoker);
    }

    public IAuthenticator ClearSession() =>
        new SessionCleaner<MicrosoftOAuthResponse>(SessionSource);

    public IAuthenticator FromMicrosoftOAuthResponse(MicrosoftOAuthResponse response) => 
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(response, SessionSource);
}
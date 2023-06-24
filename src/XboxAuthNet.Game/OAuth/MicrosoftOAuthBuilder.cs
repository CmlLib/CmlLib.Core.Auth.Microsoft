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

    private CodeFlowAuthorizationParameter createDefaultParameters()
    {
        var parameter = new CodeFlowParameterFactory().CreateAuthorizationParameter();
        parameter.Prompt = MicrosoftOAuthPromptModes.SelectAccount;
        return parameter;
    }

    public ISessionValidator Validator() =>
        new MicrosoftOAuthValidator(SessionSource);

    public IAuthenticator Silent() =>
        new SilentMicrosoftOAuth(_clientInfo, SessionSource);

    public IAuthenticator Interactive() =>
        Interactive(builder => {}, createDefaultParameters());

    public IAuthenticator Interactive(CodeFlowAuthorizationParameter parameters) =>
        Interactive(builder => {}, parameters);

    public IAuthenticator Interactive(Action<CodeFlowBuilder> builderInvoker) =>
        Interactive(builderInvoker, createDefaultParameters());

    public IAuthenticator Interactive(
        Action<CodeFlowBuilder> builderInvoker,
        CodeFlowAuthorizationParameter parameters)
    {
        return new InteractiveMicrosoftOAuth(
            _clientInfo, 
            builderInvoker, 
            parameters,
            SessionSource);
    }

    public IAuthenticator Signout() => 
        Signout(builder => {});

    public IAuthenticator Signout(Action<CodeFlowBuilder> builderInvoker)
    {
        return new MicrosoftOAuthSignout(
            _clientInfo, 
            builderInvoker, 
            SessionSource);
    }

    public IAuthenticator ClearSession() =>
        new SessionCleaner<MicrosoftOAuthResponse>(SessionSource);

    public IAuthenticator FromMicrosoftOAuthResponse(MicrosoftOAuthResponse response) => 
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(response, SessionSource);
}
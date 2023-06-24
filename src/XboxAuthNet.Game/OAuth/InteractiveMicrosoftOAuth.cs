using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow;
using XboxAuthNet.OAuth.CodeFlow.Parameters;

namespace XboxAuthNet.Game.OAuth;

public class InteractiveMicrosoftOAuth : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly MicrosoftOAuthClientInfo _clientInfo;
    private readonly Action<CodeFlowBuilder> _codeFlowBuilder;
    private readonly CodeFlowAuthorizationParameter _parameters;

    public InteractiveMicrosoftOAuth(
        MicrosoftOAuthClientInfo clientInfo,
        Action<CodeFlowBuilder> codeFlowBuilder,
        CodeFlowAuthorizationParameter parameters,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
         : base(sessionSource) =>
        (_clientInfo, _codeFlowBuilder, _parameters) = (clientInfo, codeFlowBuilder, parameters);

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(AuthenticateContext context)
    {
        var apiClient = _clientInfo.CreateApiClientForOAuthCode(context.HttpClient);
        var builder = new CodeFlowBuilder(apiClient);
        _codeFlowBuilder.Invoke(builder);
        var oauthHandler = builder.Build();

        context.Logger.LogInteractiveMicrosoftOAuth();
        return await oauthHandler.AuthenticateInteractively(_parameters, context.CancellationToken);
    }
}

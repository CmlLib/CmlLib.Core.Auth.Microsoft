using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow;
using XboxAuthNet.OAuth.CodeFlow.Parameters;

namespace XboxAuthNet.Game.OAuth;

public class InteractiveMicrosoftOAuth : MicrosoftOAuth
{
    private readonly Action<CodeFlowBuilder> _codeFlowBuilder;
    private readonly CodeFlowAuthorizationParameter _codeFlowParameters;

    public InteractiveMicrosoftOAuth(
        MicrosoftOAuthParameters parameters,
        Action<CodeFlowBuilder> codeFlowBuilder,
        CodeFlowAuthorizationParameter codeFlowParameters)
         : base(parameters) =>
        (_codeFlowBuilder, _codeFlowParameters) = 
        (codeFlowBuilder, codeFlowParameters);

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(
        AuthenticateContext context, MicrosoftOAuthParameters parameters)
    {
        context.Logger.LogInteractiveMicrosoftOAuth();

        var apiClient = parameters.ClientInfo.CreateApiClientForOAuthCode(context.HttpClient);
        var builder = new CodeFlowBuilder(apiClient);
        _codeFlowBuilder.Invoke(builder);
        var oauthHandler = builder.Build();

        var loginHint = parameters.LoginHintSource.Get(context.SessionStorage);
        if (string.IsNullOrEmpty(_codeFlowParameters.LoginHint))
            _codeFlowParameters.LoginHint = loginHint;

        return await oauthHandler.AuthenticateInteractively(_codeFlowParameters, context.CancellationToken);
    }
}

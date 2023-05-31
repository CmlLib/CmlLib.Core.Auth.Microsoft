using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuth;

public class InteractiveMicrosoftOAuth : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly MicrosoftOAuthClientInfo _clientInfo;
    private readonly Action<MicrosoftOAuthCodeFlowBuilder> _codeFlowBuilder;
    private readonly MicrosoftOAuthParameters _parameters;

    public InteractiveMicrosoftOAuth(
        MicrosoftOAuthClientInfo clientInfo,
        Action<MicrosoftOAuthCodeFlowBuilder> codeFlowBuilder,
        MicrosoftOAuthParameters parameters,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
         : base(sessionSource) =>
        (_clientInfo, _codeFlowBuilder, _parameters) = (clientInfo, codeFlowBuilder, parameters);

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate()
    {
        var apiClient = _clientInfo.CreateApiClientForOAuthCode(Context.HttpClient);
        var builder = new MicrosoftOAuthCodeFlowBuilder(apiClient);
        _codeFlowBuilder.Invoke(builder);
        var oauthHandler = builder.Build();
        return await oauthHandler.Authenticate(_parameters, Context.CancellationToken);
    }
}

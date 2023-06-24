using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthSignout : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly MicrosoftOAuthClientInfo clientInfo;
    private readonly Action<CodeFlowBuilder> builderInvoker;
    private readonly ISessionSource<MicrosoftOAuthResponse> sessionSource;

    public MicrosoftOAuthSignout(
        MicrosoftOAuthClientInfo clientInfo,
        Action<CodeFlowBuilder> builderInvoker,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
    : base(sessionSource)
    {
        this.clientInfo = clientInfo;
        this.builderInvoker = builderInvoker;
        this.sessionSource = sessionSource;
    }

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(AuthenticateContext context)
    {
        var apiClient = clientInfo.CreateApiClientForOAuthCode(context.HttpClient);
        var builder = new CodeFlowBuilder(apiClient);
        builderInvoker.Invoke(builder);
        var codeFlow = builder.Build();   

        context.Logger.LogMicrosoftOAuthSignout();
        await codeFlow.Signout(context.CancellationToken);
        return null;
    }
}
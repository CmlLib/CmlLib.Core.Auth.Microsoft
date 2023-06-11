using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthSignout : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly MicrosoftOAuthClientInfo clientInfo;
    private readonly Action<MicrosoftOAuthCodeFlowBuilder> builderInvoker;
    private readonly ISessionSource<MicrosoftOAuthResponse> sessionSource;

    public MicrosoftOAuthSignout(
        MicrosoftOAuthClientInfo clientInfo,
        Action<MicrosoftOAuthCodeFlowBuilder> builderInvoker,
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
        var builder = new MicrosoftOAuthCodeFlowBuilder(apiClient);
        builderInvoker.Invoke(builder);
        var codeFlow = builder.Build();   

        await codeFlow.Signout(context.CancellationToken);
        return null;
    }
}
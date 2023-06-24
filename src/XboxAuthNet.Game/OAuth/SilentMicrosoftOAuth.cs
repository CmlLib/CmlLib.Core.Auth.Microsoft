using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow.Parameters;

namespace XboxAuthNet.Game.OAuth;

public class SilentMicrosoftOAuth : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly MicrosoftOAuthClientInfo _clientInfo;

    public SilentMicrosoftOAuth(
        MicrosoftOAuthClientInfo clientInfo,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
        : base(sessionSource) =>
        _clientInfo = clientInfo;

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(AuthenticateContext context)
    {
        var session = GetSessionFromStorage();
        if (string.IsNullOrEmpty(session?.RefreshToken))
            throw new MicrosoftOAuthException("no refresh token", 0);

        context.Logger.LogSilentMicrosoftOAuth();
        var parameterFactory = new CodeFlowParameterFactory();
        var apiClient = _clientInfo.CreateApiClientForOAuthCode(context.HttpClient);
        return await apiClient.RefreshToken(
            parameterFactory.CreateRefreshTokenParameter(session.RefreshToken), 
            context.CancellationToken);
    }
}

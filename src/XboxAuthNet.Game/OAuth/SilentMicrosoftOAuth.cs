using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow.Parameters;

namespace XboxAuthNet.Game.OAuth;

public class SilentMicrosoftOAuth : MicrosoftOAuth
{
    public SilentMicrosoftOAuth(MicrosoftOAuthParameters parameters) : base(parameters)
    {

    }

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(
        AuthenticateContext context, MicrosoftOAuthParameters parameters)
    {
        var session = GetSessionFromStorage();
        if (string.IsNullOrEmpty(session?.RefreshToken))
            throw new MicrosoftOAuthException("no refresh token", 0);

        context.Logger.LogSilentMicrosoftOAuth();
        var parameterFactory = new CodeFlowParameterFactory();
        var apiClient = parameters.ClientInfo.CreateApiClientForOAuthCode(context.HttpClient);
        return await apiClient.RefreshToken(
            parameterFactory.CreateRefreshTokenParameter(session.RefreshToken), 
            context.CancellationToken);
    }
}

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
            throw new MicrosoftOAuthException("Cached RefreshToken of the user was empty. Interactive microsoft authentication is required.", 0);

        context.Logger.LogSilentMicrosoftOAuth();
        var apiClient = parameters.ClientInfo.CreateApiClientForOAuthCode(context.HttpClient);
        return await apiClient.RefreshToken(
            new CodeFlowRefreshTokenParameter
            {
                RefreshToken = session.RefreshToken
            }, 
            context.CancellationToken);
    }
}

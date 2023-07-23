using XboxAuthNet.Game.Authenticators;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalSilentOAuth : MsalOAuth
{
    public MsalSilentOAuth(MsalOAuthParameters parameters) : 
        base(parameters)
    {

    }

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context, MsalOAuthParameters parameters)
    {
        var loginHint = parameters.LoginHintSource.Get(context.SessionStorage);
        context.Logger.LogMsalSilentOAuth(loginHint);

        if (string.IsNullOrEmpty(loginHint))
            throw new MsalException("loginHint was empty. Interactive Microsoft OAuth with IdToken is required. (ex: MsalInteractiveOAuth)");

        return await parameters.MsalApplication
            .AcquireTokenSilent(parameters.Scopes, loginHint)
            .ExecuteAsync();
    }
}

using XboxAuthNet.Game.Authenticators;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalSilentOAuth : MsalOAuth
{
    public MsalSilentOAuth(MsalOAuthParameters parameters)
        : base(parameters)
    {
    }

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context,
        MsalOAuthParameters parameters)
    {
        var loginHint = parameters.LoginHintSource.Get(context.SessionStorage);
        var homeAccountId = parameters.HomeAccountIdSource.Get(context.SessionStorage);
        context.Logger.LogMsalSilentOAuth(loginHint);

        AcquireTokenSilentParameterBuilder builder;
        if (!string.IsNullOrEmpty(homeAccountId))
        {
            var account = await parameters.MsalApplication.GetAccountAsync(homeAccountId);
            context.CancellationToken.ThrowIfCancellationRequested();
            if (account is null)
            {
                // Do not fall back to a different account when the saved account is missing.
                throw new MsalUiRequiredException(
                    "account_not_found",
                    "The saved MSAL account was not found in the token cache. Interactive authentication is required.");
            }

            builder = parameters.MsalApplication.AcquireTokenSilent(parameters.Scopes, account);
        }
        else
        {
            // Older sessions only contain a login hint.
            if (string.IsNullOrEmpty(loginHint))
                throw new MsalException("loginHint was empty. Interactive Microsoft OAuth with IdToken is required. (ex: MsalInteractiveOAuth)");

            builder = parameters.MsalApplication.AcquireTokenSilent(parameters.Scopes, loginHint);
        }

        return await builder.ExecuteAsync(context.CancellationToken);
    }
}

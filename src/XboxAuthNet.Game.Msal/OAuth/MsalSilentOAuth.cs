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
                // 저장된 식별자가 있으면 다른 hint의 계정으로 대체하지 않는다.
                throw new MsalUiRequiredException(
                    "account_not_found",
                    "The saved MSAL account was not found in the token cache. Interactive authentication is required.");
            }

            builder = parameters.MsalApplication.AcquireTokenSilent(parameters.Scopes, account);
        }
        else
        {
            // 이전 계정 파일은 HomeAccountId 없이 login hint만 저장한다.
            if (string.IsNullOrEmpty(loginHint))
                throw new MsalException("loginHint was empty. Interactive Microsoft OAuth with IdToken is required. (ex: MsalInteractiveOAuth)");

            builder = parameters.MsalApplication.AcquireTokenSilent(parameters.Scopes, loginHint);
        }

        return await builder.ExecuteAsync(context.CancellationToken);
    }
}

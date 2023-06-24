using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using Microsoft.Identity.Client;
using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalSilentOAuth : MsalOAuth
{
    public string? LoginHint { get; set; }

    public MsalSilentOAuth(
        IPublicClientApplication app,
        string[] scopes,
        string? loginHint,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
        : base(app, scopes, sessionSource) =>
        LoginHint = loginHint;

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context, IPublicClientApplication app, string[] scopes)
    {
        context.Logger.LogMsalSilentOAuth(LoginHint);
        if (string.IsNullOrEmpty(LoginHint))
        {
            var accounts = await app.GetAccountsAsync();
            return await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
        }
        else
            return await app.AcquireTokenSilent(scopes, LoginHint).ExecuteAsync();

    }
}

using XboxAuthNet.Game.SessionStorages;
using Microsoft.Identity.Client;
using XboxAuthNet.OAuth.Models;

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
        IPublicClientApplication app, string[] scopes)
    {
        if (string.IsNullOrEmpty(LoginHint))
        {
            var accounts = await app.GetAccountsAsync();
            return await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
        }
        else
            return await app.AcquireTokenSilent(scopes, LoginHint).ExecuteAsync();

    }
}

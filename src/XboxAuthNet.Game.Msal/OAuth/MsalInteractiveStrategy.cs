using XboxAuthNet.Game.SessionStorages;
using Microsoft.Identity.Client;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalInteractiveOAuth : MsalOAuth
{
    public bool UseDefaultWebViewOption { get; set; } = true;
    public bool UseEmbeddedWebView { get; set; } = true;

    public MsalInteractiveOAuth(
        IPublicClientApplication app,
        string[] scopes,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
        : base(app, scopes, sessionSource)
    {

    }

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        IPublicClientApplication app, string[] scopes)
    {
        var builder = app.AcquireTokenInteractive(scopes);
        if (!UseDefaultWebViewOption)
            builder.WithUseEmbeddedWebView(UseEmbeddedWebView);
        var result = await builder.ExecuteAsync(Context.CancellationToken);
        return result;
    }
}

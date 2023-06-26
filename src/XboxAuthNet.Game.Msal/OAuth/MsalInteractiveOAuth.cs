using XboxAuthNet.Game.Authenticators;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalInteractiveOAuth : MsalOAuth
{
    public bool UseDefaultWebViewOption { get; set; } = true;
    public bool UseEmbeddedWebView { get; set; } = true;

    public MsalInteractiveOAuth(MsalOAuthParameters parameters) : 
        base(parameters)
    {

    }

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context, MsalOAuthParameters parameters)
    {
        context.Logger.LogMsalInteractiveOAuth();

        var builder = parameters.MsalApplication
            .AcquireTokenInteractive(parameters.Scopes);

        if (!UseDefaultWebViewOption)
            builder.WithUseEmbeddedWebView(UseEmbeddedWebView);

        var result = await builder.ExecuteAsync(context.CancellationToken);
        return result;
    }
}
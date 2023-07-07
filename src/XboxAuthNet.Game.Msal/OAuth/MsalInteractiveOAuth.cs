using XboxAuthNet.Game.Authenticators;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalInteractiveOAuth : MsalOAuth
{
    private readonly Action<AcquireTokenInteractiveParameterBuilder> _builderInvoker;

    public MsalInteractiveOAuth(
        MsalOAuthParameters parameters,
        Action<AcquireTokenInteractiveParameterBuilder> builderInvoker) : 
        base(parameters)
    {
        this._builderInvoker = builderInvoker;
    }

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context, MsalOAuthParameters parameters)
    {
        context.Logger.LogMsalInteractiveOAuth();

        var builder = parameters.MsalApplication
            .AcquireTokenInteractive(parameters.Scopes);

        var loginHint = parameters.LoginHintSource.Get(context.SessionStorage);
        if (!string.IsNullOrEmpty(loginHint))
            builder.WithLoginHint(loginHint);

        _builderInvoker.Invoke(builder);

        var result = await builder.ExecuteAsync(context.CancellationToken);
        return result;
    }
}
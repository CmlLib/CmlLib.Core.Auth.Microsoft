using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public abstract class MsalOAuth : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly MsalOAuthParameters _parameters;

    public MsalOAuth(
        MsalOAuthParameters parameters)
        : base(parameters.SessionSource) =>
        _parameters = parameters;

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(AuthenticateContext context)
    {
        var result = await AuthenticateWithMsal(context, _parameters);

        var loginHint = result.Account.Username;
        _parameters.LoginHintSource.Set(context.SessionStorage, loginHint);
        _parameters.SessionSource.SetKeyMode(context.SessionStorage, SessionStorages.SessionStorageKeyMode.NoStore);

        return MsalClientHelper.ToMicrosoftOAuthResponse(result);
    }

    protected abstract ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context, MsalOAuthParameters parameters);
}
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalResultOAuth : MsalOAuth
{
    private readonly AuthenticationResult _result;

    public MsalResultOAuth(MsalOAuthParameters parameters, AuthenticationResult result)
        : base(parameters)
    {
        _result = result;
    }

    protected override ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context,
        MsalOAuthParameters parameters) => new(_result);
}

using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public abstract class MsalOAuth : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private readonly IPublicClientApplication _msal;
    private readonly string[] _scopes;

    public MsalOAuth(
        IPublicClientApplication app, 
        string[] scopes,
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
        : base(sessionSource) =>
        (_msal, _scopes) = (app, scopes);

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate()
    {
        var result = await AuthenticateWithMsal(_msal, _scopes);
        return MsalClientHelper.ToMicrosoftOAuthResponse(result);
    }

    protected abstract ValueTask<AuthenticationResult> AuthenticateWithMsal(
        IPublicClientApplication app, string[] scopes);
}
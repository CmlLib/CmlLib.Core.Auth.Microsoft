using Microsoft.Identity.Client;
using XboxAuthNet.OAuth;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Msal;

public class MsalOAuthParameters
{
    public MsalOAuthParameters(
        IPublicClientApplication app,
        string[] scopes,
        ISessionSource<string> loginHintSource,
        ISessionSource<MicrosoftOAuthResponse> sessionSource) =>
        (MsalApplication, Scopes, LoginHintSource, SessionSource) = 
        (app, scopes, loginHintSource, sessionSource);

    public IPublicClientApplication MsalApplication { get; }
    public string[] Scopes { get; }
    public ISessionSource<MicrosoftOAuthResponse> SessionSource;
    public ISessionSource<string> LoginHintSource { get; }
}
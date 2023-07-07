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
        bool throwWhenEmptyLoginHint,
        ISessionSource<MicrosoftOAuthResponse> sessionSource) =>
        (MsalApplication, Scopes, LoginHintSource, ThrowWhenEmptyLoginHint, SessionSource) = 
        (app, scopes, loginHintSource, throwWhenEmptyLoginHint, sessionSource);

    public IPublicClientApplication MsalApplication { get; }
    public string[] Scopes { get; }
    public ISessionSource<MicrosoftOAuthResponse> SessionSource;
    public ISessionSource<string> LoginHintSource { get; }
    public bool ThrowWhenEmptyLoginHint { get; }
}
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Msal.OAuth;
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

    /// <summary>기존 생성자 계약을 유지하면서 계정 식별자의 저장 위치를 지정한다.</summary>
    public ISessionSource<string> HomeAccountIdSource { get; set; } = MsalHomeAccountIdSource.Default;
}
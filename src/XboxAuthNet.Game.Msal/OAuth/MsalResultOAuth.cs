using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Msal.OAuth;

/// <summary>직접 받은 인증 결과에도 MSAL 계정 메타데이터와 저장 정책을 적용한다.</summary>
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

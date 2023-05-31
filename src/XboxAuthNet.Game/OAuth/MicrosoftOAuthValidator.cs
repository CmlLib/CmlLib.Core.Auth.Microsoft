using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthValidator : SessionValidator<MicrosoftOAuthResponse>
{
    public MicrosoftOAuthValidator(
        ISessionSource<MicrosoftOAuthResponse> sessionSource)
        : base(sessionSource)
    {

    }
    
    protected override ValueTask<bool> Validate(AuthenticateContext context, MicrosoftOAuthResponse session)
    {
        return new ValueTask<bool>(session.Validate());
    }
}
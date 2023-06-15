using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JEProfileValidator : SessionValidator<JEProfile>
{
    public JEProfileValidator(ISessionSource<JEProfile> sessionSource)
     : base(sessionSource)
    {

    }

    protected override ValueTask<bool> Validate(AuthenticateContext context, JEProfile profile)
    {
        var isValid = (
            !string.IsNullOrEmpty(profile.Username) && 
            !string.IsNullOrEmpty(profile.UUID));
        context.Logger.LogJEProfileValidator(isValid);
        return new ValueTask<bool>(isValid);
    }
}
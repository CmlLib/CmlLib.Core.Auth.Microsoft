using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

public class JETokenValidator : SessionValidator<JEToken>
{
    public JETokenValidator(ISessionSource<JEToken> sessionSource)
    : base(sessionSource)
    {
        
    }

    protected override ValueTask<bool> Validate(AuthenticateContext context, JEToken token)
    {
        var valid = (token != null && token.Validate());
        context.Logger.LogJETokenValidator(valid);
        return new ValueTask<bool>(valid);
    }
}
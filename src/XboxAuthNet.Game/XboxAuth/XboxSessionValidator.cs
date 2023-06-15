using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxSessionValidator : SessionValidator<XboxAuthTokens>
{
    public XboxSessionValidator(ISessionSource<XboxAuthTokens> sessionSource)
    : base(sessionSource)
    {
        
    }

    protected override ValueTask<bool> Validate(AuthenticateContext context, XboxAuthTokens session)
    {
        var result = session?.XstsToken?.Validate() ?? false;
        context.Logger.LogXboxValidation(result);
        return new ValueTask<bool>(result);
    }
}
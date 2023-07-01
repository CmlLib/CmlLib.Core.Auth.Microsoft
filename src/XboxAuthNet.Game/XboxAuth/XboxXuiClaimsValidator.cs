using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.XboxLive;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxXuiClaimsValidator : SessionValidator<XboxAuthTokens>
{
    private readonly string[] _xuiClaimNames;

    public XboxXuiClaimsValidator(
        string[] xuiClaimNames,
        ISessionSource<XboxAuthTokens> sessionSource) : 
        base(sessionSource)
    {
        _xuiClaimNames = xuiClaimNames;
    }

    protected override ValueTask<bool> Validate(AuthenticateContext context, XboxAuthTokens session)
    {
        var result = validateClaims(session);
        context.Logger.LogXboxXuiClaimsValidation(result);
        return new ValueTask<bool>(result);
    }

    private bool validateClaims(XboxAuthTokens tokens)
    {
        var claims = tokens.XstsToken?.XuiClaims;
        if (claims == null)
            return false;

        foreach (var name in _xuiClaimNames)
        {
            var invalid = 
                (name == XboxAuthXuiClaimNames.AgeGroup && string.IsNullOrEmpty(claims.AgeGroup)) ||
                (name == XboxAuthXuiClaimNames.Gamertag && string.IsNullOrEmpty(claims.Gamertag)) ||
                (name == XboxAuthXuiClaimNames.Privileges && string.IsNullOrEmpty(claims.Privileges)) ||
                (name == XboxAuthXuiClaimNames.UserHash && string.IsNullOrEmpty(claims.UserHash)) ||
                (name == XboxAuthXuiClaimNames.XboxUserId && string.IsNullOrEmpty(claims.XboxUserId)) ||
                (name == XboxAuthXuiClaimNames.UserSettingsRestrictions && string.IsNullOrEmpty(claims.UserSettingsRestrictions)) ||
                (name == XboxAuthXuiClaimNames.UserTitleRestrictions && string.IsNullOrEmpty(claims.UserTitleRestrictions));
            if (invalid)
                return false;
        }
        return true;
    }
}
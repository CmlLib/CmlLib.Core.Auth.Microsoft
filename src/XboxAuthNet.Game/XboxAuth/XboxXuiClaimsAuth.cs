using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.XboxLive;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxXuiClaimsAuth : SessionAuthenticator<XboxAuthTokens>
{
    private readonly string[] _xuiClaimNames;

    public XboxXuiClaimsAuth(
        string[] xuiClaimNames,
        ISessionSource<XboxAuthTokens> sessionSource) :
        base(sessionSource)
    {
        _xuiClaimNames = xuiClaimNames;
    }

    protected async override ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        context.Logger.LogXboxXuiClaims();
        var xboxAuthTokens = GetSessionFromStorage() ?? new XboxAuthTokens();

        var tempXboxAuthToken = new XboxAuthTokens
        {
            UserToken = xboxAuthTokens.UserToken
        };
        var tempSessionSource = new InMemorySessionSource<XboxAuthTokens>(tempXboxAuthToken);

        var xstsAuth = new XboxXstsTokenAuth(XboxAuthConstants.XboxLiveRelyingParty, tempSessionSource);
        await xstsAuth.ExecuteAsync(context);
        tempXboxAuthToken = tempSessionSource.Get(context.SessionStorage);

        if (tempXboxAuthToken != null)
            copyXuiClaims(tempXboxAuthToken, xboxAuthTokens);

        return xboxAuthTokens;
    }


    private void copyXuiClaims(XboxAuthTokens from, XboxAuthTokens to)
    {
        to.XstsToken ??= new XboxLive.Responses.XboxAuthResponse();
        to.XstsToken.XuiClaims ??= new XboxLive.Responses.XboxAuthXuiClaims();

        var fromClaims = from.XstsToken?.XuiClaims;
        var toClaims = to.XstsToken.XuiClaims;

        if (fromClaims == null)
            return;

        foreach (var name in _xuiClaimNames)
        {
            if (name == XboxAuthXuiClaimNames.AgeGroup)
                toClaims.AgeGroup = fromClaims.AgeGroup;
            if (name == XboxAuthXuiClaimNames.Gamertag)
                toClaims.Gamertag = fromClaims.Gamertag;
            if (name == XboxAuthXuiClaimNames.Privileges)
                toClaims.Privileges = fromClaims.Privileges;
            if (name == XboxAuthXuiClaimNames.UserHash)
                toClaims.UserHash = fromClaims.UserHash;
            if (name == XboxAuthXuiClaimNames.UserSettingsRestrictions)
                toClaims.UserSettingsRestrictions = fromClaims.UserSettingsRestrictions;
            if (name == XboxAuthXuiClaimNames.UserTitleRestrictions)
                toClaims.UserTitleRestrictions = fromClaims.UserTitleRestrictions;
            if (name == XboxAuthXuiClaimNames.XboxUserId)
                toClaims.XboxUserId = fromClaims.XboxUserId;
        }
    }
}
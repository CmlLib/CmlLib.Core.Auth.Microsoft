using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.XboxLive;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxDeviceTokenAuth : SessionAuthenticator<XboxAuthTokens>
{
    private readonly string _deviceType;
    private readonly string _deviceVersion;

    public XboxDeviceTokenAuth(
        string deviceType,
        string deviceVersion,
        ISessionSource<XboxAuthTokens> sessionSource)
        : base(sessionSource) => 
        (_deviceType, _deviceVersion) = (deviceType, deviceVersion);

    protected override async ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        var xboxTokens = GetSessionFromStorage() ?? new XboxAuthTokens();

        context.Logger.LogXboxDeviceToken();
        var xboxAuthClient = new XboxAuthClient(context.HttpClient);
        xboxTokens.DeviceToken = await xboxAuthClient.RequestDeviceToken(
            _deviceType, _deviceVersion);
        return xboxTokens;
    }
}
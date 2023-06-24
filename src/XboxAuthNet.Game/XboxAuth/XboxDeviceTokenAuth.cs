using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Crypto;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxDeviceTokenAuth : SessionAuthenticator<XboxAuthTokens>
{
    private readonly IXboxRequestSigner _signer;
    private readonly string _deviceType;
    private readonly string _deviceVersion;

    public XboxDeviceTokenAuth(
        string deviceType,
        string deviceVersion,
        IXboxRequestSigner signer,
        ISessionSource<XboxAuthTokens> sessionSource)
        : base(sessionSource) => 
        (_deviceType, _deviceVersion, _signer) = (deviceType, deviceVersion, signer);

    protected override async ValueTask<XboxAuthTokens?> Authenticate(AuthenticateContext context)
    {
        var xboxTokens = GetSessionFromStorage() ?? new XboxAuthTokens();

        context.Logger.LogXboxDeviceToken();
        var xboxAuthClient = new XboxSignedClient(_signer, context.HttpClient);
        xboxTokens.DeviceToken = await xboxAuthClient.RequestDeviceToken(
            _deviceType, _deviceVersion);
        return xboxTokens;
    }
}
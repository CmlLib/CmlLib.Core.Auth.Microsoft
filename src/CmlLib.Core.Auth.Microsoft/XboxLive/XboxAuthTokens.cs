using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthTokens
    {
        public XboxAuthResponse? DeviceToken { get; set; }
        public XboxAuthResponse? TitleToken { get; set; }
        public XboxAuthResponse? UserToken { get; set; }
        public XboxAuthResponse? XstsToken { get; set; }
    }
}

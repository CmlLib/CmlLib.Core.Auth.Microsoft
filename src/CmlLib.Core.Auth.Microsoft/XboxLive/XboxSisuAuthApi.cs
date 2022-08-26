using System.Threading.Tasks;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxSisuAuthApi : IXboxLiveApi
    {
        private readonly XboxSecureAuth _sisuAuth;
        private readonly XboxSisuAuthParameters _parameters;

        public XboxSisuAuthApi(XboxSecureAuth sisuAuth, XboxSisuAuthParameters parameters)
        {
            this._sisuAuth = sisuAuth;
            this._parameters = parameters;
        }

        public async Task<XboxAuthTokens> GetTokens(string token, XboxAuthTokens? previousTokens, string xstsRelyingParty)
        {
            var deviceToken = await _sisuAuth.RequestDeviceToken(_parameters.DeviceType, _parameters.DeviceVersion);
            if (string.IsNullOrEmpty(deviceToken?.Token))
                throw new XboxAuthException("deviceToken was null", 200);

            var tokens = await _sisuAuth.SisuAuth(token, _parameters.ClientId, deviceToken?.Token!, xstsRelyingParty);
            return new XboxAuthTokens
            {
                DeviceToken = deviceToken,
                TitleToken = tokens.TitleToken,
                UserToken = tokens.UserToken,
                XstsToken = tokens.AuthorizationToken
            };
        }
    }
}

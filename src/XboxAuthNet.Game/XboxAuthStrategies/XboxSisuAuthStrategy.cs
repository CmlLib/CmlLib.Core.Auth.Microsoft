using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Requests;

namespace XboxAuthNet.Game.XboxAuthStrategies
{
    public class XboxSisuAuthStrategy : MicrosoftXboxAuthStrategy
    {
        private readonly string _clientId;
        private readonly string _tokenPrefix;
        private readonly string _deviceType;
        private readonly string _deviceVersion;

        public XboxSisuAuthStrategy(
            HttpClient httpClient, 
            IMicrosoftOAuthStrategy oAuth,
            string clientId,
            string tokenPrefix,
            string deviceType,
            string deviceVersion)
         : base(httpClient, oAuth) 
        {
            this._clientId = clientId;
            this._tokenPrefix = tokenPrefix;
            this._deviceType = deviceType;
            this._deviceVersion = deviceVersion;
        }

        protected override async Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth, string relyingParty)
        {
            var xboxAuthClient = new XboxAuthClient(HttpClient);
            var userToken = await xboxAuthClient.RequestSignedUserToken(oAuth.AccessToken!);
            if (string.IsNullOrEmpty(userToken.Token))
                throw new XboxAuthException("UserToken was empty", 0);

            var deviceToken = await xboxAuthClient.RequestDeviceToken(_deviceType, _deviceVersion);
            var sisuResponse = await xboxAuthClient.SisuAuth(new XboxSisuAuthRequest
            {
                AccessToken = userToken.Token,
                DeviceToken = deviceToken.Token,
                TokenPrefix = _tokenPrefix,
                ClientId = _clientId,
                RelyingParty = relyingParty,
            });

            var tokens = new XboxAuthTokens
            {
                UserToken = userToken,
                DeviceToken = deviceToken,
                TitleToken = sisuResponse.TitleToken,
                XstsToken = sisuResponse.AuthorizationToken,
            };
            return tokens;
        }
    }
}
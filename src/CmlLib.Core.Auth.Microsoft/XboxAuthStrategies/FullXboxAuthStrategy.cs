using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Requests;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class FullXboxAuthStrategy : MicrosoftXboxAuthStrategy
    {
        private readonly string _deviceType;
        private readonly string _deviceVersion;

        public FullXboxAuthStrategy(
            HttpClient httpClient, 
            IMicrosoftOAuthStrategy oAuth,
            string deivceType,
            string deviceVersion)
         : base(httpClient, oAuth)
        {
            this._deviceType = deivceType;
            this._deviceVersion = deviceVersion;
        }

        protected override async Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth, string relyingParty)
        {
            var xboxAuthClient = new XboxAuthClient(HttpClient);

            var deviceToken = await xboxAuthClient.RequestDeviceToken(_deviceType, _deviceVersion);
            var userToken = await xboxAuthClient.RequestUserToken(oAuth.AccessToken!);

            if (string.IsNullOrEmpty(userToken.Token))
                throw new XboxAuthException("UserToken was empty", 0);

            var xsts = await xboxAuthClient.RequestXsts(new XboxXstsRequest
            {
                UserToken = userToken.Token,
                DeviceToken = deviceToken.Token,
                RelyingParty = relyingParty
            });

            var tokens = new XboxAuthTokens
            {
                UserToken = userToken,
                DeviceToken = deviceToken,
                XstsToken = xsts
            };
            return tokens;
        }
    }
}
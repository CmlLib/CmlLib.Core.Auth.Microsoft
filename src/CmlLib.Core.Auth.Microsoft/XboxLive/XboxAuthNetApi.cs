using System.Threading.Tasks;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthNetApi : IXboxLiveApi
    {
        private readonly XboxAuth xbox;
        private readonly string? TokenPrefix;
        private readonly IXboxAuthTokenApi? deviceTokenApi;
        private readonly IXboxAuthTokenApi? titleTokenApi;

        public XboxAuthNetApi(XboxAuth xl) : this(xl, null, null, null)
        {

        }

        public XboxAuthNetApi(XboxAuth xl,
            string? tokenPrefix,
            IXboxAuthTokenApi? deviceTokenApi,
            IXboxAuthTokenApi? titleTokenApi)
        {
            this.xbox = xl;
            this.TokenPrefix = tokenPrefix;
            this.deviceTokenApi = deviceTokenApi;
            this.titleTokenApi = titleTokenApi;
        }

        /// <summary>
        /// get xsts token
        /// </summary>
        /// <param name="token">token returned by microsoft oauth</param>
        /// <param name="deviceToken"></param>
        /// <param name="titleToken"></param>
        /// <param name="xstsRelyingParty"></param>
        /// <returns></returns>
        public async Task<XboxAuthTokens> GetTokens(string token, XboxAuthTokens? previousTokens, string? xstsRelyingParty)
        {
            var rps = await xbox.ExchangeRpsTicketForUserToken(TokenPrefix + token)
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(rps.Token))
                throw new XboxAuthException("rps.Token was empty", 200);

            XboxAuthResponse? deviceToken = null; 
            XboxAuthResponse? titleToken = null;

            if (deviceTokenApi != null)
                deviceToken = await deviceTokenApi.GetToken(previousTokens?.DeviceToken);
            if (titleTokenApi != null)
                titleToken = await titleTokenApi.GetToken(previousTokens?.TitleToken);

            var xsts = await xbox.ExchangeTokensForXstsIdentity(
                userToken: rps.Token!,
                deviceToken?.Token,
                titleToken?.Token,
                xstsRelyingParty,
                null);

            return new XboxAuthTokens
            {
                DeviceToken = deviceToken,
                TitleToken = titleToken,
                XstsToken = xsts
            };
        }
    }
}

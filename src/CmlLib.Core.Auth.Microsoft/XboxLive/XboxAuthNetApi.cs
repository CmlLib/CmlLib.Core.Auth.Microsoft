using System.Threading.Tasks;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthNetApi : IXboxLiveApi
    {
        private readonly XboxAuth xbox;

        public XboxAuthNetApi(XboxAuth xl)
        {
            this.xbox = xl;
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
            var rps = await xbox.ExchangeRpsTicketForUserToken(token)
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(rps.Token))
                throw new XboxAuthException("rps.Token was empty", 200);

            string? deviceToken = null;
            string? titleToken = null;

            var xsts = await xbox.ExchangeTokensForXstsIdentity(
                userToken: rps.Token!,
                deviceToken,
                titleToken,
                xstsRelyingParty,
                null);

            return new XboxAuthTokens
            {
                XstsToken = xsts
            };
        }
    }
}

using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthNetApi : IXboxLiveApi
    {
        private readonly MicrosoftOAuth oAuth;
        private readonly XboxAuth xbox;

        public XboxAuthNetApi(MicrosoftOAuth auth, XboxAuth xl)
        {
            this.oAuth = auth;
            this.xbox = xl;
        }

        private MicrosoftOAuthAuthCode? authCode;

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthAuthCode code)
        {
            var result = this.oAuth.CheckOAuthCodeResult(uri, out code);
            this.authCode = code;
            return result;
        }

        public string CreateOAuthUrl()
        {
            return this.oAuth.CreateUrlForOAuth();
        }

        public Task<MicrosoftOAuthResponse> GetTokens()
        {
            if (this.authCode == null)
                throw new InvalidOperationException("authCode was null");

            return this.oAuth.GetTokens(this.authCode);
        }

        public Task<MicrosoftOAuthResponse> RefreshTokens(string token)
        {
            return this.oAuth.RefreshToken(token);
        }

        public async Task<XboxAuthResponse> GetXSTS(string token, string? deviceToken, string? titleToken, string? xstsRelyingParty)
        {
            var rps = await xbox.ExchangeRpsTicketForUserToken(token)
                .ConfigureAwait(false);

            var xsts = await xbox.ExchangeTokensForXstsIdentity(
                userToken: rps.Token, 
                deviceToken,
                titleToken,
                xstsRelyingParty,
                null);
            return xsts;
        }
    }
}

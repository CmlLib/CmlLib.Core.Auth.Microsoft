using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public class MicrosoftOAuthApiWithWebUI : MicrosoftOAuthApi
    {
        private readonly IWebUI _webUI;

        public MicrosoftOAuthApiWithWebUI(IWebUI webUI, MicrosoftOAuth oa) : base(oa)
        {
            this._webUI = webUI;
        }

        public override async Task<MicrosoftOAuthResponse> RequestNewTokens()
        {
            var loginHandler = new MicrosoftOAuthWebUILoginHandler(_oAuth);
            var authCode = await _webUI.GetAuthCode(loginHandler);
            var tokens = await _oAuth.GetTokens(authCode);
            return tokens;
        }
    }
}

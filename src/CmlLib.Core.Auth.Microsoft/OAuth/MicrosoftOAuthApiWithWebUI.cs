using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public class MicrosoftOAuthApiWithWebUI : MicrosoftOAuthApi
    {
        private readonly IWebUI _webUI;
        private readonly MicrosoftOAuthParameters _parameters;

        public MicrosoftOAuthApiWithWebUI(
            IWebUI webUI,
            MicrosoftOAuth oa)
            : this(webUI, oa, new MicrosoftOAuthParameters())
        {

        }

        public MicrosoftOAuthApiWithWebUI(
            IWebUI webUI, 
            MicrosoftOAuth oa,
            MicrosoftOAuthParameters parameters) 
            : base(oa)
        {
            this._webUI = webUI;
            this._parameters = parameters;
        }

        public override async Task<MicrosoftOAuthResponse> RequestNewTokens(CancellationToken cancellationToken)
        {
            var loginHandler = new MicrosoftOAuthWebUILoginHandler(_oAuth, _parameters);
            var authCode = await _webUI.GetAuthCode(loginHandler, cancellationToken);

            if (!authCode.IsSuccess)
            {
                throw new LoginCancelledException(authCode.Error, authCode.ErrorDescription ?? authCode.Error);
            }

            var tokens = await _oAuth.GetTokens(authCode);
            return tokens;
        }

        public override async Task InvalidateTokens()
        {
            var uri = new Uri(MicrosoftOAuth.GetSignOutUrl());
            await _webUI.ShowUri(uri, CancellationToken.None);
        }
    }
}

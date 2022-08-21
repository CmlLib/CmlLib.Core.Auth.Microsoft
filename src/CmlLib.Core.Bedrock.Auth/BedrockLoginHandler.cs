using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockLoginHandler : 
        AbstractLoginHandler<BedrockSessionCache>
    {
        private readonly IXboxLiveApi _xboxLiveApi;
        private readonly IBedrockXboxApi _bedrockApi;
        private readonly string _relyingParty;

        public BedrockLoginHandler(
            IMicrosoftOAuthApi oauthApi,
            IXboxLiveApi xboxLiveApi,
            IBedrockXboxApi bedrockApi,
            ICacheManager<BedrockSessionCache> cacheManager,
            string relyingParty) : 
            base(oauthApi, cacheManager)
        {
            this._xboxLiveApi = xboxLiveApi;
            this._bedrockApi = bedrockApi;
            this._relyingParty = relyingParty;
        }

        protected override async Task<BedrockSessionCache> GetAllTokens(MicrosoftOAuthResponse msToken)
        {
            if (string.IsNullOrEmpty(msToken.AccessToken))
                throw new ArgumentException("msToken.AccessToken was empty");

            var xsts = await _xboxLiveApi.GetXSTS(msToken.AccessToken!, null, null, _relyingParty);
            if (string.IsNullOrEmpty(xsts.Token))
                throw new XboxAuthException("xsts was empty", 200);
            if (string.IsNullOrEmpty(xsts.UserHash))
                throw new XboxAuthException("uhs was empty", 200);

            var bedrockTokens = await _bedrockApi.LoginWithXbox(xsts.UserHash, xsts.Token);

            return new BedrockSessionCache
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                BedrockTokens = bedrockTokens
            };
        }
    }
}

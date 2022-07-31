using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockLoginHandler : 
        AbstractLoginHandler<BedrockSessionCache>
    {
        private readonly IBedrockXboxApi _bedrockApi;
        private readonly string _relyingParty;

        public BedrockLoginHandler(
            IXboxLiveApi xboxLiveApi, 
            IBedrockXboxApi bedrockApi,
            ICacheManager<BedrockSessionCache> cacheManager,
            string relyingParty) : 
            base(xboxLiveApi, cacheManager)
        {
            this._bedrockApi = bedrockApi;
            this._relyingParty = relyingParty;
        }

        public override async Task<BedrockSessionCache> GetAllTokens(MicrosoftOAuthResponse msToken)
        {
            var xsts = await GetXsts(msToken, null, null, _relyingParty);
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

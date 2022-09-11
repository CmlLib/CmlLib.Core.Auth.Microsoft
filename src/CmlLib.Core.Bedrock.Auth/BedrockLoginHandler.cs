using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockLoginHandler : 
        AbstractLoginHandler<BedrockSessionCache>
    {
        private readonly IBedrockXboxApi _bedrockApi;

        public BedrockLoginHandler(
            LoginHandlerParameters parameters,
            IBedrockXboxApi bedrockApi,
            ICacheManager<BedrockSessionCache> cacheManager) : 
            base(parameters, cacheManager)
        {
            this._bedrockApi = bedrockApi;
        }

        protected override async Task<BedrockSessionCache> GetAllTokens(XboxAuthResponse xsts, CancellationToken cancellationToken)
        {
            var bedrockTokens = await _bedrockApi.LoginWithXbox(xsts.UserHash!, xsts.Token!);

            return new BedrockSessionCache
            {
                BedrockTokens = bedrockTokens
            };
        }
    }
}

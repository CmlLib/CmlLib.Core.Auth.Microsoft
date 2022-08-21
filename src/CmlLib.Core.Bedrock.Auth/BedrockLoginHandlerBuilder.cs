using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.XboxLive;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockLoginHandlerBuilder
        : AbstractLoginHandlerBuilder<BedrockLoginHandlerBuilder, BedrockSessionCache>
    {
        public BedrockLoginHandlerBuilder(HttpClient httpClient) : base(httpClient)
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_bedrock.json");
            WithCacheManager(new JsonFileCacheManager<BedrockSessionCache>(defaultPath));
            XboxLiveApi = new XboxAuthNetApi(new XboxAuthNet.XboxLive.XboxAuth(httpClient));
            BedrockXboxApi = new BedrockXboxApi(httpClient);
        }

        public string XboxRelyingParty { get; private set; } = "https://multiplayer.minecraft.net/";
        private IXboxLiveApi XboxLiveApi { get; set; }
        private IBedrockXboxApi BedrockXboxApi { get; set; }

        public BedrockLoginHandlerBuilder WithBedrockXboxApi(IBedrockXboxApi bedrockApi)
        {
            this.BedrockXboxApi = bedrockApi;
            return this;
        }

        public BedrockLoginHandlerBuilder WithXboxRelyingParty(string relyingParty)
        {
            this.XboxRelyingParty = relyingParty;
            return this;
        }

        private BedrockLoginHandler BuildConcrete()
        {
            if (MicrosoftOAuthApi == null)
                throw new InvalidOperationException("MicrosoftOAuthApi was null");

            if (CacheManager == null)
                throw new InvalidOperationException("CacheManager was null");

            return new BedrockLoginHandler(
                oauthApi: MicrosoftOAuthApi,
                xboxLiveApi: XboxLiveApi,
                bedrockApi: BedrockXboxApi,
                cacheManager: CacheManager,
                relyingParty: XboxRelyingParty);
        }

        protected override AbstractLoginHandler<BedrockSessionCache> BuildInternal()
        {
            return BuildConcrete();
        }

        public new BedrockLoginHandler Build()
        {
            return (BedrockLoginHandler)base.Build();
        }
    }
}

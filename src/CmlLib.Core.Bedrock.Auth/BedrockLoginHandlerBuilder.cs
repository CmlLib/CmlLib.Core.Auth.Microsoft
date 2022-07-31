using CmlLib.Core.Auth.Microsoft;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockLoginHandlerBuilder
        : AbstractLoginHandlerBuilder<BedrockLoginHandlerBuilder, BedrockSessionCache>
    {
        public BedrockLoginHandlerBuilder(string cid, HttpClient httpClient) : base(cid, httpClient)
        {
            SetJsonCacheManager("cml_bedrock.json");
            BedrockXboxApi = new BedrockXboxApi(httpClient);
        }

        protected string XboxRelyingParty { get; private set; } = "https://multiplayer.minecraft.net/";
        protected IBedrockXboxApi BedrockXboxApi { get; private set; }

        public BedrockLoginHandlerBuilder SetBedrockXboxApi(IBedrockXboxApi bedrockApi)
        {
            this.BedrockXboxApi = bedrockApi;
            return this;
        }

        public BedrockLoginHandlerBuilder SetXboxRelyingParty(string relyingParty)
        {
            this.XboxRelyingParty = relyingParty;
            return this;
        }

        public BedrockLoginHandler Build()
        {
            return new BedrockLoginHandler(
                xboxLiveApi: XboxLiveApi,
                bedrockApi: BedrockXboxApi,
                cacheManager: CacheManager,
                relyingParty: XboxRelyingParty);
        }
    }
}

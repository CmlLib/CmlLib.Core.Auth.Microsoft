using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Cache;
using System;
using System.IO;
using System.Net.Http;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockLoginHandlerBuilder
        : AbstractLoginHandlerBuilder<BedrockLoginHandlerBuilder, BedrockSessionCache>
    {
        public BedrockLoginHandlerBuilder(HttpClient httpClient) : base(httpClient)
        {
            Context.CachePath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_bedrock.json");
            WithCacheManager(new JsonFileCacheManager<BedrockSessionCache>(Context.CachePath));
            BedrockXboxApi = new BedrockXboxApi(httpClient);
            WithRelyingParty("https://multiplayer.minecraft.net/");
        }

        private IBedrockXboxApi BedrockXboxApi { get; set; }

        public BedrockLoginHandlerBuilder WithBedrockXboxApi(IBedrockXboxApi bedrockApi)
        {
            this.BedrockXboxApi = bedrockApi;
            return this;
        }

        private BedrockLoginHandler BuildConcrete(LoginHandlerParameters parameters)
        {
            if (CacheManager == null)
                throw new InvalidOperationException("CacheManager was null");

            return new BedrockLoginHandler(
                parameters,
                bedrockApi: BedrockXboxApi,
                cacheManager: CacheManager);
        }

        protected override AbstractLoginHandler<BedrockSessionCache> BuildInternal(LoginHandlerParameters parameters)
        {
            return BuildConcrete(parameters);
        }

        public new BedrockLoginHandler Build()
        {
            return (BedrockLoginHandler)base.Build();
        }
    }
}

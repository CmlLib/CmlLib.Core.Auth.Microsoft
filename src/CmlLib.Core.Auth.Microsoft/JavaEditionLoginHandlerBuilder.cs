using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Net.Http;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JavaEditionLoginHandlerBuilder
        : AbstractLoginHandlerBuilder<JavaEditionLoginHandlerBuilder, JavaEditionSessionCache>
    {
        public static readonly string DefaultClientId = "00000000402B5328";

        public JavaEditionLoginHandlerBuilder()
            : this(HttpHelper.DefaultHttpClient.Value)
        {

        }

        public JavaEditionLoginHandlerBuilder(HttpClient httpClient)
            : base(httpClient)
        {
            this.XboxLiveApi = new XboxAuthNetApi(new XboxAuth(httpClient));
            this.MojangXboxApi = new MojangXboxApi(httpClient);
        }

        private IXboxLiveApi XboxLiveApi { get; set; }
        private IMojangXboxApi MojangXboxApi { get; set; }
        public string XboxRelyingParty { get; set; } = Mojang.MojangXboxApi.RelyingParty;

        public override string GetDefaultClientId() => DefaultClientId;

        public JavaEditionLoginHandlerBuilder WithXboxLiveApi(IXboxLiveApi xboxApi)
        {
            this.XboxLiveApi = xboxApi;
            return this;
        }

        public JavaEditionLoginHandlerBuilder WithXboxRelyingParty(string relyingParty)
        {
            this.XboxRelyingParty = relyingParty;
            return this;
        }

        public JavaEditionLoginHandlerBuilder WithMojangXboxApi(IMojangXboxApi mojangApi)
        {
            this.MojangXboxApi = mojangApi;
            return this;
        }

        public JavaEditionLoginHandler Build()
        {
            if (MicrosoftOAuthApi == null)
                throw new InvalidOperationException("MicrosoftOAuthApi null");

            return new JavaEditionLoginHandler(
                oauthApi : MicrosoftOAuthApi,
                xboxLiveApi : XboxLiveApi,
                cacheManager: CacheManager,
                mojangXboxApi : MojangXboxApi,
                relyingParty : XboxRelyingParty
            );
        }
    }
}

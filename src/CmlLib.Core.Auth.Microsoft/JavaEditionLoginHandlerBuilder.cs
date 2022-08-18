using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System.Net.Http;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JavaEditionLoginHandlerBuilder
        : AbstractLoginHandlerBuilder<JavaEditionLoginHandlerBuilder, JavaEditionSessionCache>
    {
        public static readonly string DefaultClientId = "00000000402B5328";

        public IXboxLiveApi XboxLiveApi { get; private set; }
        public IMojangXboxApi MojangXboxApi { get; private set; }
        public string XboxRelyingParty { get; set; } = Mojang.MojangXboxApi.RelyingParty;

        public JavaEditionLoginHandlerBuilder(string cid, HttpClient httpClient)
            : base(cid, httpClient)
        {
            this.XboxLiveApi = new XboxAuthNetApi(new XboxAuth(httpClient));
            this.MojangXboxApi = new MojangXboxApi(httpClient);
        }

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

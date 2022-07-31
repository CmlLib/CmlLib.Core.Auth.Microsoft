using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using System.Net.Http;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JavaEditionLoginHandlerBuilder : AbstractLoginHandlerBuilder<JavaEditionLoginHandlerBuilder, JavaEditionSessionCache>
    {
        public static readonly string DefaultClientId = "00000000402B5328";

        public string XboxRelyingParty { get; set; } = Mojang.MojangXboxApi.RelyingParty;
        public IMojangXboxApi MojangXboxApi { get; private set; }

        internal JavaEditionLoginHandlerBuilder(string cid, HttpClient httpClient) : base(cid, httpClient)
        {
            this.MojangXboxApi = new MojangXboxApi(httpClient);
        }

        public JavaEditionLoginHandlerBuilder SetMojangXboxApi(IMojangXboxApi mojangApi)
        {
            this.MojangXboxApi = mojangApi;
            return this;
        }

        public JavaEditionLoginHandlerBuilder SetXboxRelyingParty(string relyingParty)
        {
            this.XboxRelyingParty = relyingParty;
            return this;
        }

        public JavaEditionLoginHandler Build()
        {
            return new JavaEditionLoginHandler(
                xboxLiveApi : XboxLiveApi,
                cacheManager: CacheManager,
                mojangXboxApi : MojangXboxApi,
                relyingParty : XboxRelyingParty
            );
        }
    }
}

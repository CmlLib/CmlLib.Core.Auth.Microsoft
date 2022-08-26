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
        public static readonly string MojangClientId = "00000000402B5328";

        public JavaEditionLoginHandlerBuilder()
            : this(HttpHelper.DefaultHttpClient.Value)
        {

        }

        public JavaEditionLoginHandlerBuilder(HttpClient httpClient)
            : base(httpClient)
        {
            this.MojangXboxApi = new MojangXboxApi(httpClient);
        }

        public IMojangXboxApi MojangXboxApi { get; private set; }

        public override bool IsDefaultClientIdAvailable => true;
        protected override string GetDefaultClientId() => MojangClientId;

        public JavaEditionLoginHandlerBuilder WithMojangXboxApi(IMojangXboxApi mojangApi)
        {
            this.MojangXboxApi = mojangApi;
            return this;
        }

        private JavaEditionLoginHandler BuildConcrete(LoginHandlerParameters parameters)
        {
            if (CacheManager == null)
                throw new InvalidOperationException("CacheManager was null");

            return new JavaEditionLoginHandler(
                parameters,
                MojangXboxApi,
                CacheManager
            );
        }

        protected override AbstractLoginHandler<JavaEditionSessionCache> BuildInternal(LoginHandlerParameters parameters)
        {
            return BuildConcrete(parameters);
        }

        public new JavaEditionLoginHandler Build()
        {
            return (JavaEditionLoginHandler)base.Build();
        }
    }
}

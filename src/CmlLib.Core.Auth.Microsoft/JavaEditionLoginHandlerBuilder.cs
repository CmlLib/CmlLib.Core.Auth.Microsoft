using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using System;
using System.Net.Http;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JavaEditionLoginHandlerBuilder
        : AbstractLoginHandlerBuilder<JavaEditionLoginHandlerBuilder, JavaEditionSessionCache>
    {
        public JavaEditionLoginHandlerBuilder(LoginHandlerBuilderContext context)
            : base(context)
        {
            if (string.IsNullOrEmpty(this.Context.ClientId))
                this.Context.ClientId = XboxAuthNet.XboxLive.XboxGameTitles.MinecraftJava;

            this.MojangXboxApi = new MojangXboxApi(context.HttpClient);
            WithRelyingParty(Mojang.MojangXboxApi.RelyingParty);
        }

        public IMojangXboxApi MojangXboxApi { get; private set; }

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

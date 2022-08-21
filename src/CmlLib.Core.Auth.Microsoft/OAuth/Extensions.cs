using CmlLib.Core.Auth.Microsoft.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public static class Extensions
    {
        public static TBuilder WithMicrosoftOAuthApi<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder, 
            MicrosoftOAuthApiBuilder msBuilder)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            msBuilder.WithHttpClient(builder.HttpClient);
            var microsoftOAuthApi = msBuilder.Build();
            return builder.WithMicrosoftOAuthApi(microsoftOAuthApi);
        }

        public static TBuilder WithMicrosoftOAuthApi<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            Func<MicrosoftOAuthApiBuilder, MicrosoftOAuthApiBuilder> msOAuthConfig)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            var msBuilder = msOAuthConfig.Invoke(MicrosoftOAuthApiBuilder.Create(builder.DefaultClientId));
            return builder.WithMicrosoftOAuthApi(msBuilder);
        }
    }
}

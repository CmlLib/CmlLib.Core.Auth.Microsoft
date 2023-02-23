using System;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public static class XboxAuthStrategyFactoryExtensions
    {
        public static T WithXboxAuth<T>(
            this IBuilderWithXboxAuthStrategy<T> builder, 
            Func<XboxAuthStrategyFactory, IXboxAuthStrategy> factoryInvoker)
        {
            var context = builder.XboxAuthStrategyBuilderContext;
            var factory = new XboxAuthStrategyFactory(context.HttpClient, context.OAuthStrategy);
            var strategy = factoryInvoker.Invoke(factory);
            return builder.WithXboxAuth(strategy);
        }

        public static T WithBasicXboxAuth<T>(this IBuilderWithXboxAuthStrategy<T> builder)
        {
            return builder.WithXboxAuth<T>(factory => 
            {
                return factory.CreateBasicXboxAuth();
            });
        }
    }
}
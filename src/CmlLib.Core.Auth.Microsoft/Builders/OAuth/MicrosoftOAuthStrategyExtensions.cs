using System;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public static class MicrosoftOAuthStrategyExtensions
    {
        public static T WithMicrosoftOAuth<T>(
            this IBuilderWithMicrosoftOAuthStrategy<T> builder,
            Func<MicrosoftOAuthStrategyFactory, IMicrosoftOAuthStrategy> factoryInvoker)
        {
            var context = builder.MicrosoftOAuthBuilderContext;
            var apiClient = createMicrosoftOAuthApiClient(context);
            var factory = new MicrosoftOAuthStrategyFactory(apiClient);
            factory.SessionSource = context.SessionSource;

            var strategy = factoryInvoker.Invoke(factory);
            return builder.WithMicrosoftOAuth(strategy);
        }

        public static T WithSilentMicrosoftOAuth<T>(this IBuilderWithMicrosoftOAuthStrategy<T> builder)
        {
            return builder.WithMicrosoftOAuth<T>(factory => {
                var strategy = factory.CreateSilentStrategy();
                return factory.CreateCachingStrategy(strategy);
            });
        }

        public static T WithInteractiveMicrosoftOAuth<T>(this IBuilderWithMicrosoftOAuthStrategy<T> builder)
        {
            return builder.WithInteractiveMicrosoftOAuth<T>(builder => builder, new MicrosoftOAuthParameters());
        }

        public static T WithInteractiveMicrosoftOAuth<T>(
            this IBuilderWithMicrosoftOAuthStrategy<T> builder,
            MicrosoftOAuthParameters parameters)
        {
            return builder.WithInteractiveMicrosoftOAuth<T>(builder => builder, parameters);
        }

        public static T WithInteractiveMicrosoftOAuth<T>(
            this IBuilderWithMicrosoftOAuthStrategy<T> builder, 
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker)
        {
            return builder.WithInteractiveMicrosoftOAuth<T>(builderInvoker, new MicrosoftOAuthParameters());
        }

        public static T WithInteractiveMicrosoftOAuth<T>(
            this IBuilderWithMicrosoftOAuthStrategy<T> builder,
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker, 
            MicrosoftOAuthParameters parameters)
        {
            return builder.WithMicrosoftOAuth<T>(factory => {
                var strategy = factory.CreateInteractiveStrategy(builderInvoker, parameters);
                return factory.CreateCachingStrategy(strategy);
            });
        }

        private static MicrosoftOAuthCodeApiClient createMicrosoftOAuthApiClient(MicrosoftOAuthStrategyFactoryContext context)
        {
            var apiClient = new MicrosoftOAuthCodeApiClient(
                context.ClientId ?? throw new InvalidOperationException(),
                context.Scopes ?? throw new InvalidOperationException(),
                context.HttpClient ?? HttpHelper.DefaultHttpClient.Value);
            return apiClient;
        }
    }
}
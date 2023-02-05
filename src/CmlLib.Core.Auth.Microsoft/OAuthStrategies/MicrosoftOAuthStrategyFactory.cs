using System;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using CmlLib.Core.Auth.Microsoft.Cache;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class MicrosoftOAuthStrategyFactory
    {
        private readonly MicrosoftOAuthCodeApiClient _apiClient;
        private readonly ICacheStorage<MicrosoftOAuthResponse> _cacheStorage;

        public MicrosoftOAuthStrategyFactory(
            MicrosoftOAuthCodeApiClient apiClient,
            ICacheStorage<MicrosoftOAuthResponse> cacheStorage)
        {
            this._apiClient = apiClient;
            this._cacheStorage = cacheStorage;
        }

        public IMicrosoftOAuthStrategy CreateInteractiveOAuth(
            MicrosoftOAuthParameters parameters)
        {
            return CreateInteractiveOAuth(builder => builder, parameters);
        }

        public IMicrosoftOAuthStrategy CreateInteractiveOAuth(
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builder,
            MicrosoftOAuthParameters parameters)
        {
            var codeFlowBuilder = new MicrosoftOAuthCodeFlowBuilder(_apiClient);
            var codeFlow = builder.Invoke(codeFlowBuilder).Build();
            var strategy = new InteractiveMicrosoftOAuthStrategy(codeFlow, parameters);
            return WithCaching(strategy);
        }

        public IMicrosoftOAuthStrategy CreateSilentOAuth()
        {
            var strategy = new SilentMicrosoftOAuthStrategy(_apiClient, _cacheStorage);
            return WithCaching(strategy);
        }

        // CreateDeviceCodeOAuth(), or etc...

        private IMicrosoftOAuthStrategy WithCaching(IMicrosoftOAuthStrategy strategy)
        {
            var cacheStrategy = new CachingMicrosoftOAuthResponseStrategy(strategy, _cacheStorage);
            return cacheStrategy;
        }
    }
}
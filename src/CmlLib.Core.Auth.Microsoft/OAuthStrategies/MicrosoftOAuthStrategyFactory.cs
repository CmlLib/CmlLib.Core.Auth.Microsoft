using System;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class MicrosoftOAuthStrategyFactory : IMicrosoftOAuthStrategyFactory
    {
        private readonly MicrosoftOAuthCodeApiClient _oAuthClient;

        public MicrosoftOAuthStrategyFactory(MicrosoftOAuthCodeApiClient apiClient) =>
            _oAuthClient = apiClient;

        public IMicrosoftOAuthStrategy CreateSilentStrategy(ISessionSource<MicrosoftOAuthResponse> sessionSource)
        {
            return new SilentMicrosoftOAuthStrategy(_oAuthClient, sessionSource);
        }

        public IMicrosoftOAuthStrategy CreateInteractiveStrategy(
            MicrosoftOAuthCodeFlow codeFlow, 
            MicrosoftOAuthParameters parameters)
        {
            return new InteractiveMicrosoftOAuthStrategy(codeFlow, parameters);
        }

        public IMicrosoftOAuthStrategy CreateCachingStrategy(
            IMicrosoftOAuthStrategy strategy,
            ISessionSource<MicrosoftOAuthResponse> sessionSource)
        {
            return new CachingMicrosoftOAuthStrategy(strategy, sessionSource);
        }
    }
}
using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxAuthStrategyFactory
    {
        private readonly HttpClient _httpClient;
        private readonly IMicrosoftOAuthStrategy _oAuthStrategy;

        public XboxAuthStrategyFactory(
            HttpClient httpClient,
            IMicrosoftOAuthStrategy oAuthStrategy) =>
            (_httpClient, _oAuthStrategy) = (httpClient, oAuthStrategy);

        public IXboxAuthStrategy CreateBasicXboxAuth()
        {
            return new BasicXboxAuthStrategy(_httpClient, _oAuthStrategy);
        }
    }
}
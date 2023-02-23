using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxAuthStrategyFactoryContext
    {
        public HttpClient? HttpClient { get; set; }
        public IMicrosoftOAuthStrategy? OAuthStrategy { get; set; }
    }
}
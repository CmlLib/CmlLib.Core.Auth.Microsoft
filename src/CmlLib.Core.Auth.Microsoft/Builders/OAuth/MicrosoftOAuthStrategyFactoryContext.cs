using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftOAuthStrategyFactoryContext
    {
        public HttpClient? HttpClient { get; set; }
        public ISessionSource<MicrosoftOAuthResponse>? SessionSource { get; set; }
        public string? ClientId { get; set; }
        public string? Scopes { get; set; }
    }
}
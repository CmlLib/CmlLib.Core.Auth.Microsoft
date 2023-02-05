using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.Executors;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameAuthenticationParameters
    {
        public HttpClient? HttpClient { get; set; }
        public ICacheStorage<XboxGameSession>? CacheStorage { get; set; }
        public IXboxAuthStrategy? XboxAuthStrategy { get; set; }
        public IXboxGameAuthenticator? GameAuthenticator { get; set; }
        public IXboxGameAuthenticationExecutor? Executor { get; set; }
    }
}
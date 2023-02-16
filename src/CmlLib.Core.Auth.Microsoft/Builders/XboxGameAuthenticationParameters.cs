using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameAuthenticationParameters
    {
        public HttpClient? HttpClient { get; set; }
        public ISessionStorage? SessionStorage { get; set; }
        public IXboxAuthStrategy? XboxAuthStrategy { get; set; }
        public IXboxGameAuthenticator? GameAuthenticator { get; set; }
    }
}
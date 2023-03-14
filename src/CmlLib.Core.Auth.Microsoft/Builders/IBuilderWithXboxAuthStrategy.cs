using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public interface IBuilderWithXboxAuthStrategy
    {
        public HttpClient HttpClient { get; set; }
        public ISessionStorage SessionStorage { get; set; }
        IXboxAuthStrategy? XboxAuthStrategy { get; set; }
    }
}
using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public interface IXboxAuthStrategyBuilder
    {
        public HttpClient HttpClient { get; set; }
        public ISessionStorage SessionStorage { get; set; }

        public IXboxAuthStrategy Build();
    }
}
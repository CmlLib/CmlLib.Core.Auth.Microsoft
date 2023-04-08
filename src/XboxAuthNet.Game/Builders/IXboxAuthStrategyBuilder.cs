using System.Net.Http;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.Builders
{
    public interface IXboxAuthStrategyBuilder
    {
        public HttpClient HttpClient { get; set; }
        public ISessionStorage SessionStorage { get; set; }

        public IXboxAuthStrategy Build();
    }
}
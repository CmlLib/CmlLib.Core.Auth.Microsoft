using System.Net.Http;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Builders
{
    public interface IBuilderWithXboxAuthStrategy
    {
        public HttpClient HttpClient { get; set; }
        public ISessionStorage SessionStorage { get; set; }
        IXboxAuthStrategy? XboxAuthStrategy { get; set; }
    }
}
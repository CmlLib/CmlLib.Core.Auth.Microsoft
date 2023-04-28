using System.Net.Http;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game
{
    public class LoginHandlerBuilderBase
    {
        private HttpClient? _httpClient;
        public HttpClient HttpClient
        {
            get => _httpClient ??= HttpHelper.DefaultHttpClient.Value;
        }

        public ISessionStorage? SessionStorage { get; set; }
    }
}

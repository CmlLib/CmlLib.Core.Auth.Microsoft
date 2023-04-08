using System.Net.Http;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game
{
    public class LoginHandlerBuilder
    {
        public static LoginHandlerBuilder Create()
        {
            return new LoginHandlerBuilder();
        }

        private LoginHandlerBuilder() 
        {
            
        }

        private HttpClient? innerHttpClient;
        public HttpClient HttpClient 
        {
            get => innerHttpClient ?? HttpHelper.DefaultHttpClient.Value;
            set => innerHttpClient = value;
        }

        public ISessionStorage? SessionStorage;

        public LoginHandlerBuilder WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public LoginHandlerBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this.SessionStorage = sessionStorage;
            return this;
        }
    }
}
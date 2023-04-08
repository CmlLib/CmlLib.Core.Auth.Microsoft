using System.IO;
using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft
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

        public string DefaultSessionStoragePath => Path.Combine(MinecraftPath.GetOSDefaultPath(), "cmllib_session.json");

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

        public JELoginHandler ForJavaEdition()
        {
            var sessionStorage = this.SessionStorage ?? new JsonFileSessionStorage(DefaultSessionStoragePath);
            return new JELoginHandler(HttpClient, sessionStorage);
        }
    }
}
using System;
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

        private HttpClient? innerHttpClient;
        public HttpClient HttpClient 
        {
            get => innerHttpClient ?? HttpHelper.DefaultHttpClient.Value;
            set => innerHttpClient = value;
        }

        private ISessionStorage? innerSessionStorage;
        public ISessionStorage SessionStorage
        {
            get => innerSessionStorage ?? createDefaultSessionStorage();
            set => innerSessionStorage = value;
        }

        private ISessionStorage createDefaultSessionStorage()
        {
            var defaultSessionStoragePath = System.IO.Path.Combine(MinecraftPath.WindowsDefaultPath, "cmllib_session.json");
            return new JsonFileSessionStorage(defaultSessionStoragePath);
        }

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
            return new JELoginHandler(HttpClient, SessionStorage);
        }
    }
}
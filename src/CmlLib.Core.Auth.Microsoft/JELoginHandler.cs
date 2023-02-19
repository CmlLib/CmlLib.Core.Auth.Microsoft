using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JELoginHandler
    {
        public static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new MicrosoftOAuthClientInfo
        {
            ClientId = "",
            Scopes = ""
        };

        private readonly HttpClient _httpClient;
        public ISessionStorage? SessionStorage { get; set; }

        public JELoginHandler(
            HttpClient httpClient, 
            ISessionStorage sessionStorage) =>
            (_httpClient, SessionStorage) = (httpClient, sessionStorage);

        public JEAuthBuilder Authenticate()
        {
            var parameters = new XboxGameAuthenticationParameters(_httpClient);
            parameters.SessionStorage = SessionStorage;
            return new JEAuthBuilder(parameters);
        }
    }
}
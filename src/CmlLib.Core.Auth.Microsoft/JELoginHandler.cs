using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Builders;

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
        private readonly ISessionStorage _cacheStorage;

        public JELoginHandler(
            HttpClient httpClient, 
            ISessionStorage sessionStorage) =>
            (_httpClient, _cacheStorage) = (httpClient, sessionStorage);

        public JEAuthenticationBuilder Authenticate()
        {
            return new JEAuthenticationBuilder(createParameters());
        }

        public JESilentAuthenticationBuilder AuthenticateSilently()
        {
            return new JESilentAuthenticationBuilder(createParameters());
        }

        private XboxGameAuthenticationParameters createParameters()
        {
            var parameters = new XboxGameAuthenticationParameters();
            parameters.HttpClient = _httpClient;
            parameters.SessionStorage = _cacheStorage;
            return parameters;
        }
    }
}
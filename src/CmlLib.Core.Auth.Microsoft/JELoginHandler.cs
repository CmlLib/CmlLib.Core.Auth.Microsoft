using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.Builders.OAuth;

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

        public XboxGameAuthenticationBuilder Authenticate()
        {
            return new XboxGameAuthenticationBuilder(createParameters(), DefaultMicrosoftOAuthClientInfo);
        }

        public SilentXboxGameAuthenticationBuilder AuthenticateSilently()
        {
            return new SilentXboxGameAuthenticationBuilder(createParameters(), DefaultMicrosoftOAuthClientInfo);
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
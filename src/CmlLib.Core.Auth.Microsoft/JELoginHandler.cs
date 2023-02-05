using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.Cache;
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
        private readonly ICacheStorage<XboxGameSession> _cacheStorage;

        public JELoginHandler(
            HttpClient httpClient, 
            ICacheStorage<XboxGameSession> cacheStorage)
        {
            this._cacheStorage = cacheStorage;
            this._httpClient = httpClient;
        }

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
            parameters.CacheStorage = _cacheStorage;
            return parameters;
        }
    }
}
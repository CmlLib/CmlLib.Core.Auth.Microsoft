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
        private readonly ISessionStorage _cacheStorage;

        public JELoginHandler(
            HttpClient httpClient, 
            ISessionStorage sessionStorage) =>
            (_httpClient, _cacheStorage) = (httpClient, sessionStorage);

        public XboxGameAuthenticationBuilder Authenticate()
        {
            return new XboxGameAuthenticationBuilder(createParameters(), DefaultMicrosoftOAuthClientInfo)
                .WithGameAuthenticator(createGameAuthenticator())
                .WithExecutor(builder => builder // define default behavior
                    .WithMicrosoftOAuth()
                    .ExecuteAsync());
        }

        public XboxGameAuthenticationBuilder AuthenticateSilently()
        {
            return new XboxGameAuthenticationBuilder(createParameters(), DefaultMicrosoftOAuthClientInfo)
                .WithGameAuthenticator(createSilentGameAuthenticator())
                .WithExecutor(builder => builder // define default behavior
                    .WithSilentMicrosoftOAuth()
                    .ExecuteAsync());
        }

        private XboxGameAuthenticationParameters createParameters()
        {
            var parameters = new XboxGameAuthenticationParameters();
            parameters.HttpClient = _httpClient;
            parameters.SessionStorage = _cacheStorage;
            return parameters;
        }

        private IXboxGameAuthenticator createGameAuthenticator()
        {
            return new DummyGameAuthenticator();
        }

        private IXboxGameAuthenticator createSilentGameAuthenticator()
        {
            var authenticator = createGameAuthenticator();
            return new SilentXboxGameAuthenticator(new InMemorySessionSource<XboxGameSession>(), authenticator);
        }
    }
}
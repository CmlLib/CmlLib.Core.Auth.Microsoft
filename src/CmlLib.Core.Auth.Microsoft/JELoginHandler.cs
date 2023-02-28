using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.Executors;

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
        public ISessionStorage SessionStorage { get; set; }

        public JELoginHandler(
            HttpClient httpClient, 
            ISessionStorage sessionStorage) =>
            (_httpClient, SessionStorage) = (httpClient, sessionStorage);

        public Task<XboxGameSession> Authenticate()
        {
            try
            {
                return AuthenticateSilently().ExecuteAsync();
            }
            catch (SessionExpiredException)
            {
                return AuthenticateInteractively().ExecuteAsync();
            }
        }

        public XboxGameAuthenticationBuilder AuthenticateInteractively()
        {
            return createAuthenticationBuilder()
                .WithSessionStorage(SessionStorage)
                .WithGameAuthenticator(createGameAuthenticator())
                .WithInteractiveMicrosoftOAuth()
                .WithBasicXboxAuth();
        }

        public XboxGameAuthenticationBuilder AuthenticateSilently()
        {
            return createAuthenticationBuilder()
                .WithSessionStorage(SessionStorage)
                .WithGameAuthenticator(createSilentGameAuthenticator())
                .WithSilentMicrosoftOAuth()
                .WithBasicXboxAuth();
        }

        private IXboxGameAuthenticator createGameAuthenticator()
        {
            return new DummyGameAuthenticator();
        }

        private IXboxGameAuthenticator createSilentGameAuthenticator()
        {
            return new SilentXboxGameAuthenticator(createGameAuthenticator());
        }

        private XboxGameAuthenticationBuilder createAuthenticationBuilder()
        {
            var oAuthContext = createOAuthContext();
            var xboxAuthContext = createXboxAuthContext();
            return new XboxGameAuthenticationBuilder(new XboxGameAuthenticationExecutor(), oAuthContext, xboxAuthContext);
        }

        private MicrosoftOAuthStrategyFactoryContext createOAuthContext()
        {
            return new MicrosoftOAuthStrategyFactoryContext
            {
                HttpClient = _httpClient,
                ClientId = DefaultMicrosoftOAuthClientInfo.ClientId,
                Scopes = DefaultMicrosoftOAuthClientInfo.Scopes,
            };
        }

        private XboxAuthStrategyFactoryContext createXboxAuthContext()
        {
            return new XboxAuthStrategyFactoryContext
            {
                HttpClient = _httpClient
            };
        }
    }
}
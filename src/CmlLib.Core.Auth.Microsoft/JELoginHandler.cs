using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using XboxAuthNet.OAuth.Models;

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

        public JEAuthenticationBuilder AuthenticateInteractively()
        {
            return new JEAuthenticationBuilder()
                .WithGameAuthenticator(createGameAuthenticator())
                .WithSessionSource(createSessionSource());
        }

        public Task<XboxGameSession> AuthenticateInteractively(IXboxAuthStrategy xboxAuthStrategy) =>
            AuthenticateInteractively().WithXboxAuth(xboxAuthStrategy).ExecuteAsync();

        public JEAuthenticationBuilder AuthenticateSilently()
        {
            return new JEAuthenticationBuilder()
                .WithGameAuthenticator(createSilentGameAuthenticator())
                .WithSessionSource(createSessionSource());
        }

        public Task<XboxGameSession> AuthenticateSilently(IXboxAuthStrategy xboxAuthStrategy) =>
            AuthenticateSilently().WithXboxAuth(xboxAuthStrategy).ExecuteAsync();

        private IXboxGameAuthenticator createGameAuthenticator()
        {
            return new DummyGameAuthenticator();
        }

        private IXboxGameAuthenticator createSilentGameAuthenticator()
        {
            return new SilentXboxGameAuthenticator(createGameAuthenticator());
        }

        private ISessionSource<XboxGameSession> createSessionSource()
        {
            return new SessionFromStorage<XboxGameSession>("G", SessionStorage);
        }

        public MicrosoftOAuthStrategyBuilder CreateMicrosoftOAuthBuilder()
        {
            return new MicrosoftOAuthStrategyBuilder(DefaultMicrosoftOAuthClientInfo, _httpClient)
                .WithCaching(true)
                .WithSessionSource(new SessionFromStorage<MicrosoftOAuthResponse>("O", SessionStorage));
        }

        public XboxAuthStrategyBuilder CreateXboxAuthBuilder(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            return new XboxAuthStrategyBuilder(_httpClient, oAuthStrategy)
                .WithCaching(true)
                .WithSessionSource(new SessionFromStorage<XboxAuthTokens>("X", SessionStorage));
        }
    }
}
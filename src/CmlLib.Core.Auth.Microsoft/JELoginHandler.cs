using System.Net.Http;
using System.Threading.Tasks;
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

        public JEAuthBuilder AuthenticateInteractively()
        {
            return new JEAuthBuilder(_httpClient, DefaultMicrosoftOAuthClientInfo)
                .WithSessionStorage(SessionStorage)
                .WithGameAuthenticator(createGameAuthenticator())
                .WithInteractiveMicrosoftOAuth()
                .WithBasicXboxAuth();
        }

        public JEAuthBuilder AuthenticateSilently()
        {
            return new JEAuthBuilder(_httpClient, DefaultMicrosoftOAuthClientInfo)
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
    }
}
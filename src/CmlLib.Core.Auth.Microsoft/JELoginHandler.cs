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
            ClientId = "test_jeloginhandler_client_id",
            Scopes = "test_jeloginhandler_scopes"
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
            return createAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage)
                .MicrosoftOAuth.WithCaching(true)
                .MicrosoftOAuth.UseInteractiveStrategy()
                .XboxAuth.WithCaching(true)
                .XboxAuth.UseBasicStrategy()
                .UseInteractiveStrategy();
        }

        public JEAuthenticationBuilder AuthenticateSilently()
        {
            return createAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage)
                .MicrosoftOAuth.WithCaching(true)
                .MicrosoftOAuth.UseSilentStrategy()
                .XboxAuth.WithCaching(true)
                .XboxAuth.UseBasicStrategy()
                .UseSilentStrategy();
        }

        private JEAuthenticationBuilder createAuthenticationBuilder()
        {
            return new JEAuthenticationBuilder(JELoginHandler.DefaultMicrosoftOAuthClientInfo);
        }
    }
}
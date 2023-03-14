using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Builders;

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

        public async Task<MSession> Authenticate()
        {
            JESession session;
            try
            {
                session = await AuthenticateSilently().ExecuteAsync();
            }
            catch (SessionExpiredException)
            {
                session = await AuthenticateInteractively().ExecuteAsync();
            }

            return session.ToLauncherSession();
        }

        public JEInteractiveAuthenticationBuilder AuthenticateInteractively()
        {
            return new JEInteractiveAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage);
        }

        public JESilentAuthenticationBuilder AuthenticateSilently()
        {
            return new JESilentAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage);
        }
    }
}
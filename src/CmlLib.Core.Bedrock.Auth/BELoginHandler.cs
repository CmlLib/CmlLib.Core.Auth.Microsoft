using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BELoginHandler
    {
        public static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new MicrosoftOAuthClientInfo
        {
            
        };

        private readonly HttpClient _httpClient;
        public ISessionStorage SessionStorage { get; set; }

        public BELoginHandler(
            HttpClient httpClient, 
            ISessionStorage sessionStorage) =>
        (_httpClient, SessionStorage) = (httpClient, sessionStorage);

        public async Task<BESession> Authenticate()
        {
            BESession session;
            try
            {
                session = await AuthenticateSilently()
                    .ExecuteAsync();
            }
            catch (SessionExpiredException)
            {
                session = await AuthenticateInteractively()
                    .ExecuteAsync();
            }
            return session;
        }

        public BEInteractiveAuthenticationBuilder AuthenticateInteractively()
        {
            return new BEInteractiveAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage);
        }

        public BESilentAuthenticationBuilder AuthenticateSilently()
        {
            return new BESilentAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage);
        }
    }
}
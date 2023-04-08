using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JELoginHandler
    {
        public static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new MicrosoftOAuthClientInfo
        {
            ClientId = XboxGameTitles.MinecraftJava,
            Scopes = XboxAuthConstants.XboxScope
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

        public Task Signout()
        {
            return CreateSignout()
                .ExecuteAsync();
        }

        public JESignoutBuilder CreateSignout()
        {
            return new JESignoutBuilder()
                .WithHttpClient(_httpClient)
                .WithSessionStorage(SessionStorage);
        }
    }
}
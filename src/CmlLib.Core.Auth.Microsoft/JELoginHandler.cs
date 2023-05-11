using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JELoginHandler : XboxGameLoginHandler
    {
        public static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new MicrosoftOAuthClientInfo
        {
            ClientId = XboxGameTitles.MinecraftJava,
            Scopes = XboxAuthConstants.XboxScope
        };

        public JELoginHandler(
            HttpClient httpClient, 
            IXboxGameAccountManager accountManager) :
            base(httpClient, accountManager)
        {

        }

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
                .WithHttpClient(HttpClient)
                .WithAccountManager(AccountManager);
        }

        public JESilentAuthenticationBuilder AuthenticateSilently()
        {
            return new JESilentAuthenticationBuilder()
                .WithHttpClient(HttpClient)
                .WithAccountManager(AccountManager);
        }

        public Task Signout()
        {
            return CreateSignout()
                .ExecuteAsync();
        }

        public JESignoutBuilder CreateSignout()
        {
            return new JESignoutBuilder()
                .WithHttpClient(HttpClient)
                .WithAccountManager(AccountManager);
        }
    }
}
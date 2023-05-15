using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;
using CmlLib.Core.Auth.Microsoft.Sessions;

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

        public XboxGameAuthenticationBuilder<JESession> AuthenticateInteractively()
        {
            return new XboxGameAuthenticationBuilder<JESession>()
                .WithJEAuthenticator(_ => {})
                .WithInteractiveMicrosoftOAuth()
                .WithNewAccount(AccountManager)
                .WithHttpClient(HttpClient);
        }

        public XboxGameAuthenticationBuilder<JESession> AuthenticateSilently()
        {
            return new XboxGameAuthenticationBuilder<JESession>()
                .WithJEAuthenticator(builder => builder.WithSilentAuthenticator())
                .WithSilentMicrosoftOAuth()
                .WithDefaultAccount(AccountManager)
                .WithHttpClient(HttpClient);
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
                .WithAccount(AccountManager.GetDefaultAccount())
                .AddSavingAccountManager(AccountManager);
        }
    }
}
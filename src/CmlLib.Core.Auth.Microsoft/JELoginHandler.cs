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

        public Task<MSession> Authenticate()
        {
            var account = AccountManager.GetDefaultAccount();
            return Authenticate(account);
        }

        public async Task<MSession> Authenticate(IXboxGameAccount account)
        {
            JESession session;
            try
            {
                session = await AuthenticateSilently()
                    .WithAccount(account)
                    .ExecuteAsync();
            }
            catch (SessionExpiredException)
            {
                session = await AuthenticateInteractively()
                    .WithAccount(account)
                    .ExecuteAsync();
            }

            return session.ToLauncherSession();
        }

        public XboxGameAuthenticationBuilder<JESession> AuthenticateInteractively()
        {
            return new XboxGameAuthenticationBuilder<JESession>()
                .WithJEAuthenticator(_ => {})
                .WithInteractiveMicrosoftOAuth()
                .WithAccountManager(AccountManager)
                .WithNewAccount(AccountManager)
                .WithHttpClient(HttpClient);
        }

        public XboxGameAuthenticationBuilder<JESession> AuthenticateSilently()
        {
            return new XboxGameAuthenticationBuilder<JESession>()
                .WithJEAuthenticator(builder => builder.WithSilentAuthenticator())
                .WithSilentMicrosoftOAuth()
                .WithAccountManager(AccountManager)
                .WithDefaultAccount(AccountManager)
                .WithHttpClient(HttpClient);
        }

        public Task Signout() =>
            CreateSignout().ExecuteAsync();

        public Task Signout(IXboxGameAccount account) =>
            CreateSignout(account).ExecuteAsync();

        public JESignoutBuilder CreateSignout()
        {
            var account = AccountManager.GetDefaultAccount();
            return CreateSignout(account);
        }

        public JESignoutBuilder CreateSignout(IXboxGameAccount account)
        {
            return new JESignoutBuilder()
                .WithHttpClient(HttpClient)
                .WithAccount(account)
                .AddSavingAccountManager(AccountManager);
        }
    }
}
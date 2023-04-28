using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.SessionStorages;
using System.Linq;

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
        private readonly XboxGameAccountManager<JEGameAccount> _accountManager;

        public JELoginHandler(
            HttpClient httpClient, 
            XboxGameAccountManager<JEGameAccount> accountManager) =>
            (_httpClient, _accountManager) = (httpClient, accountManager);

        public XboxGameAccountCollection<JEGameAccount> GetAccounts()
        {
            if (!_accountManager.Accounts.Any())
                _accountManager.Load();
            return _accountManager.Accounts;
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
                .WithHttpClient(_httpClient)
                .WithAccountManager(_accountManager);
        }

        public JESilentAuthenticationBuilder AuthenticateSilently()
        {
            return new JESilentAuthenticationBuilder()
                .WithHttpClient(_httpClient)
                .WithAccountManager(_accountManager);
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
                .WithJEAccountManager(_accountManager);
        }
    }
}
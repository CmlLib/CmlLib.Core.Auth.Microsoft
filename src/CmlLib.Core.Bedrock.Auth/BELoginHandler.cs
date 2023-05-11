using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BELoginHandler : XboxGameLoginHandler
    {
        public static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new MicrosoftOAuthClientInfo
        {
            
        };

        public BELoginHandler(
            HttpClient httpClient, 
            IXboxGameAccountManager accountManager) : 
            base(httpClient, accountManager)
        {

        }

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
                .WithHttpClient(HttpClient)
                .WithAccountManager(AccountManager);
        }

        public BESilentAuthenticationBuilder AuthenticateSilently()
        {
            return new BESilentAuthenticationBuilder()
                .WithHttpClient(HttpClient)
                .WithAccountManager(AccountManager);
        }
    }
}
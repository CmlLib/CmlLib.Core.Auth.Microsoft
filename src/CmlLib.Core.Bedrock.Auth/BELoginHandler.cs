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

        public XboxGameAuthenticationBuilder<BESession> AuthenticateInteractively()
        {
            return new XboxGameAuthenticationBuilder<BESession>()
                .WithInteractiveMicrosoftOAuth()
                .WithHttpClient(HttpClient)
                .WithNewAccount(AccountManager);
        }

        public XboxGameAuthenticationBuilder<BESession> AuthenticateSilently()
        {
            return new XboxGameAuthenticationBuilder<BESession>()
                .WithSilentMicrosoftOAuth()
                .WithHttpClient(HttpClient)
                .WithDefaultAccount(AccountManager);
        }
    }
}
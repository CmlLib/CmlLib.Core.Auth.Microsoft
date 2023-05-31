using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;
using CmlLib.Core.Auth.Microsoft.Sessions;
using CmlLib.Core.Auth.Microsoft.Authenticators;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JELoginHandler : XboxGameLoginHandler
    {
        public readonly static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new()
        {
            ClientId = XboxGameTitles.MinecraftJava,
            Scopes = XboxAuthConstants.XboxScope
        };

        public readonly static string RelyingParty = "rp://api.minecraftservices.com/";

        public JELoginHandler(
            HttpClient httpClient, 
            IXboxGameAccountManager accountManager) :
            base(httpClient, accountManager)
        {

        }

        public Task<MSession> Authenticate(CancellationToken cancellationToken = default)
        {
            var account = AccountManager.GetDefaultAccount();
            return Authenticate(account);
        }

        public async Task<MSession> Authenticate(
            IXboxGameAccount account,
            CancellationToken cancellationToken = default)
        {
            JEGameAccount result;
            try
            {
                result = await AuthenticateSilently(account, cancellationToken);
            }
            catch
            {
                result = await AuthenticateInteractively(account, cancellationToken);
            }
            return result.ToLauncherSession();
        }

        public async Task<JEGameAccount> AuthenticateInteractively(
            IXboxGameAccount account,
            CancellationToken cancellationToken = default)
        {
            var authenticator = CreateAuthenticator(account, cancellationToken);
            authenticator.AddForceMicrosoftOAuthForJE(oauth => oauth.Interactive());
            authenticator.AddForceXboxAuth(xbox => xbox.Basic());
            authenticator.AddForceJEAuthenticator();
            
            var session = await authenticator.ExecuteAsync();
            return JEGameAccount.FromSessionStorage(session);
        }

        public async Task<JEGameAccount> AuthenticateSilently(
            IXboxGameAccount account,
            CancellationToken cancellationToken = default)
        {
            var authenticator = CreateAuthenticator(account, cancellationToken);
            authenticator.AddMicrosoftOAuthForJE(oauth => oauth.Silent());
            authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
            authenticator.AddJEAuthenticator();

            var session = await authenticator.ExecuteAsync();
            return JEGameAccount.FromSessionStorage(session);
        }
        
        public async Task Signout(
            IXboxGameAccount account, 
            CancellationToken cancellationToken = default)
        {
            var authenticator = CreateAuthenticator(account, cancellationToken);
            authenticator.AddMicrosoftOAuthSignout(DefaultMicrosoftOAuthClientInfo);
            authenticator.AddSessionCleaner(XboxSessionSource.Default);
            authenticator.AddSessionCleaner(JETokenSource.Default);
            authenticator.AddSessionCleaner(JEProfileSource.Default);
            await authenticator.ExecuteAsync();
        }

        public CompositeAuthenticator CreateAuthenticator(
            IXboxGameAccount account,
            CancellationToken cancellationToken)
        {
            var authenticator = new CompositeAuthenticator();
            authenticator.Context = createContext(account, cancellationToken);
            authenticator.AddPostAuthenticator(LastAccessLogger.Default);
            authenticator.AddPostAuthenticator(new AccountSaver(AccountManager));
            return authenticator;
        }

        private AuthenticateContext createContext(
            IXboxGameAccount account, 
            CancellationToken cancellationToken)
        {
            return new AuthenticateContext(
                account.SessionStorage, 
                HttpClient, 
                cancellationToken);
        }
    }
}
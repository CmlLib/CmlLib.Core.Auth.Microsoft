using Microsoft.Identity.Client;
using System.Linq;
using System.Threading.Tasks;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal.OAuth
{
    public class MsalSilentStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IPublicClientApplication _app;
        public string[] Scopes { get; set; } = MsalClientHelper.XboxScopes;
        public string? LoginHint { get; set; }

        public MsalSilentStrategy(IPublicClientApplication app) : this(app, null)
        {
            
        }

        public MsalSilentStrategy(
            IPublicClientApplication app, 
            string? loginHint)
        {
            this._app = app;
            this.LoginHint = loginHint;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            AuthenticationResult result;
            if (string.IsNullOrEmpty(LoginHint))
            {
                var accounts = await _app.GetAccountsAsync();
                result = await _app.AcquireTokenSilent(Scopes, accounts.FirstOrDefault()).ExecuteAsync();
            }
            else
                result = await _app.AcquireTokenSilent(Scopes, LoginHint).ExecuteAsync();

            return MsalClientHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}

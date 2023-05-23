using Microsoft.Identity.Client;
using System.Threading.Tasks;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal.OAuth
{
    public class MsalInteractiveStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IPublicClientApplication _app;
        public string[] Scopes { get; set; } = MsalClientHelper.XboxScopes;

        public bool UseDefaultWebViewOption { get; set; } = true;
        public bool UseEmbeddedWebView { get; set; } = true;

        public MsalInteractiveStrategy(IPublicClientApplication app)
        {
            this._app = app;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var builder = _app.AcquireTokenInteractive(Scopes);
            if (!UseDefaultWebViewOption)
                builder.WithUseEmbeddedWebView(UseEmbeddedWebView);

            var result = await builder.ExecuteAsync();
            return MsalClientHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}

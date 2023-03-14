using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalInteractiveStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IPublicClientApplication _app;
        private readonly string[] scopes;

        public bool UseDefaultWebViewOption { get; set; } = true;
        public bool UseEmbeddedWebView { get; set; } = true;

        public MsalInteractiveStrategy(IPublicClientApplication app)
        {
            this._app = app;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var builder = _app.AcquireTokenInteractive(scopes);
            if (!UseDefaultWebViewOption)
                builder.WithUseEmbeddedWebView(UseEmbeddedWebView);

            var result = await builder.ExecuteAsync();
            return MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}

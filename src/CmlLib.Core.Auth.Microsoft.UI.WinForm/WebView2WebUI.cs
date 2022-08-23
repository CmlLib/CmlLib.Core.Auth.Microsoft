using CmlLib.Core.Auth.Microsoft.OAuth;
using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.UI.WinForm
{
    public class WebView2WebUI : IWebUI
    {
        private readonly object? _ownerWindow;

        public WebView2WebUI()
        {

        }

        public WebView2WebUI(object ownerWindow)
        {
            this._ownerWindow = ownerWindow;
        }

        public Task<MicrosoftOAuthCode> GetAuthCode(IWebUILoginHandler loginHandler, CancellationToken cancellationToken)
        {
            var form = new WinFormsPanelWithWebView2(_ownerWindow);
            var result = form.DisplayDialogAndInterceptUri(loginHandler, cancellationToken);
            return Task.FromResult(result);
        }

        public Task ShowUri(Uri uri, CancellationToken cancellationToken)
        {
            var form = new WinFormsPanelWithWebView2(_ownerWindow);
            form.DisplayDialogAndNavigateUri(uri, cancellationToken);
            return Task.CompletedTask;
        }
    }
}

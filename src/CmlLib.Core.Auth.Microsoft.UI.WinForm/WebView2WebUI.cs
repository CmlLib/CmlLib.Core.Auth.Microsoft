using CmlLib.Core.Auth.Microsoft.OAuth;
using Microsoft.Web.WebView2.Core;
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

        public static bool IsWebView2Available()
        {
            try
            {
                string wv2Version = CoreWebView2Environment.GetAvailableBrowserVersionString();
                return !string.IsNullOrEmpty(wv2Version);
            }
            catch (WebView2RuntimeNotFoundException)
            {
                return false;
            }
            catch (Exception ex) when (ex is BadImageFormatException || ex is DllNotFoundException)
            {
                return false;
                //throw new MsalClientException(MsalError.WebView2LoaderNotFound, MsalErrorMessage.WebView2LoaderNotFound, ex);
            }
        }
    }
}

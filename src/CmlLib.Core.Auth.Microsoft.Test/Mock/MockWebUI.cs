using CmlLib.Core.Auth.Microsoft.OAuth;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockWebUI : IWebUI
    {
        public MockWebUI(Uri redirectedUri)
        {
            this.RedirectedUri = redirectedUri;
        }

        public Uri RedirectedUri { get; private set; }
        public string? OAuthUrl { get; private set; }

        public Task<MicrosoftOAuthCode> GetAuthCode(IWebUILoginHandler loginHandler)
        {
            OAuthUrl = loginHandler.CreateOAuthUrl();
            var result = loginHandler.CheckOAuthCodeResult(RedirectedUri);

            if (result.IsSuccess && result.OAuthCode != null)
                return Task.FromResult(result.OAuthCode);
            else
                throw new LoginCancelledException("MockWebUI");
        }

        public Task ShowUri(Uri uri)
        {
            return Task.CompletedTask;
        }
    }
}

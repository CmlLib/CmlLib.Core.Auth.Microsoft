using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.SignoutStrategy
{
    public class MicrosoftOAuthBrowserSignoutStrategy : ISignoutStrategy
    {
        private readonly MicrosoftOAuthCodeFlow _codeFlow;

        public MicrosoftOAuthBrowserSignoutStrategy(MicrosoftOAuthCodeFlow codeFlow) =>
            _codeFlow = codeFlow;

        public async ValueTask Signout()
        {
            await _codeFlow.Signout();
        }
    }
}
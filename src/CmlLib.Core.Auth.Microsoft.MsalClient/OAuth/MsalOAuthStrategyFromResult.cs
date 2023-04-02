using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.MsalClient.OAuth
{
    public class MsalOAuthStrategyFromResult : IMicrosoftOAuthStrategy
    {
        private readonly AuthenticationResult _result;

        public MsalOAuthStrategyFromResult(AuthenticationResult result) => _result = result;

        public Task<MicrosoftOAuthResponse> Authenticate()
        {
            var response = MsalClientHelper.ToMicrosoftOAuthResponse(_result);
            return Task.FromResult(response);
        }
    }
}

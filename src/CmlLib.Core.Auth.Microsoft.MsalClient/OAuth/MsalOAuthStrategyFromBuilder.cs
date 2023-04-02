using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.MsalClient.OAuth
{
    public class MsalOAuthStrategyFromBuilder<T> : IMicrosoftOAuthStrategy where T : AbstractAcquireTokenParameterBuilder<T>
    {
        private readonly AbstractAcquireTokenParameterBuilder<T> _builder;

        public MsalOAuthStrategyFromBuilder(AbstractAcquireTokenParameterBuilder<T> builder) => _builder = builder;

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _builder.ExecuteAsync();
            var response = MsalClientHelper.ToMicrosoftOAuthResponse(result);
            return response;
        }
    }
}

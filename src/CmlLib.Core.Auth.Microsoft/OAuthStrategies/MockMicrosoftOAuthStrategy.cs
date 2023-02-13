using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class MockMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly MicrosoftOAuthResponse _response;

        public MockMicrosoftOAuthStrategy(MicrosoftOAuthResponse response)
        {
            this._response = response;
        }

        public Task<MicrosoftOAuthResponse> Authenticate()
        {
            return Task.FromResult(_response);
        }

        public Task<MicrosoftOAuthResponse> Authenticate(MicrosoftOAuthResponse cachedResponse)
        {
            return Task.FromResult(_response);
        }
    }
}
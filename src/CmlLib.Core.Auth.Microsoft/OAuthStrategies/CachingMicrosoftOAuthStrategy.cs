using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class CachingMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly ISessionSource<MicrosoftOAuthResponse> _oauthTokenSource;
        private readonly IMicrosoftOAuthStrategy _innerStrategy;

        public CachingMicrosoftOAuthStrategy(
            IMicrosoftOAuthStrategy strategy, 
            ISessionSource<MicrosoftOAuthResponse> oauthTokenSource) =>
            (_oauthTokenSource, _innerStrategy) = (oauthTokenSource, strategy);

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _innerStrategy.Authenticate();
            await _oauthTokenSource.SetAsync(result);
            return result;
        }
    }
}
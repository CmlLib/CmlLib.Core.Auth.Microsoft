using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuthStrategies
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
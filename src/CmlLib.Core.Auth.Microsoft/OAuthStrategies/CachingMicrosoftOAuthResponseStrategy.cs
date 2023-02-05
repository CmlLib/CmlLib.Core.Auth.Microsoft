using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Cache;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class CachingMicrosoftOAuthResponseStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IMicrosoftOAuthStrategy _strategy;
        private readonly ICacheStorage<MicrosoftOAuthResponse> _cacheStorage;

        public CachingMicrosoftOAuthResponseStrategy(
            IMicrosoftOAuthStrategy strategy,
            ICacheStorage<MicrosoftOAuthResponse> cacheStorage)
        {
            this._strategy = strategy;
            this._cacheStorage = cacheStorage;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _strategy.Authenticate();
            _cacheStorage.Set(result);
            return result;
        }
    }
}
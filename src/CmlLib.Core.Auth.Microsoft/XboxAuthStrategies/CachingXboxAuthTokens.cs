using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class CachingXboxAuthStrategy : IXboxAuthStrategy
    {
        private readonly IXboxAuthStrategy _innerStrategy;
        private readonly ISessionSource<XboxAuthTokens> _sessionSource;

        public CachingXboxAuthStrategy(
            IXboxAuthStrategy innerStrategy,
            ISessionSource<XboxAuthTokens> sessionSource) =>
            (_innerStrategy, _sessionSource) = (innerStrategy, sessionSource);

        public async Task<XboxAuthTokens> Authenticate()
        {
            var result = await _innerStrategy.Authenticate();
            await _sessionSource.SetAsync(result);
            return result;
        }
    }
}
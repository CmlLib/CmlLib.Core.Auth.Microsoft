using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.XboxAuthStrategies
{
    public class CachingXboxAuthStrategy : IXboxAuthStrategy
    {
        private readonly IXboxAuthStrategy _innerStrategy;
        private readonly ISessionSource<XboxAuthTokens> _sessionSource;

        public CachingXboxAuthStrategy(
            IXboxAuthStrategy innerStrategy,
            ISessionSource<XboxAuthTokens> sessionSource) =>
            (_innerStrategy, _sessionSource) = (innerStrategy, sessionSource);

        public async Task<XboxAuthTokens> Authenticate(string relyingParty)
        {
            var result = await _innerStrategy.Authenticate(relyingParty);
            _sessionSource.Set(result);
            return result;
        }
    }
}
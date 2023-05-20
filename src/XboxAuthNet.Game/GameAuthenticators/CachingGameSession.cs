using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.GameAuthenticators
{
    public class CachingGameSession<T> : IXboxGameAuthenticator<T>
        where T : ISession
    {
        private readonly IXboxGameAuthenticator<T> _inner;
        private readonly ISessionSource<T> _sessionSource;

        public CachingGameSession(IXboxGameAuthenticator<T> inner, ISessionSource<T> sessionSource) =>
        (_inner, _sessionSource) = (inner, sessionSource);

        public async Task<T> Authenticate(IXboxAuthStrategy strategy)
        {
            var result = await _inner.Authenticate(strategy);
            _sessionSource.Set((T?)result);
            return result;
        }
    }
}
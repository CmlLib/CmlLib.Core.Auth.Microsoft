using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.XboxGame
{
    public class CachingGameSession<T> : IXboxGameAuthenticator
    {
        private readonly IXboxGameAuthenticator _inner;
        private readonly ISessionSource<T> _sessionSource;

        public CachingGameSession(IXboxGameAuthenticator inner, ISessionSource<T> sessionSource) =>
        (_inner, _sessionSource) = (inner, sessionSource);

        public async Task<ISession> Authenticate(IXboxAuthStrategy strategy)
        {
            var result = await _inner.Authenticate(strategy);
            _sessionSource.Set((T?)result);
            return result;
        }
    }
}
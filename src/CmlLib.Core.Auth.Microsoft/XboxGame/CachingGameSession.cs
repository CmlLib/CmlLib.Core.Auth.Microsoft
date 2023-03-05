using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class CachingGameSession<T> : IXboxGameAuthenticator<T> where T : ISession
    {
        private readonly IXboxGameAuthenticator<T> _inner;
        private readonly ISessionSource<T> _sessionSource;

        public CachingGameSession(IXboxGameAuthenticator<T> inner, ISessionSource<T> sessionSource) =>
        (_inner, _sessionSource) = (inner, sessionSource);

        public async Task<T> Authenticate(IXboxAuthStrategy strategy)
        {
            var result = await _inner.Authenticate(strategy);
            await _sessionSource.SetAsync(result);
            return result;
        }
    }
}
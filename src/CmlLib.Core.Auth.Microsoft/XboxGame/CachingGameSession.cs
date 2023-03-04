using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class CachingGameSession : IXboxGameAuthenticator
    {
        private readonly IXboxGameAuthenticator _inner;
        private readonly ISessionSource<XboxGameSession> _sessionSource;

        public CachingGameSession(IXboxGameAuthenticator inner, ISessionSource<XboxGameSession> sessionSource) =>
        (_inner, _sessionSource) = (inner, sessionSource);

        public async Task<XboxGameSession> Authenticate(IXboxAuthStrategy strategy)
        {
            var cached = await _sessionSource.GetAsync();
            if (cached != null && cached.Validate())
                return cached;
            return await _inner.Authenticate(strategy);
        }
    }
}
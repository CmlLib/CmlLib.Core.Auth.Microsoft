using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class CachingXboxGameSession : IXboxGameAuthenticator
    {
        private readonly ISessionSource<XboxGameSession> _sessionSource;
        private readonly IXboxGameAuthenticator _innerAuthenticator;

        public CachingXboxGameSession(
            ISessionSource<XboxGameSession> sessionSource,
            IXboxGameAuthenticator authenticator) =>
            (_sessionSource, _innerAuthenticator) = (sessionSource, authenticator);

        public async Task<XboxGameSession> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var result = await _innerAuthenticator.Authenticate(xboxAuthStrategy);
            await _sessionSource.SetAsync(result);
            return result;
        }
    }
}
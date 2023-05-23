using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.GameAuthenticators
{
    public class SilentXboxGameAuthenticator<T> : IXboxGameAuthenticator<T> 
        where T : ISession
    {
        private readonly IXboxGameAuthenticator<T> _innerAuthenticator;
        private readonly ISessionSource<T> _sessionSource;

        public SilentXboxGameAuthenticator(
            IXboxGameAuthenticator<T> authenticator,
            ISessionSource<T> sessionSource) =>
            (_innerAuthenticator, _sessionSource) = (authenticator, sessionSource);

        public async Task<T> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var storedSession = _sessionSource.Get();
            if (storedSession != null && storedSession.Validate())
                return storedSession;
            else
                return await _innerAuthenticator.Authenticate(xboxAuthStrategy);
        }
    }
}
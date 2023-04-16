using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.XboxGame
{
    public class SilentXboxGameAuthenticator<T> : IXboxGameAuthenticator where T : ISession
    {
        private readonly IXboxGameAuthenticator _innerAuthenticator;
        private readonly ISessionSource<T> _sessionSource;

        public SilentXboxGameAuthenticator(
            IXboxGameAuthenticator authenticator,
            ISessionSource<T> sessionSource) =>
            (_innerAuthenticator, _sessionSource) = (authenticator, sessionSource);

        public async Task<ISession> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var storedSession = _sessionSource.Get();
            if (storedSession != null && storedSession.Validate())
                return storedSession;
            else
                return await _innerAuthenticator.Authenticate(xboxAuthStrategy);
        }
    }
}
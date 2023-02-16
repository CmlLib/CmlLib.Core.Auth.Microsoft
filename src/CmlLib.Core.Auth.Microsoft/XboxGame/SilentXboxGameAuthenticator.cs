using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class SilentXboxGameAuthenticator : IXboxGameAuthenticator
    {
        private readonly ISessionSource<XboxGameSession> _sessionSource;
        private readonly IXboxGameAuthenticator _innerAuthenticator;

        public SilentXboxGameAuthenticator(
            ISessionSource<XboxGameSession> sessionSource,
            IXboxGameAuthenticator authenticator) =>
            (_sessionSource, _innerAuthenticator) = (sessionSource, authenticator);

        public async Task<XboxGameSession> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var storedSession = await _sessionSource.GetAsync();
            if (storedSession != null && storedSession.Validate())
                return storedSession;
            else
                return await _innerAuthenticator.Authenticate(xboxAuthStrategy);
        }
    }
}
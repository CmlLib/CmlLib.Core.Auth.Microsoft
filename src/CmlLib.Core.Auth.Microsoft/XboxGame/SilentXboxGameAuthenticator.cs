using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class SilentXboxGameAuthenticator : IXboxGameAuthenticator
    {
        private readonly IXboxGameAuthenticator _innerAuthenticator;

        public SilentXboxGameAuthenticator(
            IXboxGameAuthenticator authenticator) =>
            _innerAuthenticator = authenticator;

        public async Task<XboxGameSession> Authenticate(IXboxAuthStrategy xboxAuthStrategy, ISessionSource<XboxGameSession> sessionSource)
        {
            var storedSession = await sessionSource.GetAsync();
            if (storedSession != null && storedSession.Validate())
                return storedSession;
            else
                return await _innerAuthenticator.Authenticate(xboxAuthStrategy, sessionSource);
        }
    }
}
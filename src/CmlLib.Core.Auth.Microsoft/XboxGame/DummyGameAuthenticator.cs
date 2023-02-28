using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class DummyGameAuthenticator : IXboxGameAuthenticator
    {
        public async Task<XboxGameSession> Authenticate(IXboxAuthStrategy xboxAuthStrategy, ISessionSource<XboxGameSession> sessionSource)
        {
            var xboxTokens = await xboxAuthStrategy.Authenticate("relyingParty");
            var dummySession = new XboxGameSession();
            await sessionSource.SetAsync(dummySession);
            return dummySession;
        }
    }
}
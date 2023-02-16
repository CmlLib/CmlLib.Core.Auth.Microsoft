using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class DummyGameAuthenticator : IXboxGameAuthenticator
    {
        public async Task<XboxGameSession> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var xboxTokens = await xboxAuthStrategy.Authenticate();
            var dummySession = new XboxGameSession();
            return dummySession;
        }
    }
}
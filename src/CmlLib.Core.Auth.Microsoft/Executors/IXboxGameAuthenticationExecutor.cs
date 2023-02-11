using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Executors
{
    public interface IXboxGameAuthenticationExecutor
    {
        Task<XboxGameSession> Authenticate(
            IMicrosoftOAuthStrategy oAuthStrategy,
            IXboxAuthStrategy xboxAuthStrategy,
            IXboxGameAuthenticator xboxGameAuthenticator,
            ISessionStorage sessionStorage);
    }
}
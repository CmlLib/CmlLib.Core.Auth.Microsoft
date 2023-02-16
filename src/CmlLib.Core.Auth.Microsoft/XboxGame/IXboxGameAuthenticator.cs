using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public interface IXboxGameAuthenticator
    {
        Task<XboxGameSession> Authenticate(IXboxAuthStrategy xboxAuthStrategy);
    }
}
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public interface IXboxGameAuthenticator<T>
    {
        Task<T> Authenticate(IXboxAuthStrategy xboxAuthStrategy);
    }
}
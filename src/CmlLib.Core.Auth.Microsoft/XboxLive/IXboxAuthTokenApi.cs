using System.Threading.Tasks;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public interface IXboxAuthTokenApi
    {
        Task<XboxAuthResponse?> GetToken(XboxAuthResponse? previousToken);
    }
}

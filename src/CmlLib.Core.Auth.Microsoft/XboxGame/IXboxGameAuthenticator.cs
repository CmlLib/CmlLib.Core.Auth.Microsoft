using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public interface IXboxGameAuthenticator
    {
        Task<XboxGameSession> Authenticate(XboxAuthTokens tokens);
    }
}
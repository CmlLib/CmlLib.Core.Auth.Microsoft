using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public interface IXboxAuthStrategy
    {
        Task<XboxAuthTokens> Authenticate(MicrosoftOAuthResponse oAuthResponse);
    }
}
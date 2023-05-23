using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuthStrategies
{
    public interface IMicrosoftOAuthStrategy
    {
        Task<MicrosoftOAuthResponse> Authenticate();
    }
}
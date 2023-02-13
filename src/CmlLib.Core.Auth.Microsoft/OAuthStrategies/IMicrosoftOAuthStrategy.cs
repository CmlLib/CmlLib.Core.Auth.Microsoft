using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public interface IMicrosoftOAuthStrategy
    {
        Task<MicrosoftOAuthResponse> Authenticate();
        Task<MicrosoftOAuthResponse> Authenticate(MicrosoftOAuthResponse cachedResponse);
    }
}
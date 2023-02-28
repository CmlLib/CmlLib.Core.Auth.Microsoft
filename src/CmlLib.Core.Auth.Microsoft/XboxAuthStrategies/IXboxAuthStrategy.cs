using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public interface IXboxAuthStrategy
    {
        Task<XboxAuthTokens> Authenticate(string relyingParty);
    }
}
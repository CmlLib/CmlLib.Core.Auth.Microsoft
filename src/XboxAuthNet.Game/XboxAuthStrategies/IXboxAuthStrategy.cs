using System.Threading.Tasks;

namespace XboxAuthNet.Game.XboxAuthStrategies
{
    public interface IXboxAuthStrategy
    {
        Task<XboxAuthTokens> Authenticate(string relyingParty);
    }
}
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public interface IXboxLiveApi
    {
        Task<XboxAuthTokens> GetTokens(string token, XboxAuthTokens? previousTokens, string xstsRelyingParty);
    }
}

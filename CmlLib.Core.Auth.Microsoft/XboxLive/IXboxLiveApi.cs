using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public interface IXboxLiveApi
    {
        string CreateOAuthUrl();
        bool CheckOAuthLoginSuccess(string url);
        Task<MicrosoftOAuthResponse> GetTokens();
        Task<MicrosoftOAuthResponse> RefreshTokens(string token);
        Task<XboxAuthResponse> GetXSTS(string token, string? deviceToken, string? titleToken, string? xstsRelyingParty);
    }
}

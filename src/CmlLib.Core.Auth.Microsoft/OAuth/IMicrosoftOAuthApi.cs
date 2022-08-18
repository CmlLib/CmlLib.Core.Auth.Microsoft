using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public interface IMicrosoftOAuthApi
    {
        Task<MicrosoftOAuthResponse> GetOrRefreshTokens(MicrosoftOAuthResponse refreshToken);
        Task<MicrosoftOAuthResponse> RequestNewTokens();
    }
}

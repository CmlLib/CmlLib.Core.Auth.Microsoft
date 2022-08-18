using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public interface IXboxLiveApi
    {
        Task<XboxAuthResponse> GetXSTS(string token, string? deviceToken, string? titleToken, string? xstsRelyingParty);
    }
}

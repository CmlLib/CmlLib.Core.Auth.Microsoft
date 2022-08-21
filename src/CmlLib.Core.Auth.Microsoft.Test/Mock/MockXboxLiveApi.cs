using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockXboxLiveApi : IXboxLiveApi
    {
        public Task<XboxAuthResponse> GetXSTS(string token, string? deviceToken, string? titleToken, string? xstsRelyingParty)
        {
            return Task.FromResult(new XboxAuthResponse
            {
                Token = "MockXboxLiveApi_Token",
                XuiClaims = new XboxAuthXuiClaims
                {
                    UserHash = "MockXboxLiveApi_UserHash",
                    XboxUserId = "MockXboxLiveApi_XboxUserId"
                }
            });
        }
    }
}

using CmlLib.Core.Auth.Microsoft.XboxLive;
using System.Threading.Tasks;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockXboxLiveApi : IXboxLiveApi
    {
        public XboxAuthTokens ReturnObject = new XboxAuthTokens
        {
            XstsToken = new XboxAuthResponse
            {
                Token = "MockXboxLiveApi_Token",
                XuiClaims = new XboxAuthXuiClaims
                {
                    UserHash = "MockXboxLiveApi_UserHash",
                    XboxUserId = "MockXboxLiveApi_XboxUserId"
                }
            }
        };

        public Task<XboxAuthTokens> GetTokens(string token, XboxAuthTokens? previousTokens, string? xstsRelyingParty)
        {
            return Task.FromResult(ReturnObject);
        }
    }
}

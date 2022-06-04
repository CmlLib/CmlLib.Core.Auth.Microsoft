//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using XboxAuthNet.OAuth;
//using XboxAuthNet.XboxLive;
//using XboxWebApi.Authentication;
//using XboxWebApi.Authentication.Model;

//namespace CmlLib.Core.Auth.Microsoft.XboxLive
//{
//    public class OpenXboxApi : IXboxLiveApi
//    {
//        public OpenXboxApi()
//        {

//        }

//        private WindowsLiveResponse? windowsLiveResponse;

//        public bool CheckOAuthLoginSuccess(string url)
//        {
//            try
//            {
//                windowsLiveResponse = AuthenticationService.ParseWindowsLiveResponse(url);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public string CreateOAuthUrl()
//        {
//            return AuthenticationService.GetWindowsLiveAuthenticationUrl();
//        }

//        public async Task<MicrosoftOAuthResponse> GetTokens()
//        {
//            var service = new AuthenticationService(windowsLiveResponse);
//            var res = await service.AuthenticateAsync();
//            if (res)
//            {
//                return new MicrosoftOAuthResponse
//                {
//                    AccessToken = service.AccessToken.Jwt,

//                };
//            }
//            else
//                throw new MicrosoftOAuthException("");
//        }

//        public Task<XboxAuthResponse> GetXSTS(string token, string? deviceToken, string? titleToken, string? xstsRelyingParty)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<MicrosoftOAuthResponse> RefreshTokens(string token)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Requests;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class BasicXboxAuthStrategy : MicrosoftXboxAuthStrategy
    {
        public BasicXboxAuthStrategy(HttpClient httpClient, IMicrosoftOAuthStrategy oAuth)
         : base(httpClient, oAuth) 
        {

        }

        protected override async Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth, string relyingParty)
        {
            var xboxAuthClient = new XboxAuthClient(HttpClient);
            var userToken = await xboxAuthClient.RequestUserToken(oAuth.AccessToken!);
            
            if (string.IsNullOrEmpty(userToken.Token))
                throw new XboxAuthException("UserToken was empty", 0);

            var xsts = await xboxAuthClient.RequestXsts(new XboxXstsRequest
            {
                UserToken = userToken.Token,
                RelyingParty = relyingParty
            });

            var tokens = new XboxAuthTokens
            {
                UserToken = userToken,
                XstsToken = xsts
            };
            return tokens;
        }
    }
}
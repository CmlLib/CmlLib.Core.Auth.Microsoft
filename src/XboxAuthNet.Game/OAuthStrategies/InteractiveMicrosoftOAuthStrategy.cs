using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuthStrategies
{
    public class InteractiveMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly MicrosoftOAuthCodeFlow _oauthHandler;
        private readonly MicrosoftOAuthParameters _parameters;

        public InteractiveMicrosoftOAuthStrategy(
            MicrosoftOAuthCodeFlow oauthFlow, 
            MicrosoftOAuthParameters parameters) =>
            (_oauthHandler, _parameters) = (oauthFlow, parameters);

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _oauthHandler.Authenticate(_parameters);
            return result;
        }
    }
}
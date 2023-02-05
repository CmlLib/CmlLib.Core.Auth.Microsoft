using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class InteractiveMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly MicrosoftOAuthCodeFlow _oauthHandler;
        private readonly MicrosoftOAuthParameters _parameters;

        public InteractiveMicrosoftOAuthStrategy(MicrosoftOAuthCodeFlow oauthFlow, MicrosoftOAuthParameters parameters)
        {
            this._oauthHandler = oauthFlow;
            this._parameters = parameters;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _oauthHandler.Authenticate(_parameters);
            return result;
        }
    }
}
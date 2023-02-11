using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class InteractiveMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly MicrosoftOAuthCodeFlow _oauthHandler;
        private readonly MicrosoftOAuthParameters _parameters;
        private readonly ISessionSource<MicrosoftOAuthResponse> _oauthTokenSource;

        public InteractiveMicrosoftOAuthStrategy(
            MicrosoftOAuthCodeFlow oauthFlow, 
            MicrosoftOAuthParameters parameters,
            ISessionSource<MicrosoftOAuthResponse> tokenSource) =>
            (_oauthHandler, _parameters, _oauthTokenSource) = (oauthFlow, parameters, tokenSource);

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _oauthHandler.Authenticate(_parameters);
            await _oauthTokenSource.SetAsync(result);
            return result;
        }
    }
}
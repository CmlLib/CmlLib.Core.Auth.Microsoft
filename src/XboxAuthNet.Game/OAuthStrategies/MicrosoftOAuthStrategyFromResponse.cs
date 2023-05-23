using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuthStrategies
{
    public class MicrosoftOAuthStrategyFromResponse : IMicrosoftOAuthStrategy
    {
        private readonly MicrosoftOAuthResponse _response;

        public MicrosoftOAuthStrategyFromResponse(MicrosoftOAuthResponse response)
        {
            this._response = response;
        }

        public Task<MicrosoftOAuthResponse> Authenticate()
        {
            return Task.FromResult(_response);
        }
    }
}
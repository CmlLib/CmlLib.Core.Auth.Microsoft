using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.XboxGame;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BEAuthenticator : IXboxGameAuthenticator
    {
        private readonly BEAuthenticationApi _api;

        public BEAuthenticator(HttpClient httpClient)
        {
            _api = new BEAuthenticationApi(httpClient);
        }

        public async Task<ISession> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var xboxTokens = await xboxAuthStrategy.Authenticate(BEAuthenticationApi.RelyingParty);

            if (string.IsNullOrEmpty(xboxTokens?.XstsToken?.UserHash) ||
                string.IsNullOrEmpty(xboxTokens?.XstsToken?.Token))
            {
                throw new MinecraftAuthException("Cannot auth with null UserHash and null Token");
            }

            var beToken = await _api.LoginWithXbox(xboxTokens.XstsToken.UserHash, xboxTokens.XstsToken.Token);
            var beSession = new BESession
            {
                Tokens = beToken
            };
            return beSession;
        }
    }
}
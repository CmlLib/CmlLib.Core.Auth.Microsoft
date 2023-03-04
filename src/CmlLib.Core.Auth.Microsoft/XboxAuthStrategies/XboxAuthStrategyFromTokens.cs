using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class XboxAuthStrategyFromTokens : IXboxAuthStrategy
    {
        private readonly XboxAuthTokens _tokens;

        public XboxAuthStrategyFromTokens(XboxAuthTokens tokens)
        {
            this._tokens = tokens;
        }

        public Task<XboxAuthTokens> Authenticate(string relyingParty)
        {
            return Task.FromResult(_tokens);
        }
    }
}
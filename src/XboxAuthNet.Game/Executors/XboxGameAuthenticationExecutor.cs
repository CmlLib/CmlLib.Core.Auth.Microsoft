using System.Threading.Tasks;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.Game.XboxGame;

namespace XboxAuthNet.Game.Executors
{
    public class XboxGameAuthenticationExecutor : IAuthenticationExecutor
    {
        private readonly IXboxGameAuthenticator _gameAuthenticator;
        private readonly IXboxAuthStrategy _xboxAuthStrategy;

        public XboxGameAuthenticationExecutor(
            IXboxAuthStrategy xboxAuthStrategy, 
            IXboxGameAuthenticator gameAuthenticator)
        {
            this._gameAuthenticator = gameAuthenticator;
            this._xboxAuthStrategy = xboxAuthStrategy;
        }

        public async Task<ISession> ExecuteAsync()
        {
            var result = await _gameAuthenticator.Authenticate(_xboxAuthStrategy);
            return result;
        }
    }
}
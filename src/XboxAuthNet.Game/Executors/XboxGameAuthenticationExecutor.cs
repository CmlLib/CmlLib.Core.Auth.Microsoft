using System.Threading.Tasks;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.Game.GameAuthenticators;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game.Executors
{
    public class XboxGameAuthenticationExecutor<T> : IAuthenticationExecutor<T>
        where T : ISession
    {
        private readonly IXboxGameAuthenticator<T> _gameAuthenticator;
        private readonly IXboxAuthStrategy _xboxAuthStrategy;
        private readonly IXboxGameAccountManager? _accountManager;

        public XboxGameAuthenticationExecutor(
            IXboxAuthStrategy xboxAuthStrategy, 
            IXboxGameAuthenticator<T> gameAuthenticator,
            IXboxGameAccountManager? accountManager)
        {
            this._gameAuthenticator = gameAuthenticator;
            this._xboxAuthStrategy = xboxAuthStrategy;
            this._accountManager = accountManager;
        }

        public async Task<T> ExecuteAsync()
        {
            var result = await _gameAuthenticator.Authenticate(_xboxAuthStrategy);
            _accountManager?.SaveAccounts();
            return result;
        }
    }
}
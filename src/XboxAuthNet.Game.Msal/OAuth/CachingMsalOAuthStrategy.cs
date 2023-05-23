using System.Threading.Tasks;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal.OAuth
{
    public class CachingMsalOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IMicrosoftOAuthStrategy _inner;
        private readonly ISessionSource<MicrosoftOAuthResponse> _sessionSource;

        public CachingMsalOAuthStrategy(IMicrosoftOAuthStrategy inner, ISessionSource<MicrosoftOAuthResponse> sessionSource)
        {
            this._sessionSource = sessionSource;
            this._inner = inner;
        }

        // Microsoft OAuth tokens should be managed by MSAL.NET
        // SaveCache method does not cache OAuth tokens. only caching GameSession and XboxSession
        // This caching strategy only store the type of authentication (eg: MsalInteractive / MsalDeviceCode)
        // Or, this strategy may be unneccesary
        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _inner.Authenticate();
            _sessionSource.Set(null);
            return result;
        }
    }
}

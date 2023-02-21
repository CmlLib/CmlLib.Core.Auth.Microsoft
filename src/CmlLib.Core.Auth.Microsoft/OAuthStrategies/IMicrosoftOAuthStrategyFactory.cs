using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public interface IMicrosoftOAuthStrategyFactory
    {
        IMicrosoftOAuthStrategy CreateSilentStrategy(ISessionSource<MicrosoftOAuthResponse> sessionSource);

        IMicrosoftOAuthStrategy CreateInteractiveStrategy(
            MicrosoftOAuthCodeFlow codeFlow, 
            MicrosoftOAuthParameters parameters);

        IMicrosoftOAuthStrategy CreateCachingStrategy(
            IMicrosoftOAuthStrategy strategy,
            ISessionSource<MicrosoftOAuthResponse> sessionSource);
    }
}
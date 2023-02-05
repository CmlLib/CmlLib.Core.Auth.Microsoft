using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class SilentMicrosoftOAuthBuilder : AbstractMicrosoftOAuthBuilder
    {
        public SilentMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters, 
            MicrosoftOAuthClientInfo clientInfo)
             : base(parameters, clientInfo)
        {

        }

        public MicrosoftXboxAuthBuilder Silent()
        {
            var oAuthCacheStorage = new MicrosoftOAuthCacheStorageAdapter(Parameters.CacheStorage);
            var oauth = new SilentMicrosoftOAuthStrategy(OAuthClient, oAuthCacheStorage);
            return WithOAuthStrategy(oauth);
        }

        public override Task<XboxGameSession> ExecuteAsync()
        {
            return Silent().ExecuteAsync();
        }
    }
}
using System;
using System.Threading.Tasks;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.XboxGame;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BEInteractiveAuthenticationBuilder : 
        XboxGameAuthenticationBuilder<BEInteractiveAuthenticationBuilder>
    {
        public BEInteractiveAuthenticationBuilder() => 
            WithMicrosoftOAuth(builder => {});

        public BEInteractiveAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker) =>
            this.WithInteractiveMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

        public BEInteractiveAuthenticationBuilder WithAccountManager(IXboxGameAccountManager accountManager) =>
            this.WithNewAccount(accountManager);

        protected override IXboxGameAuthenticator BuildAuthenticator() => 
            new BEAuthenticator(HttpClient);

        public new async Task<BESession> ExecuteAsync()
        {
            var session = await ExecuteAsync();
            return session;
        }
    }
}
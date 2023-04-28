using System;
using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.XboxGame;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BESilentAuthenticationBuilder : XboxGameAuthenticationBuilder<BESilentAuthenticationBuilder>
    {
        public BESilentAuthenticationBuilder()
            => WithMicrosoftOAuth(builder => {});

        public BESilentAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker)
            => this.WithSilentMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

        protected override IXboxGameAuthenticator BuildAuthenticator()
            => new BEAuthenticator(HttpClient);

        public new async Task<BESession> ExecuteAsync()
        {
            var session = await ExecuteAsync();
            return session;
        }
    }
}
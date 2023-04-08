using System;
using System.Threading.Tasks;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.Executors;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BESilentAuthenticationBuilder : XboxGameAuthenticationBuilder<BESilentAuthenticationBuilder>
    {
        public BESilentAuthenticationBuilder()
        {
            WithMicrosoftOAuth(builder => {});
        }

        public BESilentAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker)
        {
            this.WithMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo, builder =>
            {
                builder.MicrosoftOAuth.UseSilentStrategy();
                builder.XboxAuth.UseBasicStrategy();
                builderInvoker.Invoke(builder);
            });
            return this;
        }

        public override IAuthenticationExecutor Build()
        {
            if (XboxAuthStrategyFactory == null)
                throw new InvalidOperationException("Set XboxAuthStrategy first");
            var xboxAuthStrategy = XboxAuthStrategyFactory.Invoke(this);

            // GameAuthenticator
            var gameAuthenticator = new BEAuthenticator(HttpClient);

            // Executor
            return new XboxGameAuthenticationExecutor<BESession>(xboxAuthStrategy, gameAuthenticator);
        }

        public new async Task<BESession> ExecuteAsync()
        {
            var session = await ExecuteAsync();
            return (BESession)session;
        }
    }
}
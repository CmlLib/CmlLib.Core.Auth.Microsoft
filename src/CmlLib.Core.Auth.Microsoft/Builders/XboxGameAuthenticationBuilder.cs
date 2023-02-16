using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameAuthenticationBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;
        private Func<XboxGameAuthenticationBuilder, Task<XboxGameSession>>? _executor;

        public MicrosoftOAuthClientInfo? OAuthClientInfo { get; private set; }

        public XboxGameAuthenticationBuilder(XboxGameAuthenticationParameters parameters, MicrosoftOAuthClientInfo? clientInfo)
        {
            this._parameters = parameters;
            OAuthClientInfo = clientInfo;
        }

        public XboxGameAuthenticationBuilder WithExecutor(Func<XboxGameAuthenticationBuilder, Task<XboxGameSession>> executor)
        {
            this._executor = executor;
            return this;
        }

        public XboxGameAuthenticationBuilder WithGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            this._parameters.GameAuthenticator = authenticator;
            return this;
        }

        public MicrosoftOAuthBuilder WithMicrosoftOAuth()
        {
            if (OAuthClientInfo == null)
                throw new InvalidOperationException("No default OAuth client info set.");
            return WithMicrosoftOAuth(OAuthClientInfo);
        }

        public MicrosoftOAuthBuilder WithMicrosoftOAuth(MicrosoftOAuthClientInfo oAuthClientInfo)
        {
            return new MicrosoftOAuthBuilder(_parameters, oAuthClientInfo);
        }

        public SilentMicrosoftOAuthBuilder WithSilentMicrosoftOAuth()
        {
            if (OAuthClientInfo == null)
                throw new InvalidOperationException("No default OAuth client info set.");
            return WithSilentMicrosoftOAuth(OAuthClientInfo);
        }

        public SilentMicrosoftOAuthBuilder WithSilentMicrosoftOAuth(MicrosoftOAuthClientInfo oAuthClientInfo)
        {
            return new SilentMicrosoftOAuthBuilder(_parameters, oAuthClientInfo);
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            if (this._executor == null)
                throw new InvalidOperationException("No executor");
            else
                return this._executor(this);
        }
    }
}
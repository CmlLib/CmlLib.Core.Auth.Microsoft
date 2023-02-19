using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class AuthenticationBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;
        private Func<AuthenticationBuilder, Task<XboxGameSession>>? _executor;

        public MicrosoftOAuthClientInfo? OAuthClientInfo { get; private set; }

        public AuthenticationBuilder(XboxGameAuthenticationParameters parameters, MicrosoftOAuthClientInfo? clientInfo)
        {
            this._parameters = parameters;
            OAuthClientInfo = clientInfo;
        }

        public AuthenticationBuilder WithExecutor(Func<AuthenticationBuilder, Task<XboxGameSession>> executor)
        {
            this._executor = executor;
            return this;
        }

        public AuthenticationBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this._parameters.SessionStorage = sessionStorage;
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
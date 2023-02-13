using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Executors;
using CmlLib.Core.Auth.Microsoft.Builders.OAuth;
using CmlLib.Core.Auth.Microsoft.Builders.XboxAuth;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class SilentXboxGameAuthenticationBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;
        private readonly MicrosoftOAuthClientInfo? _oAuthClientInfo;

        public SilentXboxGameAuthenticationBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthClientInfo? oAuthClientInfo)
        {
            this._parameters = parameters;
            if (this._parameters.Executor == null)
                this._parameters.Executor = new XboxGameSilentAuthenticationExecutor(new XboxGameAuthenticationExecutor());
            this._oAuthClientInfo = oAuthClientInfo;
        }

        public SilentXboxGameAuthenticationBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this._parameters.SessionStorage = sessionStorage;
            return this;
        }

        public SilentMicrosoftOAuthBuilder WithMicrosoftOAuth()
        {
            if (this._oAuthClientInfo == null)
                throw new InvalidOperationException("no default client info. use WithMicrosoftOAuth(MicrosoftOAuthClientInfo oAuthClientInfo).");
            return WithMicrosoftOAuth(this._oAuthClientInfo);
        }

        public SilentMicrosoftOAuthBuilder WithMicrosoftOAuth(MicrosoftOAuthClientInfo clientInfo)
        {
            return new SilentMicrosoftOAuthBuilder(_parameters, clientInfo);
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            return WithMicrosoftOAuth().ExecuteAsync();
        }
    }
}
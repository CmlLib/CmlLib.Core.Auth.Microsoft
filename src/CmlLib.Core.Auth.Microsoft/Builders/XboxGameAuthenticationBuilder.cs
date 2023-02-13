using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Executors;
using CmlLib.Core.Auth.Microsoft.Builders.OAuth;
using CmlLib.Core.Auth.Microsoft.Builders.XboxAuth;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameAuthenticationBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;
        private readonly MicrosoftOAuthClientInfo? _oAuthClientInfo;

        public XboxGameAuthenticationBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthClientInfo? oAuthClientInfo)
        {
            this._parameters = parameters;
            if (this._parameters.Executor == null)
                this._parameters.Executor = new XboxGameAuthenticationExecutor();
            this._oAuthClientInfo = oAuthClientInfo;
        }

        public XboxGameAuthenticationBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this._parameters.SessionStorage = sessionStorage;
            return this;
        }

        public MicrosoftOAuthBuilder WithMicrosoftOAuth()
        {
            if (this._oAuthClientInfo == null)
                throw new InvalidOperationException("no default client info. use WithMicrosoftOAuth(MicrosoftOAuthClientInfo oAuthClientInfo).");
            return WithMicrosoftOAuth(_oAuthClientInfo);
        }

        public MicrosoftOAuthBuilder WithMicrosoftOAuth(MicrosoftOAuthClientInfo oAuthClientInfo)
        {
            return new MicrosoftOAuthBuilder(_parameters, oAuthClientInfo);
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            return WithMicrosoftOAuth().ExecuteAsync();
        }
    }
}
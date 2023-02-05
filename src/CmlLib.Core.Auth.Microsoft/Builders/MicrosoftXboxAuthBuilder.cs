using System;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftXboxAuthBuilder : XboxAuthBuilder
    {
        private IMicrosoftOAuthStrategy _oAuthStrategy;

        public MicrosoftXboxAuthBuilder(
            IMicrosoftOAuthStrategy oAuthStrategy,
            XboxGameAuthenticationParameters parameters) : base(parameters)
        {
            this._oAuthStrategy = oAuthStrategy;
        }

        public MicrosoftXboxAuthBuilder WithBasicXboxAuth()
        {
            WithXboxAuthStrategy(oAuthStrategy => new BasicXboxAuthStrategy(Parameters.HttpClient, oAuthStrategy));
            return this;
        }

        public MicrosoftXboxAuthBuilder WithXboxAuthStrategy(Func<IMicrosoftOAuthStrategy, IXboxAuthStrategy> factory)
        {
            var xboxAuthStrategy = factory.Invoke(_oAuthStrategy);
            WithXboxAuthStrategy(xboxAuthStrategy);
            return this;
        }
    }
}
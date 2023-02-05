using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JEAuthenticationBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;

        public JEAuthenticationBuilder(XboxGameAuthenticationParameters parameters)
        {
            this._parameters = parameters;
            this._parameters.Executor = new XboxGameAuthenticationExecutor();
        }

        public MicrosoftOAuthBuilder WithMicrosoftOAuth()
        {
            return WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo);
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
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JESilentAuthenticationBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;

        public JESilentAuthenticationBuilder(XboxGameAuthenticationParameters parameters)
        {
            this._parameters = parameters;
            this._parameters.Executor = new XboxGameSilentAuthenticationExecutor(new XboxGameAuthenticationExecutor());
        }

        public SilentMicrosoftOAuthBuilder WithMicrosoftOAuth()
        {
            return WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo);
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
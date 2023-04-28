using System.Net.Http;
using XboxAuthNet.Game;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JELoginHandlerBuilder : LoginHandlerBuilderBase
    {
        public static JELoginHandler CreateDefault() =>
            new JELoginHandlerBuilder().Build();

        public JELoginHandler Build()
        {
            return new JELoginHandler(HttpClient, accountManager);
        }
    }
}

using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public interface IWebUI
    {
        Task<MicrosoftOAuthCode> GetAuthCode(IWebUILoginHandler loginHandler);
    }
}

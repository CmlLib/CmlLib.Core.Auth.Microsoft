using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public interface IWebUI
    {
        Task ShowUri(Uri uri);
        Task<MicrosoftOAuthCode> GetAuthCode(IWebUILoginHandler loginHandler);
    }
}

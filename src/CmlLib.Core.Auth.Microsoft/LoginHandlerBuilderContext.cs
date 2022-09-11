using System.Net.Http;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandlerBuilderContext
    {
        public HttpClient HttpClient { get; }
        public string? ClientId { get; set; }
        public string? CachePath { get; set; }

        public LoginHandlerBuilderContext(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
    }
}

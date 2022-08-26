using System.Net.Http;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginBuilderContext
    {
        public HttpClient? HttpClient { get; set; }
        public string? ClientId { get; set; }
        public string? CachePath { get; set; }
    }
}

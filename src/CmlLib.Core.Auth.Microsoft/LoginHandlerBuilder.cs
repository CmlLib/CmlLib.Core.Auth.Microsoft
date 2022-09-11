using System.Net.Http;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandlerBuilder
    {
        public static LoginHandlerBuilder Create()
        {
            return new LoginHandlerBuilder();
        }

        private HttpClient? httpClient;
        private string? clientId;
        private string? cachePath;

        public LoginHandlerBuilder WithHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            return this;
        }

        public LoginHandlerBuilder WithClientId(string cid)
        {
            this.clientId = cid;
            return this;
        }

        public LoginHandlerBuilder WithCachePath(string cachePath)
        {
            this.cachePath = cachePath;
            return this;
        }

        public LoginHandlerBuilderContext Build()
        {
            if (httpClient == null)
                httpClient = HttpHelper.DefaultHttpClient.Value;

            return new LoginHandlerBuilderContext(httpClient)
            {
                ClientId = clientId,
                CachePath = cachePath
            };
        }
    }
}

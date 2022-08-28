using System.Net.Http;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthNetApiBuilder
    {
        private XboxAuth? xboxAuth;
        private IXboxAuthTokenApi? deviceTokenApi;
        private IXboxAuthTokenApi? titleTokenApi;
        private string? tokenPrefix;
        public HttpClient? HttpClient { get; private set; }

        public XboxAuthNetApiBuilder()
        {

        }

        public XboxAuthNetApiBuilder WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = HttpClient;
            return this;
        }

        public XboxAuthNetApiBuilder WithXboxAuth(XboxAuth xa)
        {
            this.xboxAuth = xa;
            return this;
        }

        public XboxAuthNetApiBuilder WithDeviceTokenApi(IXboxAuthTokenApi deviceToken)
        {
            this.deviceTokenApi = deviceToken;
            return this;
        }

        public XboxAuthNetApiBuilder WithDummyDeviceTokenApi()
        {
            return WithDeviceTokenApi(new DummyDeviceTokenApi(HttpClient ?? HttpHelper.DefaultHttpClient.Value));
        }

        public XboxAuthNetApiBuilder WithTitleTokenApi(IXboxAuthTokenApi titleToken)
        {
            this.titleTokenApi = titleToken;
            return this;
        }

        public XboxAuthNetApiBuilder WithTokenPrefix(string prefix)
        {
            this.tokenPrefix = prefix;
            return this;
        }

        public XboxAuthNetApiBuilder WithAzureTokenPrefix()
        {
            return WithTokenPrefix(XboxSecureAuth.AzureTokenPrefix);
        }

        public XboxAuthNetApiBuilder WithXboxTokenPrefix()
        {
            return WithTokenPrefix(XboxSecureAuth.XboxTokenPrefix);
        }

        public XboxAuthNetApi Build()
        {
            if (string.IsNullOrEmpty(tokenPrefix))
                tokenPrefix = "";
            if (xboxAuth == null)
                xboxAuth = new XboxAuth(HttpClient);

            return new XboxAuthNetApi(
                xboxAuth, 
                tokenPrefix, 
                deviceTokenApi, 
                titleTokenApi);
        }
    }
}

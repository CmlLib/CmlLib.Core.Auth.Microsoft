using System.Net.Http;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthNetApiBuilder
    {
        private XboxAuth? _xboxAuth;
        private IXboxAuthTokenApi? _deviceTokenApi;
        private IXboxAuthTokenApi? _titleTokenApi;
        private string? _tokenPrefix;
        public HttpClient? HttpClient { get; private set; }

        public XboxAuthNetApiBuilder()
        {

        }

        public XboxAuthNetApiBuilder WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public XboxAuthNetApiBuilder WithXboxAuth(XboxAuth xa)
        {
            this._xboxAuth = xa;
            return this;
        }

        public XboxAuthNetApiBuilder WithDeviceTokenApi(IXboxAuthTokenApi deviceToken)
        {
            this._deviceTokenApi = deviceToken;
            return this;
        }

        public XboxAuthNetApiBuilder WithDummyDeviceTokenApi()
        {
            return WithDeviceTokenApi(new DummyDeviceTokenApi(HttpClient ?? HttpHelper.DefaultHttpClient.Value));
        }

        public XboxAuthNetApiBuilder WithTitleTokenApi(IXboxAuthTokenApi titleToken)
        {
            this._titleTokenApi = titleToken;
            return this;
        }

        public XboxAuthNetApiBuilder WithTokenPrefix(string prefix)
        {
            this._tokenPrefix = prefix;
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
            if (string.IsNullOrEmpty(_tokenPrefix))
                _tokenPrefix = "";
            if (_xboxAuth == null)
                _xboxAuth = new XboxAuth(HttpClient ?? HttpHelper.DefaultHttpClient.Value);

            return new XboxAuthNetApi(
                _xboxAuth, 
                _tokenPrefix, 
                _deviceTokenApi, 
                _titleTokenApi);
        }
    }
}

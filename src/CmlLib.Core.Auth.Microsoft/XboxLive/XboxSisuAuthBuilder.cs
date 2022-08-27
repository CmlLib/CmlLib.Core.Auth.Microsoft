using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Net.Http;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxSisuAuthBuilder
    {
        private HttpClient? HttpClient;
        private IXboxSisuRequestSigner? Signer;
        private readonly XboxSisuAuthParameters _parameters;

        public XboxSisuAuthBuilder()
        {
            this._parameters = new XboxSisuAuthParameters();
        }

        public XboxSisuAuthBuilder WithSigner(IXboxSisuRequestSigner signer)
        {
            Signer = signer;
            return this;
        }

        public XboxSisuAuthBuilder WithECKeyPairGenerator(IAsymmetricCipherKeyPairGenerator keyPairGenerator)
        {
            var keyPair = keyPairGenerator.GenerateKeyPair();
            Signer = new ECXboxSisuRequestSigner(
                (ECPublicKeyParameters)keyPair.Public, 
                (ECPrivateKeyParameters)keyPair.Private);
            return this;
        }

        public XboxSisuAuthBuilder WithHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
            return this;
        }

        public XboxSisuAuthBuilder WithWin32Device()
        {
            return WithDeviceType(XboxDeviceTypes.Win32)
                  .WithDeviceVersion("10.0.18363");
        }

        public XboxSisuAuthBuilder WithNintendoDevice()
        {
            return WithDeviceType(XboxDeviceTypes.Nintendo)
                  .WithDeviceVersion("0.0.0");
        }

        public XboxSisuAuthBuilder WithiOSDevice()
        {
            return WithDeviceType(XboxDeviceTypes.iOS)
                  .WithDeviceType("0.0.0");
        }

        public XboxSisuAuthBuilder WithDeviceType(string deviceType)
        {
            _parameters.DeviceType = deviceType;
            return this;
        }

        public XboxSisuAuthBuilder WithDeviceVersion(string deviceVersion)
        {
            _parameters.DeviceVersion = deviceVersion;
            return this;
        }

        public XboxSisuAuthBuilder WithClientId(string clientId)
        {
            _parameters.ClientId = clientId;
            return this;
        }

        public XboxSisuAuthBuilder WithTokenPrefix(string tokenPrefix)
        {
            _parameters.TokenPrefix = tokenPrefix;
            return this;
        }

        public XboxSisuAuthBuilder WithAzureTokenPrefix()
        {
            return WithTokenPrefix(XboxSecureAuth.AzureTokenPrefix);
        }

        public XboxSisuAuthBuilder WithXboxTokenPrefix()
        {
            return WithTokenPrefix(XboxSecureAuth.XboxTokenPrefix);
        }

        public XboxSisuAuthApi Build()
        {
            if (HttpClient == null)
                HttpClient = HttpHelper.DefaultHttpClient.Value;
            if (Signer == null)
                throw new InvalidOperationException("Call WithSigner first");

            var xbox = new XboxSecureAuth(HttpClient, Signer);
            return new XboxSisuAuthApi(xbox, _parameters);
        }
    }
}

using System;
using System.Net.Http;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftOAuthClientInfo
    {
        public string? ClientId { get; set; }
        public string? Scopes { get; set; }

        public MicrosoftOAuthCodeApiClient CreateApiClientForOAuthCode(HttpClient httpClient)
        {   
            if (string.IsNullOrEmpty(ClientId))
                throw new InvalidOperationException("ClientId was empty");
            if (string.IsNullOrEmpty(Scopes))
                throw new InvalidCastException("Scopes was empty");

            return new MicrosoftOAuthCodeApiClient(ClientId, Scopes, httpClient);
        }
    }
}
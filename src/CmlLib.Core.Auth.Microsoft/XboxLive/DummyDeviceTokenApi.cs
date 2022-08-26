using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    // EXPERIMENTAL: bypass age detection
    // https://github.com/XboxReplay/xboxlive-auth/blob/83fbe6f9a9224b44a0ef0562947bc3315fc0e82e/src/core/xboxlive/index.ts#L175
    // https://github.com/OpenXbox/xcloud-python/blob/master/xcloud/auth/xal_auth.py
    public class DummyDeviceTokenApi : IXboxAuthTokenApi
    {
        private readonly HttpClient _httpClient;

        public DummyDeviceTokenApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<XboxAuthResponse?> GetToken(XboxAuthResponse? previousToken)
        {
            try
            {
                if (previousToken != null && checkDeviceTokenValidation(previousToken))
                    return previousToken;
                else
                {
                    using var httpClient = new HttpClient();
                    return await requestDeviceToken();
                }
            }
            catch (Exception ex)
            {
                // this method is experimental, can be break or removed anytime.
                // since this token is not necessary for minecraft authentication, simply ignore device token if error occurs.
                return null;
            }
        }

        private bool checkDeviceTokenValidation(XboxAuthResponse deviceToken)
        {
            if (deviceToken == null)
                return false;

            if (!DateTime.TryParse(deviceToken.ExpireOn, out var expireOn))
                return false;

            if (DateTime.Now > expireOn)
                return false;

            return true;
        }

        private async Task<XboxAuthResponse?> requestDeviceToken()
        {
            var req = "{\"RelyingParty\":\"http://auth.xboxlive.com\",\"TokenType\":\"JWT\",\"Properties\":{\"AuthMethod\":\"ProofOfPossession\",\"TrustedParty\":\"https://xboxreplay.net/\",\"Id\":\"{21354D2F-352F-472F-5842-5265706C6179}\",\"DeviceType\":\"Win32\",\"Version\":\"10.0.18363\",\"ProofKey\":{\"crv\":\"P-256\",\"alg\":\"ES256\",\"use\":\"sig\",\"kty\":\"EC\",\"x\":\"b8Zc6GPFeu41DqiWPJxRa_jqUTSiMA537emKVHt8UO8\",\"y\":\"CXAuTEHet72GjgSDfDg6psBrwE1waxBsNEIGrRZV_90\"}}}";
            var reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://device.auth.xboxlive.com/device/authenticate"),
                Method = HttpMethod.Post,
                Content = new StringContent(req, Encoding.UTF8, "application/json"),
            };
            reqMessage.Headers.Add("Accept", "application/json");
            reqMessage.Headers.TryAddWithoutValidation("User-Agent", "XboxReplay; XboxLiveAuth/4.0");
            reqMessage.Headers.Add("Accept-Encoding", "gzip, deflate, compress");
            reqMessage.Headers.Add("Accept-Language", "en-US, en;q=0.9");
            reqMessage.Headers.Add("X-Xbl-Contract-Version", "2");
            reqMessage.Headers.Add("Signature", "AAAAAQHW6oD31MwA6MAjn67vdCppWCbrMovubA85xejO06rtOAEdZ0tMTZFnu7xbI6lZDNvIWfuMaIPJSUcpvxjKqSFJl1oaWzQGBw==");

            var res = await _httpClient.SendAsync(reqMessage);
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<XboxAuthResponse>(json);
        }
    }
}

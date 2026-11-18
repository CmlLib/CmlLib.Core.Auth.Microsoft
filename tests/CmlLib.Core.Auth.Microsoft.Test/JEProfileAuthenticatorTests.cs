using System.Net;
using System.Net.Http.Headers;
using CmlLib.Core.Auth.Microsoft.Authenticators;
using CmlLib.Core.Auth.Microsoft.Sessions;
using Microsoft.Extensions.Logging.Abstractions;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Test;

public class JEProfileAuthenticatorTests
{
    [Theory]
    [InlineData("60", "{\"error\":\"TooManyRequests\"}", "TooManyRequests")]
    [InlineData("Wed, 21 Oct 2026 07:28:00 GMT", "", null)]
    [InlineData("0", "null", null)]
    [InlineData(null, "", null)]
    [InlineData("invalid", "", null)]
    public async Task ProfileRateLimitPreservesRetryAfterAndStatusCode(
        string? retryAfter,
        string body,
        string? errorCode)
    {
        using var httpClient = new HttpClient(new RateLimitedProfileHandler(retryAfter, body));
        var context = new AuthenticateContext(
            new InMemorySessionStorage(),
            httpClient,
            CancellationToken.None,
            NullLogger.Instance);
        var tokenSource = new InMemorySessionSource<JEToken>(new JEToken { AccessToken = "test-token" });
        var authenticator = new JEProfileAuthenticator(tokenSource, JEProfileSource.Default);

        var error = await Assert.ThrowsAsync<JEAuthException>(async () => await authenticator.ExecuteAsync(context));

        RetryConditionHeaderValue.TryParse(retryAfter, out var expectedRetryAfter);
        Assert.Equal(429, error.StatusCode);
        Assert.Equal(expectedRetryAfter, error.RetryAfter);
        Assert.Equal(errorCode, error.Error);
    }

    private sealed class RateLimitedProfileHandler(string? retryAfter, string body) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent(body)
            };
            if (retryAfter != null)
                response.Headers.TryAddWithoutValidation("Retry-After", retryAfter);

            return Task.FromResult(response);
        }
    }
}

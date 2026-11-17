using System.Net;
using CmlLib.Core.Auth.Microsoft.Authenticators;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.Game.Authenticators;

namespace CmlLib.Core.Auth.Microsoft.Test;

public class JEProfileRefreshTests
{
    private const string ProfileId = "0123456789abcdef0123456789abcdef";
    private const string LegacyAccountJson = $$"""
        { "{{ProfileId}}": { "JEProfile": { "id": "{{ProfileId}}", "name": "SavedPlayer" } } }
        """;
    private const string RefreshedProfileJson = $$"""
        { "id": "{{ProfileId}}", "name": "UpdatedPlayer" }
        """;

    [Fact]
    public async Task successful_refresh_saves_utc_timestamp_for_legacy_account()
    {
        var accountFile = Path.GetTempFileName();
        try
        {
            await File.WriteAllTextAsync(accountFile, LegacyAccountJson);
            using var httpClient = new HttpClient(new ProfileResponseHandler(HttpStatusCode.OK, RefreshedProfileJson));
            var login = new JELoginHandlerBuilder()
                .WithAccountManager(accountFile)
                .WithHttpClient(httpClient)
                .Build();
            var account = Assert.IsType<JEGameAccount>(Assert.Single(login.AccountManager.GetAccounts()));
            Assert.Null(Assert.IsType<JEProfile>(account.Profile).LastRefreshedAt);

            var beforeRefresh = DateTimeOffset.UtcNow;
            await RefreshProfileAsync(login, account);
            var afterRefresh = DateTimeOffset.UtcNow;

            var profile = ReadSavedProfile(accountFile);
            Assert.Equal("UpdatedPlayer", profile.Username);
            var refreshedAt = Assert.IsType<DateTimeOffset>(profile.LastRefreshedAt);
            Assert.Equal(TimeSpan.Zero, refreshedAt.Offset);
            Assert.InRange(refreshedAt, beforeRefresh, afterRefresh);
        }
        finally
        {
            File.Delete(accountFile);
        }
    }

    [Theory]
    [InlineData(HttpStatusCode.TooManyRequests, """{ "error": "TooManyRequests" }""")]
    [InlineData(HttpStatusCode.OK, """{ "name": "IncompletePlayer" }""")]
    public async Task failed_refresh_preserves_saved_profile_and_timestamp(HttpStatusCode status, string body)
    {
        var accountFile = Path.GetTempFileName();
        try
        {
            var savedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var accountJson = $$"""
                { "{{ProfileId}}": { "JEProfile": {
                    "id": "{{ProfileId}}", "name": "SavedPlayer", "lastRefreshedAt": "{{savedAt:O}}"
                } } }
                """;
            await File.WriteAllTextAsync(accountFile, accountJson);
            using var httpClient = new HttpClient(new ProfileResponseHandler(status, body));
            var login = new JELoginHandlerBuilder()
                .WithAccountManager(accountFile)
                .WithHttpClient(httpClient)
                .Build();
            var account = Assert.IsType<JEGameAccount>(Assert.Single(login.AccountManager.GetAccounts()));

            await Assert.ThrowsAsync<JEAuthException>(() => RefreshProfileAsync(login, account));

            Assert.Equal(savedAt, account.Profile?.LastRefreshedAt);
            var profile = ReadSavedProfile(accountFile);
            Assert.Equal("SavedPlayer", profile.Username);
            Assert.Equal(savedAt, profile.LastRefreshedAt);
        }
        finally
        {
            File.Delete(accountFile);
        }
    }

    private static async Task RefreshProfileAsync(JELoginHandler login, JEGameAccount account)
    {
        JETokenSource.Default.Set(account.SessionStorage, new JEToken { AccessToken = "test-token" });
        var authenticator = login.CreateAuthenticator(account, CancellationToken.None);
        authenticator.AddAuthenticator(StaticValidator.Invalid, new JEAuthenticatorBuilder().ProfileAuthenticator());
        await authenticator.ExecuteAsync();
    }

    private static JEProfile ReadSavedProfile(string accountFile)
    {
        var login = new JELoginHandlerBuilder()
            .WithAccountManager(accountFile)
            .Build();
        var account = Assert.IsType<JEGameAccount>(Assert.Single(login.AccountManager.GetAccounts()));
        return Assert.IsType<JEProfile>(account.Profile);
    }

    private sealed class ProfileResponseHandler(HttpStatusCode status, string body) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(body) });
        }
    }
}

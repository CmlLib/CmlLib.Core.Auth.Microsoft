using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.Msal;
using XboxAuthNet.Game.Msal.OAuth;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Test;

public class MsalOAuthTests
{
    private const string ClientId = "00000000-0000-0000-0000-000000000001";
    private const string TenantId = "11111111-1111-1111-1111-111111111111";
    private const string UserId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
    private const string HomeAccountId = UserId + "." + TenantId;
    private const string Username = "test@example.invalid";

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FromResultPersistsAccountMetadataAndClearSessionRemovesIt(bool customSources)
    {
        var app = MsalClientHelper.BuildApplication(ClientId);
        var builder = new MsalOAuthBuilder(app);
        if (customSources)
        {
            builder.HomeAccountIdSource = new SessionFromStorage<string>("CustomHomeAccountId");
            builder.LoginHintSource = new SessionFromStorage<string>("CustomLoginHint");
        }
        var storage = JsonSessionStorage.CreateEmpty();
        using var http = new HttpClient();
        var context = Context(storage, http);
        await builder.FromResult(Result()).ExecuteAsync(context);

        Assert.Equal(HomeAccountId, builder.HomeAccountIdSource.Get(storage));
        Assert.Equal(Username, builder.LoginHintSource.Get(storage));
        Assert.NotNull(builder.SessionSource.Get(storage));
        Assert.Equal(SessionStorageKeyMode.NoStore, builder.SessionSource.GetKeyMode(storage));
        var reloaded = new JsonSessionStorage(storage.ToJsonObjectForStoring());
        Assert.Equal(HomeAccountId, builder.HomeAccountIdSource.Get(reloaded));
        Assert.Equal(Username, builder.LoginHintSource.Get(reloaded));
        Assert.Null(builder.SessionSource.Get(reloaded));
        if (customSources)
            Assert.Null(MsalHomeAccountIdSource.Default.Get(storage));

        await builder.ClearSession().ExecuteAsync(context);
        Assert.Null(builder.HomeAccountIdSource.Get(storage));
        Assert.Null(builder.LoginHintSource.Get(storage));
        Assert.Null(builder.SessionSource.Get(storage));
    }

    [Fact]
    public async Task SilentUsesHomeAccountIdWhenLoginHintMatchesMultipleAccounts()
    {
        using var network = new MsalNetwork();
        var app = Application(network, multipleAccounts: true);
        var storage = new InMemorySessionStorage();
        MsalHomeAccountIdSource.Default.Set(storage, HomeAccountId);
        MicrosoftOAuthLoginHintSource.Default.Set(storage, Username);
        using var http = new HttpClient();
        await new MsalOAuthBuilder(app).Silent().ExecuteAsync(Context(storage, http));
        Assert.Equal("synthetic-refresh-a", network.RefreshToken);
        Assert.Equal(HomeAccountId, MsalHomeAccountIdSource.Default.Get(storage));
    }

    [Fact]
    public async Task SilentReadsLegacyLoginHintAndAddsHomeAccountId()
    {
        using var network = new MsalNetwork();
        var app = Application(network);
        var storage = new InMemorySessionStorage();
        MicrosoftOAuthLoginHintSource.Default.Set(storage, Username);
        using var http = new HttpClient();
        await new MsalOAuthBuilder(app).Silent().ExecuteAsync(Context(storage, http));
        Assert.Equal(HomeAccountId, MsalHomeAccountIdSource.Default.Get(storage));
        Assert.Equal("synthetic-refresh-a", network.RefreshToken);
    }

    [Fact]
    public async Task MissingStoredHomeAccountDoesNotFallBackToADifferentCachedAccount()
    {
        using var network = new MsalNetwork();
        var app = Application(network);
        var storage = new InMemorySessionStorage();
        MsalHomeAccountIdSource.Default.Set(storage, "missing-account");
        MicrosoftOAuthLoginHintSource.Default.Set(storage, Username);
        using var http = new HttpClient();
        var error = await Assert.ThrowsAsync<MsalUiRequiredException>(async () =>
            await new MsalOAuthBuilder(app).Silent().ExecuteAsync(Context(storage, http)));
        Assert.Equal("account_not_found", error.ErrorCode);
        Assert.False(network.TokenRequested.Task.IsCompleted);
        Assert.Null(MicrosoftOAuthSessionSource.Default.Get(storage));
    }

    [Fact]
    public async Task SilentPassesCancellationToAnInFlightMsalTokenRequest()
    {
        using var network = new MsalNetwork(blockToken: true);
        var app = Application(network);
        var storage = new InMemorySessionStorage();
        MsalHomeAccountIdSource.Default.Set(storage, HomeAccountId);
        using var cancellation = new CancellationTokenSource();
        using var http = new HttpClient();
        var authentication = new MsalOAuthBuilder(app).Silent()
            .ExecuteAsync(Context(storage, http, cancellation.Token))
            .AsTask();
        var first = await Task.WhenAny(network.TokenRequested.Task, authentication);
        if (first == authentication)
            await authentication;

        await network.TokenRequested.Task.WaitAsync(TimeSpan.FromSeconds(5));
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await authentication.WaitAsync(TimeSpan.FromSeconds(5)));
        Assert.Null(MicrosoftOAuthSessionSource.Default.Get(storage));
    }

    [Fact]
    public async Task CancelledResultDoesNotWriteAccountMetadata()
    {
        var app = MsalClientHelper.BuildApplication(ClientId);
        var storage = new InMemorySessionStorage();
        using var http = new HttpClient();
        using var cancellation = new CancellationTokenSource();
        var parameters = new MsalOAuthParameters(
            app,
            MsalClientHelper.XboxScopes,
            MicrosoftOAuthLoginHintSource.Default,
            false,
            MicrosoftOAuthSessionSource.Default);
        var authenticator = new CancelledResultOAuth(parameters, cancellation);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await authenticator.ExecuteAsync(Context(storage, http, cancellation.Token)));
        Assert.Empty(storage.Keys);
    }

    private static AuthenticateContext Context(
        ISessionStorage storage,
        HttpClient http,
        CancellationToken cancellationToken = default) =>
        new(
            storage,
            http,
            cancellationToken,
            NullLogger.Instance);

    private static AuthenticationResult Result() => new(
        "synthetic-access-token",
        false,
        UserId,
        DateTimeOffset.UtcNow.AddHours(1),
        DateTimeOffset.UtcNow.AddHours(1),
        TenantId,
        new TestAccount(),
        "",
        MsalClientHelper.XboxScopes,
        Guid.NewGuid());

    private static IPublicClientApplication Application(MsalNetwork network, bool multipleAccounts = false)
    {
        var app = PublicClientApplicationBuilder.Create(ClientId)
            .WithAuthority($"https://login.microsoftonline.com/{TenantId}")
            .WithInstanceDiscovery(false)
            .WithHttpClientFactory(network)
            .Build();
        var accounts = new Dictionary<string, object>();
        var refreshTokens = new Dictionary<string, object>();
        AddCachedAccount(
            accounts,
            refreshTokens,
            UserId,
            "synthetic-refresh-a");
        if (multipleAccounts)
            AddCachedAccount(
                accounts,
                refreshTokens,
                "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
                "synthetic-refresh-b");

        var cache = new Dictionary<string, object>
        {
            ["Account"] = accounts,
            ["RefreshToken"] = refreshTokens
        };
        var cacheBytes = JsonSerializer.SerializeToUtf8Bytes(cache);
        app.UserTokenCache.SetBeforeAccess(args => args.TokenCache.DeserializeMsalV3(cacheBytes));
        app.UserTokenCache.SetAfterAccess(args =>
        {
            if (args.HasStateChanged)
                cacheBytes = args.TokenCache.SerializeMsalV3();
        });
        return app;
    }

    private static void AddCachedAccount(
        Dictionary<string, object> accounts,
        Dictionary<string, object> refreshTokens,
        string userId,
        string refreshToken)
    {
        var homeAccountId = userId + "." + TenantId;
        accounts[$"{homeAccountId}-login.microsoftonline.com-{TenantId}"] = new
        {
            home_account_id = homeAccountId,
            environment = "login.microsoftonline.com",
            realm = TenantId,
            local_account_id = userId,
            username = Username,
            authority_type = "MSSTS"
        };
        refreshTokens[$"{homeAccountId}-login.microsoftonline.com-refreshtoken-{ClientId}--"] = new
        {
            home_account_id = homeAccountId,
            environment = "login.microsoftonline.com",
            credential_type = "RefreshToken",
            client_id = ClientId,
            secret = refreshToken
        };
    }

    private sealed class TestAccount : IAccount
    {
        public string Username => MsalOAuthTests.Username;
        public string Environment => "login.microsoftonline.com";
        public AccountId HomeAccountId => new(MsalOAuthTests.HomeAccountId, UserId, TenantId);
    }

    private sealed class CancelledResultOAuth(
        MsalOAuthParameters parameters,
        CancellationTokenSource cancellation) : MsalOAuth(parameters)
    {
        protected override ValueTask<AuthenticationResult> AuthenticateWithMsal(
            AuthenticateContext context,
            MsalOAuthParameters parameters)
        {
            cancellation.Cancel();
            return ValueTask.FromResult(Result());
        }
    }

    private sealed class MsalNetwork : HttpMessageHandler, IMsalHttpClientFactory
    {
        private readonly HttpClient _client;
        private readonly bool _blockToken;
        public TaskCompletionSource TokenRequested { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public string? RefreshToken { get; private set; }

        public MsalNetwork(bool blockToken = false)
        {
            _blockToken = blockToken;
            _client = new HttpClient(this, disposeHandler: false);
        }

        public HttpClient GetHttpClient() => _client;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/.well-known/openid-configuration"))
            {
                return JsonResponse(new
                {
                    authorization_endpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/authorize",
                    token_endpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token",
                    issuer = $"https://login.microsoftonline.com/{TenantId}/v2.0",
                    jwks_uri = "https://login.microsoftonline.com/common/discovery/v2.0/keys"
                });
            }

            if (!request.RequestUri.AbsolutePath.EndsWith("/token"))
                throw new InvalidOperationException("예상하지 못한 외부 요청입니다: " + request.RequestUri);

            var body = await request.Content!.ReadAsStringAsync(cancellationToken);
            RefreshToken = body.Split('&')
                .Where(field => field.StartsWith("refresh_token="))
                .Select(field => Uri.UnescapeDataString(field.Substring("refresh_token=".Length)))
                .Single();
            TokenRequested.TrySetResult();
            if (_blockToken)
                await Task.Delay(Timeout.Infinite, cancellationToken);

            var header = Base64Url(new { alg = "none" });
            var payload = Base64Url(new
            {
                aud = ClientId,
                iss = $"https://login.microsoftonline.com/{TenantId}/v2.0",
                sub = UserId,
                oid = UserId,
                tid = TenantId,
                preferred_username = Username,
                exp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
            });
            return JsonResponse(new
            {
                token_type = "Bearer",
                access_token = "synthetic-access-token",
                refresh_token = RefreshToken,
                id_token = header + "." + payload + ".",
                client_info = Base64Url(new { uid = UserId, utid = TenantId }),
                expires_in = 3600,
                scope = "XboxLive.signin"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _client.Dispose();
            base.Dispose(disposing);
        }

        private static string Base64Url(object value) => Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(value))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        private static HttpResponseMessage JsonResponse(object value) => new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        };
    }
}

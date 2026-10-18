using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Authenticators;
using CmlLib.Core.Auth.Microsoft.Serialization;
using CmlLib.Core.Auth.Microsoft.Sessions;
using Microsoft.Extensions.Logging.Abstractions;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.Serialization;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.XboxLive.Responses;

var checks = 0;
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = JsonTypeInfoResolver.Combine(MicrosoftAuthJsonContext.Default, SmokeJsonContext.Default)
};
var storage = JsonSessionStorage.CreateEmpty(options);
storage.Set("integer", 42);
storage.Set("text", "한글");
storage.Set("number", 3.14);
storage.Set("custom", new CustomSession { Value = "custom" });
JETokenSource.Default.Set(storage, new JEToken { AccessToken = "token", ExpiresIn = 3600 });
XboxSessionSource.Default.Set(storage, new XboxAuthTokens
{
    XstsToken = new XboxAuthResponse
    {
        Token = "xsts",
        XuiClaims = new XboxAuthXuiClaims { UserHash = "hash", XboxUserId = "123" }
    }
});
JEProfileSource.Default.Set(storage, new JEProfile { UUID = "uuid", Username = "fixture" });
storage.Set("temporary", "secret");
storage.SetKeyMode("temporary", SessionStorageKeyMode.NoStore);
var json = storage.ToJsonObjectForStoring();
Check(!json.ContainsKey("temporary"), "NoStore");
var restored = new JsonSessionStorage(JsonNode.Parse(json.ToJsonString())!.AsObject(), options);
Check(restored.Get<int>("integer") == 42, "integer roundtrip");
Check(restored.Get<string>("text") == "한글", "unicode roundtrip");
Check(restored.Get<double>("number") == 3.14, "number roundtrip");
Check(restored.Get<CustomSession>("custom").Value == "custom", "custom session metadata");
Check(JETokenSource.Default.Get(restored)?.AccessToken == "token", "JE token storage");
Check(JEProfileSource.Default.Get(restored)?.UUID == "uuid", "JE profile storage");
Check(XboxSessionSource.Default.Get(restored)?.XstsToken?.XuiClaims?.XboxUserId == "123", "Xbox claims storage");
Check(restored.ToJsonObject().ContainsKey("custom"), "typed cache rewrite");
Check(options.TypeInfoResolver is not null && options.WriteIndented, "caller options retained");
var converterOptions = new JsonSerializerOptions();
converterOptions.Converters.Add(new RootObjectConverter());
var converterStorage = JsonSessionStorage.CreateEmpty(converterOptions);
converterStorage.Set("object", 42);
Check(converterStorage.ToJsonObject()["object"]!.GetValue<string>() == "converted", "caller object converter");
var clone = new JsonSessionStorage(JsonNode.Parse(json.ToJsonString())!.AsObject(), options);
Check(clone.ToJsonObject().ToJsonString().Contains("uuid"), "unread JSON object rewrite");
Check(!clone.TryGetValue<string>("missing", out _), "missing key");

using (var doc = JsonDocument.Parse("{\"id\":\"uuid\",\"name\":\"fixture\",\"skins\":[{\"id\":\"skin\",\"variant\":\"CLASSIC\"}],\"capes\":[{\"id\":\"cape\"}]}"))
{
    var profile = JEProfile.ParseFromJson(doc.RootElement);
    Check(profile.Skins.First().Id == "skin", "skin metadata");
    Check(profile.Capes.First().Id == "cape", "cape metadata");
}
using (var doc = JsonDocument.Parse("{\"skins\":[7],\"capes\":false}"))
{
    var profile = JEProfile.ParseFromJson(doc.RootElement);
    Check(profile.Skins.Count == 0 && profile.Capes.Count == 0, "malformed skin/cape fallback");
}
var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"xuid\":\"123\",\"exp\":42}"))
    .TrimEnd('=');
var jwt = new JEToken { AccessToken = "a." + encoded + ".b" };
Check(jwt.DecodeAccesTokenPayload()?.Xuid == "123", "Minecraft JWT metadata");

var handler = new FixtureHandler();
using var http = new HttpClient(handler);
var context = new AuthenticateContext(storage, http, CancellationToken.None, NullLogger.Instance);
var authenticator = new JETokenAuthenticator(XboxSessionSource.Default, JETokenSource.Default);
await authenticator.ExecuteAsync(context);
Check(JETokenSource.Default.Get(storage)?.AccessToken == "mock-access", "Minecraft token endpoint");
Check(handler.RequestBody.Contains("XBL3.0 x=hash;xsts"), "Minecraft token request contract");
Check(JETokenSource.Default.Get(storage)?.ExpiresOn > DateTime.UtcNow, "Minecraft token expiry");
handler.ResponseBody = "null";
try
{
    await authenticator.ExecuteAsync(context);
    throw new Exception("Expected empty token error");
}
catch (JEAuthException)
{
    Check(true, "empty token response");
}
handler.Status = HttpStatusCode.Unauthorized;
handler.ResponseBody = "{\"error\":\"Unauthorized\",\"errorMessage\":\"fixture\"}";
try
{
    await authenticator.ExecuteAsync(context);
    throw new Exception("Expected token endpoint error");
}
catch (JEAuthException)
{
    Check(true, "token HTTP error");
}

var directory = Path.Combine(Path.GetTempPath(), "auth-aot-" + Guid.NewGuid());
Directory.CreateDirectory(directory);
try
{
    var sessionFile = Path.Combine(directory, "session.json");
    var fileStorage = new JsonFileSessionStorage(sessionFile);
    fileStorage.Set("text", "persisted");
    Check(new JsonFileSessionStorage(sessionFile).Get<string>("text") == "persisted", "default file storage metadata");
    var accountFile = Path.Combine(directory, "accounts.json");
    var launcher = new JELoginHandlerBuilder().WithAccountManager(accountFile).Build();
    launcher.AccountManager.GetAccounts();
    var account = launcher.AccountManager.NewAccount();
    JEProfileSource.Default.Set(account.SessionStorage, new JEProfile { UUID = "uuid", Username = "saved" });
    JETokenSource.Default.Set(account.SessionStorage, jwt);
    XboxSessionSource.Default.Set(account.SessionStorage, XboxSessionSource.Default.Get(storage));
    launcher.AccountManager.SaveAccounts();
    var loaded = new JELoginHandlerBuilder().WithAccountManager(accountFile).Build().AccountManager.GetDefaultAccount();
    var je = (JEGameAccount)loaded;
    Check(je.Profile?.Username == "saved" && je.Token?.AccessToken == jwt.AccessToken, "default builder account persistence");
    Check(je.ToLauncherSession().Xuid == "123", "launcher session conversion");
}
finally
{
    // 테스트가 직접 만든 임시 폴더만 정리한다.
    Directory.Delete(directory, true);
}
if (JsonSerializer.IsReflectionEnabledByDefault)
{
    var legacy = JsonSessionStorage.CreateEmpty();
    legacy.Set("anonymous", new { Value = "legacy" });
    Check(legacy.ToJsonObject().ToJsonString().Contains("legacy"), "legacy JIT session model");
}
Console.WriteLine($"Passed {checks} game/Minecraft auth checks. JSON reflection: {JsonSerializer.IsReflectionEnabledByDefault}");

void Check(bool condition, string name)
{
    if (!condition)
        throw new Exception(name);

    checks++;
}

sealed class FixtureHandler : HttpMessageHandler
{
    public string RequestBody { get; private set; } = "";
    public string ResponseBody { get; set; } = "{\"access_token\":\"mock-access\",\"expires_in\":3600}";
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        RequestBody = await request.Content!.ReadAsStringAsync(token);
        return new HttpResponseMessage(Status) { Content = new StringContent(ResponseBody) };
    }
}

public sealed class CustomSession
{
    public string? Value { get; set; }
}

[JsonSerializable(typeof(CustomSession))]
internal partial class SmokeJsonContext : JsonSerializerContext
{
}

sealed class RootObjectConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        return reader.GetString()!;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        writer.WriteStringValue("converted");
    }
}

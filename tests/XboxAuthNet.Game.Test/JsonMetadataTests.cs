using System.Text.Json;
using System.Text.Json.Nodes;
using NUnit.Framework;
using XboxAuthNet.Game.Jwt;
using XboxAuthNet.Game.Serialization;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class JsonMetadataTests
{
    [Test]
    public void CustomModelRoundTripsWithGeneratedMetadata()
    {
        var options = new JsonSerializerOptions(TestJsonContext.Default.Options)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var storage = JsonSessionStorage.CreateEmpty(options);
        var expected = new MockObject { Name = "Player", Age = 42 };
        storage.Set("custom", expected);
        storage.Set("builtIn", 123);

        var json = storage.ToJsonObject();
        Assert.That((string?)json["custom"]?["name"], Is.EqualTo("Player"));

        var restored = new JsonSessionStorage(json, options);
        Assert.That(restored.Get<MockObject>("custom"), Is.EqualTo(expected));
        Assert.That(restored.Get<int>("builtIn"), Is.EqualTo(123));
    }

    [Test]
    public void UnregisteredCustomModelDoesNotUseReflection()
    {
        var storage = new JsonSessionStorage(JsonNode.Parse("{\"custom\":{\"Name\":\"Player\"}}")!.AsObject());

        Assert.Throws<NotSupportedException>(() => storage.Get<MockObject>("custom"));
        Assert.Throws<NotSupportedException>(() => XboxGameJson.CreateOptions().GetTypeInfo(typeof(MockObject)));
    }

    [Test]
    public void JwtUsesCallerGeneratedMetadata()
    {
        var payload = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("{\"Name\":\"Player\"}"));
        var jwt = "header." + payload + ".signature";

        var result = JwtDecoder.DecodePayload(jwt, TestJsonContext.Default.MockObject);

        Assert.That(result!.Name, Is.EqualTo("Player"));
        Assert.Throws<NotSupportedException>(() => JwtDecoder.DecodePayload<MockObject>(jwt));
    }
}

using CmlLib.Core.Auth.Microsoft.Sessions;
using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft.Test;

public class JEProfileSerializerTests
{
    [Fact]
    public void parse_normal_json()
    {
        var json = """
              {
              "id" : "uuid",
              "name" : "username",
              "skins" : [ {
                "id" : "skin-id-1",
                "state" : "ACTIVE",
                "url" : "http://textures.minecraft.net/texture/key1",
                "textureKey" : "key1",
                "variant" : "CLASSIC"
              } ],
              "capes" : [ {
                "id" : "cape-id-1",
                "state" : "INACTIVE",
                "url" : "http://textures.minecraft.net/texture/key2",
                "alias" : "Pan"
              } ],
              "profileActions" : { }
            }
            """;

        using var doc = JsonDocument.Parse(json);
        var profile = JEProfile.ParseFromJson(doc.RootElement);
        Assert.Equal("username", profile?.Username);
        Assert.Equal("uuid", profile?.UUID);
        Assert.Equal("skin-id-1", profile?.Skins?.FirstOrDefault()?.Id);
        Assert.Equal("ACTIVE", profile?.Skins?.FirstOrDefault()?.State);
        Assert.Equal("http://textures.minecraft.net/texture/key1", profile?.Skins?.FirstOrDefault()?.Url);
        Assert.Equal("key1", profile?.Skins?.FirstOrDefault()?.TextureKey);
        Assert.Equal("CLASSIC", profile?.Skins?.FirstOrDefault()?.Variant);
        Assert.Equal("cape-id-1", profile?.Capes?.FirstOrDefault()?.Id);
        Assert.Equal("INACTIVE", profile?.Capes?.FirstOrDefault()?.State);
        Assert.Equal("http://textures.minecraft.net/texture/key2", profile?.Capes?.FirstOrDefault()?.Url);
        Assert.Equal("Pan", profile?.Capes?.FirstOrDefault()?.Alias);
    }

    [Fact]
    public void parse_empty_skin_json()
    {
        var json = """
              {
              "id" : "uuid",
              "name" : "username",
              "skins" : [],
              "capes" : [],
              "profileActions" : { }
            }
            """;

        using var doc = JsonDocument.Parse(json);
        var profile = JEProfile.ParseFromJson(doc.RootElement);
        Assert.Equal("username", profile?.Username);
        Assert.Equal("uuid", profile?.UUID);
        Assert.Empty(profile?.Skins ?? []);
        Assert.Empty(profile?.Capes ?? []);
    }

    [Fact]
    public void parse_undefined_skin_json()
    {
        var json = """
              {
              "id" : "uuid",
              "name" : "username",
              "profileActions" : { }
            }
            """;

        using var doc = JsonDocument.Parse(json);
        var profile = JEProfile.ParseFromJson(doc.RootElement);
        Assert.Equal("username", profile?.Username);
        Assert.Equal("uuid", profile?.UUID);
        Assert.Empty(profile?.Skins ?? []);
        Assert.Empty(profile?.Capes ?? []);
    }

    [Fact]
    public void parse_broken_skin_json()
    {
        var json = """
              {
              "id" : "uuid",
              "name" : "username",
              "skins": "nope",
              "capes": "nope",
              "profileActions" : { }
            }
            """;

        using var doc = JsonDocument.Parse(json);
        var profile = JEProfile.ParseFromJson(doc.RootElement);
        Assert.Equal("username", profile?.Username);
        Assert.Equal("uuid", profile?.UUID);
        Assert.Empty(profile?.Skins ?? []);
        Assert.Empty(profile?.Capes ?? []);
    }
}

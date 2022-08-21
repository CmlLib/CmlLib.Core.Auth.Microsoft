using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Bedrock.Auth;
using System.Text.Json;
using XboxAuthNet.XboxLive;

class Program
{
    public static async Task Main()
    {
        var cid = "499c8d36-be2a-4231-9ebd-ef291b7bb64c";
        var app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(cid);

        var bedrockLoginHandler = new BedrockLoginHandlerBuilder(new HttpClient())
            .WithMsalOAuth(app, factory => factory.CreateInteractiveApi())
            .Build();

        BedrockSessionCache? result;
        try
        {
            result = await bedrockLoginHandler.LoginFromCache();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            result = await bedrockLoginHandler.LoginFromOAuth();
        }

        if (result?.BedrockTokens == null)
        {
            Console.WriteLine("null tokens");
            return;
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        foreach (var token in result.BedrockTokens)
        {
            var r = JsonSerializer.Serialize(token.DecodeTokenPayload(), options);
            Console.WriteLine(r);
        }
    }
}
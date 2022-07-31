using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using CmlLib.Core.Bedrock.Auth;
using System.Text.Json;

class Program
{
    public static async Task Main()
    {
        var cid = "499c8d36-be2a-4231-9ebd-ef291b7bb64c";

        var bedrockLoginHandler = LoginHandlerBuilder.Create()
            .SetClientId(cid)
            .SetHttpClient(new HttpClient())
            .ForBedrockEdition()
            .Build();

        var app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(cid);
        var msalClient = new MsalMinecraftLoginHandler<BedrockSessionCache>(app, bedrockLoginHandler);
        //await msalClient.RemoveAccounts();
        var result = await msalClient.LoginSilent();

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
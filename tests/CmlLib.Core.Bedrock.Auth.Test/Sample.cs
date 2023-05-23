using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using CmlLib.Core.Bedrock.Auth;

namespace CmlLib.Core.Bedrock.Auth.Test;

public class Sample
{
    private BELoginHandler buildLoginHandler()
    {
        return new BELoginHandler(
            HttpHelper.DefaultHttpClient.Value, 
            new InMemoryXboxGameAccountManager(XboxGameAccount.FromSessionStorage));
    }

    public async Task<BESession> Authenticate()
    {
        var loginHandler = buildLoginHandler();

        var result = await loginHandler.Authenticate();
        return result;
    }

    public async Task<BESession> AuthenticateInteractively()
    {
        var loginHandler = buildLoginHandler();

        var result = await loginHandler.AuthenticateInteractively()
            .ExecuteAsync();
        return result;
    }

    public async Task<BESession> AuthenticateSilently()
    {
        var loginHandler = buildLoginHandler();

        var result = await loginHandler.AuthenticateSilently()
            .ExecuteAsync();
        return result;
    }

}
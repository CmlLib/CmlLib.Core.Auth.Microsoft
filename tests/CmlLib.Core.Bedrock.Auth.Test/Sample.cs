using XboxAuthNet.Game;
using CmlLib.Core.Bedrock.Auth;

namespace CmlLib.Core.Bedrock.Auth.Test;

public class Sample
{
    public async Task<BESession> Authenticate()
    {
        var loginHandler = LoginHandlerBuilder.Create().ForBedrockEdition();

        var result = await loginHandler.Authenticate();
        return result;
    }

    public async Task<BESession> AuthenticateInteractively()
    {
        var loginHandler = LoginHandlerBuilder.Create().ForBedrockEdition();

        var result = await loginHandler.AuthenticateInteractively()
            .ExecuteAsync();
        return result;
    }

    public async Task<BESession> AuthenticateSilently()
    {
        var loginHandler = LoginHandlerBuilder.Create().ForBedrockEdition();

        var result = await loginHandler.AuthenticateSilently()
            .ExecuteAsync();
        return result;
    }

}
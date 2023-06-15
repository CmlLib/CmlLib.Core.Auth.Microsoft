using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using CmlLib.Core.Bedrock.Auth.Sessions;
using XboxAuthNet.Game.OAuth;
using Microsoft.Extensions.Logging.Abstractions;

namespace CmlLib.Core.Bedrock.Auth.Test;

public class Sample
{
    MicrosoftOAuthClientInfo clientInfo = new(
        "<MICROSOFT_OAUTH_SCOPES>", "<MICROSOFT_OAUTH_CLIENTID>");

    private XboxGameLoginHandler buildLoginHandler()
    {
        var parameters = new XboxGameLoginHandlerBuilder()
            .BuildParameters();
        return new XboxGameLoginHandler(parameters);
    }

    public async Task<BESession?> AuthenticateInteractively()
    {
        var loginHandler = buildLoginHandler();
        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddForceMicrosoftOAuth(clientInfo, oauth => oauth.Interactive());
        authenticator.AddForceXboxAuthForBE(xbox => xbox.Basic());
        authenticator.AddBEAuthenticator();

        var result = await authenticator.ExecuteAsync();
        var account = BEGameAccount.FromSessionStorage(result);
        return account.Session;
    }

    public async Task<BESession?> AuthenticateSilently()
    {
        var loginHandler = buildLoginHandler();
        var authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount();
        authenticator.AddForceMicrosoftOAuth(clientInfo, oauth => oauth.Silent());
        authenticator.AddForceXboxAuthForBE(xbox => xbox.Basic());
        authenticator.AddBEAuthenticator();

        var result = await authenticator.ExecuteAsync();
        var account = BEGameAccount.FromSessionStorage(result);
        return account.Session;
    }
}
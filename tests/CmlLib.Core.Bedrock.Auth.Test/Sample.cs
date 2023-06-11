using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using CmlLib.Core.Bedrock.Auth.Sessions;
using XboxAuthNet.Game.OAuth;

namespace CmlLib.Core.Bedrock.Auth.Test;

public class Sample
{
    MicrosoftOAuthClientInfo clientInfo = new()
    {
        Scopes = "<MICROSOFT_OAUTH_SCOPES>",
        ClientId = "<MICROSOFT_OAUTH_CLIENTID>"
    };

    private XboxGameLoginHandler buildLoginHandler()
    {
        return new XboxGameLoginHandler(
            HttpHelper.DefaultHttpClient.Value, 
            new InMemoryXboxGameAccountManager(XboxGameAccount.FromSessionStorage));
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
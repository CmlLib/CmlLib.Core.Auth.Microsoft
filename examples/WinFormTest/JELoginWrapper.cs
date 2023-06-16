using CmlLib.Core.Auth.Microsoft;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.Msal;

namespace WinFormTest;

public class JELoginWrapper
{

    private static JELoginWrapper? _instance;
    public static JELoginWrapper Instance => _instance ??= new JELoginWrapper();

    private JELoginWrapper() { }

    public JELoginHandler LoginHandler
    {
        get
        {
            if (loginHandler == null)
                initializeLoginHandler();
            return loginHandler!;
        }
    }

    public MicrosoftOAuthClientInfo GetOAuthClientInfo(string cid)
    {
        if (checkOAuthInitializingRequired(cid))
            initializeOAuthClient(cid);
        return oauthClient!;
    }

    public async Task<IPublicClientApplication> GetMsalAppAsync(string cid)
    {
        if (checkMsalInitializingRequired(cid))
            await initializeMsalApp(cid);
        return msalApp!;
    }

    private JELoginHandler? loginHandler;
    private MicrosoftOAuthClientInfo? oauthClient;
    private IPublicClientApplication? msalApp;

    private bool checkOAuthInitializingRequired(string cid)
    {
        return oauthClient?.ClientId != cid;
    }

    private void initializeOAuthClient(string cid)
    {
        oauthClient = new MicrosoftOAuthClientInfo(
            cid, JELoginHandler.DefaultMicrosoftOAuthClientInfo.Scopes);
    }

    private bool checkMsalInitializingRequired(string cid)
    {
        return msalApp?.AppConfig?.ClientId != cid;
    }

    private async Task initializeMsalApp(string cid)
    {
        msalApp = await MsalClientHelper.BuildApplicationWithCache(cid);
    }

    private void initializeLoginHandler()
    {
        loginHandler = JELoginHandlerBuilder.BuildDefault();
    }
}

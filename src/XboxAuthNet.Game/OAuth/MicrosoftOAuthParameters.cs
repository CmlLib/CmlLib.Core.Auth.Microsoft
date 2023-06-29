using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthParameters
{
    public MicrosoftOAuthParameters(
        MicrosoftOAuthClientInfo clientInfo, 
        ISessionSource<MicrosoftOAuthResponse> sessionSource, 
        ISessionSource<string> loginHintSource)
    {
        ClientInfo = clientInfo;
        SessionSource = sessionSource;
        LoginHintSource = loginHintSource;
    }

    public MicrosoftOAuthClientInfo ClientInfo { get; }
    public ISessionSource<MicrosoftOAuthResponse> SessionSource { get; }
    public ISessionSource<string> LoginHintSource { get; }
}

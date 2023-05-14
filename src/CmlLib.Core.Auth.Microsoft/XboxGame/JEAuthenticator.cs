using System;
using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.Game.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.XboxGame
{
    public class JEAuthenticator : IXboxGameAuthenticator<JESession>
    {
        private readonly JEAuthenticationApi _api;
        private readonly ISessionSource<JESession> _sessionSource;

        public bool CheckGameOwnership { get; set; } = false;

        public JEAuthenticator(HttpClient httpClient, ISessionSource<JESession> sessionSource)
        {
            this._api = new JEAuthenticationApi(httpClient);
            this._sessionSource = sessionSource;
        }

        public async Task<JESession> Authenticate(IXboxAuthStrategy xboxAuthStrategy)
        {
            var cachedSession = _sessionSource.Get();
            if (cachedSession == null)
                cachedSession = new JESession();
            
            var xboxTokens = await xboxAuthStrategy.Authenticate(JEAuthenticationApi.RelyingParty);

            if (string.IsNullOrEmpty(xboxTokens?.XstsToken?.UserHash) ||
                string.IsNullOrEmpty(xboxTokens?.XstsToken?.Token))
            {
                throw new MinecraftAuthException("Cannot auth with null UserHash and null Token");
            }

            var mojangToken = await _api.LoginWithXbox(xboxTokens.XstsToken.UserHash, xboxTokens.XstsToken.Token);

            if (cachedSession.Profile == null || 
                string.IsNullOrEmpty(cachedSession.Profile.Username) || 
                string.IsNullOrEmpty(cachedSession.Profile.UUID))
            {
                cachedSession.Profile = await getProfile(mojangToken);
            }

            cachedSession.Token = mojangToken;
            return cachedSession;
        }

        private async Task<JEProfile> getProfile(JEAuthenticationToken token)
        {
            if (string.IsNullOrEmpty(token?.AccessToken))
                throw new MinecraftAuthException("Null access token");

            if (CheckGameOwnership)
            {
                var own = await _api.CheckGameOwnership(token.AccessToken);
                if (!own)
                    throw new MinecraftAuthException("User does not own the game");
            }

            var profile = await _api.GetProfileUsingToken(token.AccessToken);

            if (string.IsNullOrEmpty(profile.UUID))
                throw new MinecraftAuthException("No uuid");
            if (string.IsNullOrEmpty(profile.Username)) 
                throw new MinecraftAuthException("No username");

            return profile;
        }
    }
}
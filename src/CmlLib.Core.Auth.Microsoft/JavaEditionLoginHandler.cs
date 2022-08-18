using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JavaEditionLoginHandler : AbstractLoginHandler<JavaEditionSessionCache>
    {
        private readonly IXboxLiveApi _xboxLiveApi;
        private readonly IMojangXboxApi _mojangXboxApi;

        public string RelyingParty { get; set; }
        public bool CheckGameOwnership { get; set; } = false;

        public JavaEditionLoginHandler(
            IMicrosoftOAuthApi oauthApi,
            IXboxLiveApi xboxLiveApi, 
            ICacheManager<JavaEditionSessionCache>? cacheManager, 
            IMojangXboxApi mojangXboxApi, 
            string relyingParty) :
            base(oauthApi, cacheManager)
        {
            this._xboxLiveApi = xboxLiveApi;
            this._mojangXboxApi = mojangXboxApi;
            this.RelyingParty = relyingParty;
        }

        public override async Task<JavaEditionSessionCache> LoginFromCache(JavaEditionSessionCache? sessionCache)
        {
            sessionCache = await base.LoginFromCache(sessionCache);

            // always create new GameSession
            sessionCache.GameSession = await GetMSession(sessionCache.MojangXboxToken!, sessionCache.GameSession);

            return sessionCache;
        }

        public override async Task<JavaEditionSessionCache> GetAllTokens(MicrosoftOAuthResponse msToken)
        {
            if (string.IsNullOrEmpty(msToken.AccessToken))
                throw new Exception("msToken.AccessToken was empty");

            var xsts = await _xboxLiveApi.GetXSTS(msToken.AccessToken!, null, null, this.RelyingParty);
            if (string.IsNullOrEmpty(xsts.Token))
                throw new XboxAuthException("xsts was empty", 200);
            if (string.IsNullOrEmpty(xsts.UserHash))
                throw new XboxAuthException("uhs was empty", 200);

            var session = await LoginFromXsts(xsts);
            session.MicrosoftOAuthToken = msToken;
            return session;
        }

        public async Task<JavaEditionSessionCache> LoginFromXsts(XboxAuthResponse xsts)
        {
            var mojangToken = await GetMojangXboxToken(xsts);
            var msession = await GetMSession(mojangToken, new MSession());

            return new JavaEditionSessionCache
            {
                XstsToken = xsts,
                MojangXboxToken = mojangToken,
                GameSession = msession
            };
        }

        private async Task<MojangXboxLoginResponse> GetMojangXboxToken(XboxAuthResponse xsts)
        {
            if (string.IsNullOrEmpty(xsts.UserHash))
                throw new ArgumentException("xsts.UserHash was null");
            if (string.IsNullOrEmpty(xsts.Token))
                throw new ArgumentException("xsts.Token was null");

            var mcToken = await _mojangXboxApi.LoginWithXbox(xsts.UserHash!, xsts.Token!); // not null
            return mcToken;
        }

        private async Task<MSession> GetMSession(MojangXboxLoginResponse mcToken, MSession? cachedSession)
        {
            if (cachedSession == null)
                cachedSession = new MSession();

            // update Username, UUID
            if (string.IsNullOrEmpty(cachedSession.Username) ||
                string.IsNullOrEmpty(cachedSession.UUID))
            {
                cachedSession = await CreateMinecraftSession(mcToken);
            }

            // update XUID (for CmlLib.Core 3.4.0)
            //if (string.IsNullOrEmpty(sessionCache.GameSession.UserXUID))
            //{
            //    var payload = mcToken.DecodeAccesTokenPayload();
            //    sessionCache.GameSession.UserXUID = payload.Xuid;
            //}

            // update AccessToken
            cachedSession.AccessToken = mcToken.AccessToken;
            return cachedSession;
        }

        private async Task<MSession> CreateMinecraftSession(MojangXboxLoginResponse mcToken)
        {
            if (mcToken == null)
                throw new ArgumentNullException(nameof(mcToken));
            if (mcToken.AccessToken == null)
                throw new ArgumentNullException(nameof(mcToken.AccessToken));

            if (CheckGameOwnership && !await _mojangXboxApi.CheckGameOwnership(mcToken.AccessToken))
                throw new MinecraftAuthException("mojang_nogame");

            MSession session;
            try
            {
                // throw 404 exception if profile is not exists
                session = await _mojangXboxApi.GetProfileUsingToken(mcToken.AccessToken);

                if (!session.CheckIsValid())
                    throw new MinecraftAuthException("mojang_noprofile");

                return session;
            }
            catch (MinecraftAuthException ex)
            {
                throw new MinecraftAuthException("mojang_noprofile", ex);
            }
        }
    }
}

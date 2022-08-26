using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JavaEditionLoginHandler : AbstractLoginHandler<JavaEditionSessionCache>
    {
        public static JavaEditionLoginHandler CreateWith(Action<JavaEditionLoginHandlerBuilder, LoginBuilderContext> action)
        {
            return new JavaEditionLoginHandlerBuilder()
                .With(action)
                .Build();
        }

        private readonly IMojangXboxApi _mojangXboxApi;

        public bool CheckGameOwnership { get; set; } = false;

        public JavaEditionLoginHandler(
            LoginHandlerParameters parameters,
            IMojangXboxApi mojangApi,
            ICacheManager<JavaEditionSessionCache> cacheManager) :
            base(parameters, cacheManager)
        {
            this._mojangXboxApi = mojangApi;
        }

        public override async Task<JavaEditionSessionCache> LoginFromCache(JavaEditionSessionCache? sessionCache, CancellationToken cancellationToken = default)
        {
            sessionCache = await base.LoginFromCache(sessionCache, cancellationToken);

            // always create new GameSession
            sessionCache.GameSession = await GetMSession(sessionCache.MojangXboxToken!, sessionCache.GameSession);

            return sessionCache;
        }

        protected override async Task<JavaEditionSessionCache> GetAllTokens(XboxAuthResponse xsts, CancellationToken cancellationToken = default)
        {
            var mojangToken = await GetMojangXboxToken(xsts);
            var msession = await GetMSession(mojangToken, new MSession());

            return new JavaEditionSessionCache
            {
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
            }
            catch (MinecraftAuthException ex)
            {
                throw new MinecraftAuthException("mojang_noprofile", ex);
            }

            if (!session.CheckIsValid())
                throw new MinecraftAuthException("mojang_noprofile");

            return session;
        }
    }
}

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using Microsoft.Extensions.Logging;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandler
    {
        public static readonly string DefaultClientId = "00000000402B5328";
        private static readonly Lazy<HttpClient> defaultHttpClient
            = new Lazy<HttpClient>(() => new HttpClient());

        public static ICacheManager<SessionCache> CreateDefaultCacheManager()
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
            return new JsonFileCacheManager<SessionCache>(defaultPath);
        }

        private readonly ILoggerFactory? loggerFactory;
        private readonly ILogger<LoginHandler>? logger;
        private readonly HttpClient httpClient;
        private readonly MojangXboxApi mojangXboxApi;
        private readonly ICacheManager<SessionCache>? cacheManager;

        public string RelyingParty { get; set; } = MojangXboxApi.RelyingParty;

        public LoginHandler() :
            this(new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope, defaultHttpClient.Value))
        {

        }

        public LoginHandler(MicrosoftOAuth oAuth)
        {
            this.cacheManager = CreateDefaultCacheManager();
            this.OAuth = oAuth;
            this.httpClient = defaultHttpClient.Value;
            this.mojangXboxApi = new MojangXboxApi(httpClient);
        }

        public LoginHandler(ICacheManager<SessionCache>? cacheManager, ILoggerFactory? logFactory = null)
        {
            this.loggerFactory = logFactory;
            this.logger = logFactory?.CreateLogger<LoginHandler>();
            this.cacheManager = cacheManager;
            this.httpClient = defaultHttpClient.Value;
            this.OAuth = new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope, httpClient, logFactory);
            this.mojangXboxApi = new MojangXboxApi(httpClient, logFactory);
        }

        public LoginHandler(MicrosoftOAuth oAuth, ICacheManager<SessionCache>? cacheManager, HttpClient client, ILoggerFactory? logFactory = null)
        {
            this.loggerFactory = logFactory;
            this.logger = logFactory?.CreateLogger<LoginHandler>();
            this.cacheManager = cacheManager;
            this.OAuth = oAuth;
            this.httpClient = client;
            this.mojangXboxApi = new MojangXboxApi(httpClient, logFactory);
        }

        public bool CheckGameOwnership { get; set; } = false;
        public MicrosoftOAuth OAuth { get; }
        public MicrosoftOAuthAuthCode? AuthCode { get; private set; }
        private SessionCache? sessionCache;

        private void readSessionCache()
        {
            sessionCache = cacheManager?.ReadCache();
        }

        private void saveSessionCache()
        {
            cacheManager?.SaveCache(sessionCache ?? new SessionCache());
        }

        public void ClearCache()
        {
            if (sessionCache != null)
            {
                sessionCache.MojangSession = null;
                sessionCache.XboxAuthSession = null;
                sessionCache.GameSession = null;
                sessionCache.MicrosoftOAuthSession = null;
            }

            saveSessionCache();
        }

        public virtual async Task<MSession?> LoginFromCache()
        {
            readSessionCache();

            var mcToken = sessionCache?.MojangSession;
            var msToken = sessionCache?.MicrosoftOAuthSession;
            var xboxToken = sessionCache?.XboxAuthSession;

            if (mcToken == null || DateTime.Now > mcToken.ExpiresOn) // invalid mc session
            {
                logger?.LogTrace("MojangSession was expired");

                if (string.IsNullOrEmpty(msToken?.RefreshToken)) // failed to refresh ms
                    return null;

                logger?.LogTrace("Try to refresh OAuth token");

                var refreshResult = await OAuth.RefreshToken(msToken?.RefreshToken);
                if (!refreshResult.IsSuccess)
                    return null;

                logger?.LogTrace("Try to refresh Game token");

                // success to refresh ms
                var xsts = await LoginXbox(msToken);
                mcToken = await LoginMinecraft(xsts);
            } 

            return await getGameSession(msToken, xboxToken, mcToken);
        }

        public virtual string CreateOAuthUrl()
        {
            return OAuth.CreateUrlForOAuth();
        }

        public virtual bool CheckOAuthLoginSuccess(string url)
        {
            var result = OAuth.CheckLoginSuccess(url, out var authCode);
            this.AuthCode = authCode;
            return result;
        }

        public Task<MSession> LoginFromOAuth()
        {
            if (this.AuthCode == null)
                throw new InvalidOperationException("authCode was null");
            return LoginFromOAuth(this.AuthCode);
        }

        public async Task<MSession> LoginFromOAuth(MicrosoftOAuthAuthCode oauthCode)
        {
            var msToken = await OAuth.GetTokens(oauthCode);
            if (msToken == null || !msToken.IsSuccess)
                throw new MicrosoftOAuthException("mslogin_fail", msToken?.Error, msToken?.ErrorDescription, msToken?.ErrorCodes);

            return await LoginFromOAuth(msToken);
        }

        public virtual async Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var xboxToken = await LoginXbox(msToken);
            var mcToken = await LoginMinecraft(xboxToken);
            return await getGameSession(msToken, xboxToken, mcToken);
        }

        private async Task<MSession> getGameSession(MicrosoftOAuthResponse? msToken, XboxAuthResponse? xboxToken, MojangXboxLoginResponse mcToken)
        {
            if (sessionCache == null)
                sessionCache = new SessionCache();
            
            sessionCache.GameSession ??= await CreateMinecraftSession(mcToken);
            sessionCache.GameSession.AccessToken = mcToken.AccessToken;

            //if (!string.IsNullOrEmpty(xboxToken?.UserXUID)) // XUID is not necessary
            //    sessionCache.GameSession.XUID = xboxToken.UserXUID;

            sessionCache.XboxAuthSession = xboxToken;
            sessionCache.MojangSession = mcToken;
            sessionCache.MicrosoftOAuthSession = msToken;

            saveSessionCache();
            return sessionCache.GameSession;
        }

        public virtual async Task<XboxAuthResponse> LoginXbox(MicrosoftOAuthResponse? msToken)
        {
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (!msToken.IsSuccess)
                throw new ArgumentException("msToken was failed");
            if (msToken.AccessToken == null)
                throw new ArgumentNullException(nameof(msToken.AccessToken));

            var xbox = new XboxAuth(httpClient, loggerFactory);
            var rps = await xbox.ExchangeRpsTicketForUserToken(msToken.AccessToken);

            if (!rps.IsSuccess || string.IsNullOrEmpty(rps.Token))
                throw new XboxAuthException("ExchangeRpsTicketForUserToken", rps.Error, rps.Message);
            
            var xsts = await xbox.ExchangeTokensForXstsIdentity(
                rps.Token, // not null 
                null, 
                null,
                RelyingParty, 
                null);

            if (!xsts.IsSuccess || string.IsNullOrEmpty(xsts.UserHash) || string.IsNullOrEmpty(xsts.Token))
            {
                throw CreateXboxException(xsts);
            }

            return xsts;
        }

        protected Exception CreateXboxException(XboxAuthResponse xsts)
        {
            logger?.LogError("XboxAuthException Error: {Error}, Message: {ErrorCode}, Redirect: {Redirect}",
                xsts.Error, xsts.Message, xsts.Redirect);

            string msg = "";
            if (xsts.Error == XboxAuthResponse.ChildError || xsts.Error == "2148916236")
                msg = "xbox_error_child";
            else if (xsts.Error == XboxAuthResponse.NoXboxAccountError)
                msg = "xbox_error_noaccount";
            else if (string.IsNullOrEmpty(xsts.UserHash))
                msg = "empty_userhash";
            else if (string.IsNullOrEmpty(xsts.Token))
                msg = "empty_token";

            string errorCode;
            try
            {
                var errorCodeStr = xsts.Error?.Trim();
                if (string.IsNullOrEmpty(errorCodeStr))
                {
                    errorCode = "no_error_msg";
                }
                else
                {
                    var errorInt = long.Parse(errorCodeStr);
                    errorCode = errorInt.ToString("x");
                }
            }
            catch
            {
                errorCode = xsts.Error ?? "no_error_msg";
            }

            if (string.IsNullOrEmpty(msg))
                msg = errorCode;

            return new XboxAuthException(msg, errorCode, xsts.Message ?? "no_error_msg");
        }

        public virtual async Task<MojangXboxLoginResponse> LoginMinecraft(XboxAuthResponse xsts)
        {
            if (string.IsNullOrEmpty(xsts.UserHash))
                throw new ArgumentException("xsts.UserHash was null");
            if (string.IsNullOrEmpty(xsts.Token))
                throw new ArgumentException("xsts.Token was null");

            var mcToken = await mojangXboxApi.LoginWithXbox(xsts.UserHash, xsts.Token); // not null
            return mcToken;
        }

        public virtual async Task<MSession> CreateMinecraftSession(MojangXboxLoginResponse xboxToken)
        {
            // 6. get minecraft profile (username, uuid)

            if (xboxToken == null)
                throw new ArgumentNullException(nameof(xboxToken));
            if (xboxToken.AccessToken == null)
                throw new ArgumentNullException(nameof(xboxToken.AccessToken));

            if (CheckGameOwnership && !await mojangXboxApi.CheckGameOwnership(xboxToken.AccessToken))
                throw new MinecraftAuthException("mojang_nogame");

            MSession session;
            try
            {
                // throw 404 exception if profile is not exists
                session = await mojangXboxApi.GetProfileUsingToken(xboxToken.AccessToken);

                if (!session.CheckIsValid())
                    throw new MinecraftAuthException("mojang_noprofile");

                return session;
            }
            catch (System.Net.WebException ex)
            {
                throw new MinecraftAuthException("mojang_noprofile", ex);
            }
        }
    }
}

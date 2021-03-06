using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.IO;
using System.Net.Http;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public sealed class LoginHandlerBuilder
    {
        public static readonly string DefaultClientId = "00000000402B5328";
        private static readonly Lazy<HttpClient> defaultHttpClient
            = new Lazy<HttpClient>(() => new HttpClient());

        internal LoginHandlerBuilder() 
        {
            this.HttpClient = new Lazy<HttpClient>(() => defaultHttpClient.Value);
            this.XboxLiveApi = new Lazy<IXboxLiveApi>(() => new XboxAuthNetApi(
                new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope, HttpClient.Value),
                new XboxAuth(this.HttpClient.Value)));
            this.MojangXboxApi = new Lazy<IMojangXboxApi>(() => new MojangXboxApi(
                this.HttpClient.Value));
            this.CacheManager = new Lazy<ICacheManager<SessionCache>>(() =>
            {
                var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
                return new JsonFileCacheManager<SessionCache>(defaultPath);
            });
        }

        internal Lazy<HttpClient> HttpClient;
        internal Lazy<IXboxLiveApi> XboxLiveApi;
        internal Lazy<IMojangXboxApi> MojangXboxApi;
        internal Lazy<ICacheManager<SessionCache>> CacheManager;

        public LoginHandlerBuilder SetHttpClient(HttpClient client)
        {
            this.HttpClient = new Lazy<HttpClient>(() => client);
            return this;
        }

        public LoginHandlerBuilder SetMicrosoftOAuthHandler(string id, string scope)
        {
            this.XboxLiveApi = new Lazy<IXboxLiveApi>(() => new XboxAuthNetApi(
                new MicrosoftOAuth(id, scope, this.HttpClient.Value),
                new XboxAuth(HttpClient.Value)));
            return this;
        }

        public LoginHandlerBuilder SetMicrosoftOAuthHandler(MicrosoftOAuth oAuth)
        {
            this.XboxLiveApi = new Lazy<IXboxLiveApi>(() => new XboxAuthNetApi(
                oAuth, 
                new XboxAuth(HttpClient.Value)));
            return this;
        }

        public LoginHandlerBuilder SetXboxLiveApi(IXboxLiveApi xboxApi)
        {
            this.XboxLiveApi = new Lazy<IXboxLiveApi>(() => xboxApi);
            return this;
        }

        public LoginHandlerBuilder SetMojangXboxApi(IMojangXboxApi mojangApi)
        {
            this.MojangXboxApi = new Lazy<IMojangXboxApi>(() => mojangApi);
            return this;
        }

        public LoginHandlerBuilder SetCacheManager(ICacheManager<SessionCache> cacheManager)
        {
            this.CacheManager = new Lazy<ICacheManager<SessionCache>>(() => cacheManager);
            return this;
        }

        public LoginHandlerBuilder SetJsonCacheManager(string path)
        {
            return SetCacheManager(new JsonFileCacheManager<SessionCache>(path));
        }
    }
}

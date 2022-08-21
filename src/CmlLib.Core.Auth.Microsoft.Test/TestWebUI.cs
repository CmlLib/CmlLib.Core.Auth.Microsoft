using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.Test.Mock;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    internal class TestWebUI
    {
        public JavaEditionLoginHandler CreateLoginHandler(Uri redirectedUri)
        {
            var loginHandler = new JavaEditionLoginHandlerBuilder()
                .WithCacheManager(new InMemoryCacheManger<JavaEditionSessionCache>())
                .WithMicrosoftOAuthApi(builder => builder
                    .WithWebUI(new MockWebUI(redirectedUri)))
                .WithXboxLiveApi(new MockXboxLiveApi())
                .WithMojangXboxApi(new MockMojangXboxApi())
                .Build();

            return loginHandler;
        }

        [Test]
        public async Task TestSuccess(string uri)
        {
            var loginHandler = CreateLoginHandler(new Uri(uri));
            await loginHandler.LoginFromOAuth();
        }

        [Test]
        public async Task TestError(string uri)
        {

        }
    }
}

using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    [TestFixture]
    public class TestMicrosoftOAuthStrategyBuilder
    {
        private MicrosoftOAuthClientInfo clientInfo = new MicrosoftOAuthClientInfo
        {
            ClientId = "test_clientid",
            Scopes = "test_scopes"
        };

        private MicrosoftOAuthStrategyBuilder<object> createBuilder()
        {
            var builder = new MicrosoftOAuthStrategyBuilder<object>(this, clientInfo, new HttpClient());
            return builder;
        }

        [Test]
        [Platform("Win")]
        public void TestInteractiveStrategy()
        {
            var builder = createBuilder();
            builder.WithCaching(false);
            builder.UseInteractiveStrategy();
            var strategy = builder.Build();

            Assert.IsInstanceOf<InteractiveMicrosoftOAuthStrategy>(strategy);
        }

        [Test]
        public void TestSilentStrategy()
        {
            var builder = createBuilder();
            builder.WithCaching(false);
            builder.UseSilentStrategy();
            var strategy = builder.Build();

            Assert.IsInstanceOf<SilentMicrosoftOAuthStrategy>(strategy);
        }

        [Test]
        public void TestCachingStrategy()
        {
            var builder = createBuilder();
            builder.WithCaching(true);
            builder.UseSilentStrategy();
            var strategy = builder.Build();

            Assert.IsInstanceOf<CachingMicrosoftOAuthStrategy>(strategy);
        }

        [Test]
        public async Task TestStrategyWithCaching()
        {
            var sessionSource = new InMemorySessionSource<MicrosoftOAuthResponse>();
            var mockResponse = new MicrosoftOAuthResponse();

            var builder = createBuilder();
            builder.WithCaching(true);
            builder.WithSessionSource(sessionSource);
            builder.FromMicrosoftOAuthResponse(mockResponse);

            var strategy = builder.Build();
            var actualResponse = await strategy.Authenticate();
            var cachedResponse = await sessionSource.GetAsync();

            Assert.IsInstanceOf<CachingMicrosoftOAuthStrategy>(strategy);
            Assert.AreEqual(mockResponse, actualResponse);
            Assert.AreEqual(mockResponse, cachedResponse);
        }
    }
}
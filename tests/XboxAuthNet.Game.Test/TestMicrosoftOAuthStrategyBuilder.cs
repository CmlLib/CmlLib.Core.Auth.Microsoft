using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Test
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
            var cachedResponse = sessionSource.Get();

            Assert.IsInstanceOf<CachingMicrosoftOAuthStrategy>(strategy);
            Assert.AreEqual(mockResponse, actualResponse);
            Assert.AreEqual(mockResponse, cachedResponse);
        }
    }
}
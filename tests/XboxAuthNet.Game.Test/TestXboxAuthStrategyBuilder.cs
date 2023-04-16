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
    public class TestXboxAuthStrategyBuilder
    {
        private XboxAuthStrategyBuilder<object> createBuilder()
        {
            var builder = new XboxAuthStrategyBuilder<object>(this, new HttpClient());
            return builder;
        }

        private IMicrosoftOAuthStrategy createMockOAuthStrategy()
        {
            return new MicrosoftOAuthStrategyFromResponse(new MicrosoftOAuthResponse());
        }

        public void TestBasicStrategy()
        {
            var builder = createBuilder();
            builder.WithMicrosoftOAuthStrategy(createMockOAuthStrategy());
            builder.WithCaching(false);
            builder.UseBasicStrategy();
            var strategy = builder.Build();

            Assert.IsInstanceOf<BasicXboxAuthStrategy>(strategy);
        }

        [Test]
        public void TestCachingStrategy()
        {
            var builder = createBuilder();
            builder.WithMicrosoftOAuthStrategy(createMockOAuthStrategy());
            builder.WithCaching(true);
            builder.UseBasicStrategy();
            var strategy = builder.Build();

            Assert.IsInstanceOf<CachingXboxAuthStrategy>(strategy);
        }

        [Test]
        public async Task TestStrategyWithCaching()
        {
            var sessionSource = new InMemorySessionSource<XboxAuthTokens>();
            var mockTokens = new XboxAuthTokens();

            var builder = createBuilder();
            builder.WithCaching(true);
            builder.WithSessionSource(sessionSource);
            builder.FromXboxAuthTokens(mockTokens);

            var strategy = builder.Build();
            var actualResponse = await strategy.Authenticate("relyingParty");
            var cachedResponse = sessionSource.Get();

            Assert.IsInstanceOf<CachingXboxAuthStrategy>(strategy);
            Assert.AreEqual(mockTokens, actualResponse);
            Assert.AreEqual(mockTokens, cachedResponse);
        }
    }
}
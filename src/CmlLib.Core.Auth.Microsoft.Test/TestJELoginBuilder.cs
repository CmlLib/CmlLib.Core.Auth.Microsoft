using NUnit.Framework;
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
    public class TestJELoginBuilder
    {
        // [Setup] will initialize fields
        JELoginHandler loginHandler = null!;
        ISessionStorage sessionStorage = null!;

        [SetUp]
        public void Setup()
        {
            sessionStorage = new InMemorySessionStorage();
            loginHandler = LoginHandlerBuilder.Create()
                .WithSessionStorage(sessionStorage)
                .ForJavaEdition();
        }

        [Test]
        public void TestDefaultParametersOfInteractiveAuth()
        {
            var builder = loginHandler.AuthenticateInteractively();
            Assert.NotNull(builder.HttpClient, "Default HttpClient should be not null");
            Assert.NotNull(builder.SessionStorage, "Default SessionStorage should be not null");

            Assert.IsTrue(builder.UseCaching);
            Assert.IsTrue(builder.MicrosoftOAuth.UseCaching);
            Assert.IsTrue(builder.XboxAuth.UseCaching);
        }

        [Test]
        public async Task TestSessionStorage()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var builder = loginHandler.AuthenticateSilently()
                .WithSessionStorage(newSessionStorage)
                .MicrosoftOAuth.UseSilentStrategy();

            builder.PreExecute();

            await AssertSessionSourceIsFromSessionStorage(builder, newSessionStorage);
        }

        [Test]
        public async Task TestSessionStorageReverse()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var builder = loginHandler.AuthenticateSilently()
                .MicrosoftOAuth.UseSilentStrategy()
                .WithSessionStorage(newSessionStorage);

            builder.PreExecute();

            await AssertSessionSourceIsFromSessionStorage(builder, newSessionStorage);
        }

        private async Task AssertSessionSourceIsFromSessionStorage(
            JEAuthenticationBuilder builder, ISessionStorage newSessionStorage)
        {
            Assert.AreEqual(newSessionStorage, builder.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                new MicrosoftOAuthSessionSource(newSessionStorage),
                builder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                new XboxSessionSource(newSessionStorage),
                builder.XboxAuth.SessionSource);
        }

        [Test]
        public async Task TestSessionSource()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var newOAuthSessionSource = new InMemorySessionSource<MicrosoftOAuthResponse>();
            var newXboxSessionSource = new InMemorySessionSource<XboxAuthTokens>();

            var builder = loginHandler.AuthenticateSilently()
                .WithSessionStorage(newSessionStorage)
                .MicrosoftOAuth.WithSessionSource(newOAuthSessionSource)
                .XboxAuth.WithSessionSource(newXboxSessionSource);

            builder.PreExecute();

            Assert.AreEqual(newSessionStorage, builder.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                builder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                builder.XboxAuth.SessionSource);
        }

        [Test]
        public async Task TestSessionSourceReverse()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var newOAuthSessionSource = new InMemorySessionSource<MicrosoftOAuthResponse>();
            var newXboxSessionSource = new InMemorySessionSource<XboxAuthTokens>();

            var builder = loginHandler.AuthenticateSilently()
                .MicrosoftOAuth.WithSessionSource(newOAuthSessionSource)
                .XboxAuth.WithSessionSource(newXboxSessionSource)
                .WithSessionStorage(newSessionStorage);

            builder.PreExecute();

            Assert.AreEqual(newSessionStorage, builder.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                builder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                builder.XboxAuth.SessionSource);
        }

        private async Task AssertAreEqualOAuthSessionSource(ISessionSource<MicrosoftOAuthResponse>? expected, ISessionSource<MicrosoftOAuthResponse>? actual)
        {
            var mock = new MicrosoftOAuthResponse();
            await AssertAreEqualSessionSource(mock, expected, actual);
        }

        private async Task AssertAreEqualXboxSessionSource(ISessionSource<XboxAuthTokens>? expected, ISessionSource<XboxAuthTokens>? actual)
        {
            var mock = new XboxAuthTokens();
            await AssertAreEqualSessionSource(mock, expected, actual);
        }

        private async Task AssertAreEqualSessionSource<T>(T mock, ISessionSource<T>? expected, ISessionSource<T>? actual) where T : class
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);

            await expected!.SetAsync(mock);
            var cached = await expected.GetAsync();

            Assert.AreEqual(mock, cached);
        }
    }
}
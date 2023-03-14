using NUnit.Framework;
using System.Threading.Tasks;
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

            var microsoftBuilder = builder.WithMicrosoftOAuth();
            Assert.IsTrue(microsoftBuilder.MicrosoftOAuth.UseCaching);
            Assert.IsTrue(microsoftBuilder.XboxAuth.UseCaching);
        }

        [Test]
        public async Task TestSessionStorage()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var builder = loginHandler.AuthenticateSilently();
            builder.WithSessionStorage(newSessionStorage);
            var microsoftBuilder = builder.WithMicrosoftOAuth();
            microsoftBuilder.MicrosoftOAuth.UseSilentStrategy();
            microsoftBuilder.Build();

            builder.Build();
            await AssertSessionSourceIsFromSessionStorage(microsoftBuilder, newSessionStorage);
        }

        [Test]
        public async Task TestSessionStorageReverse()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var builder = loginHandler.AuthenticateSilently();

            var microsoftBuilder = builder.WithMicrosoftOAuth();
            microsoftBuilder.MicrosoftOAuth.UseSilentStrategy();
            microsoftBuilder.Build();

            builder.WithSessionStorage(newSessionStorage);
            builder.Build();

            await AssertSessionSourceIsFromSessionStorage(microsoftBuilder, newSessionStorage);
        }

        private async Task AssertSessionSourceIsFromSessionStorage<T>(
            MicrosoftXboxBuilder<T> builder, ISessionStorage newSessionStorage)
            where T : IBuilderWithXboxAuthStrategy
        {
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

            var builder = loginHandler.AuthenticateSilently();
            builder.WithSessionStorage(newSessionStorage);

            var microsoftBuilder = builder.WithMicrosoftOAuth();
            microsoftBuilder.MicrosoftOAuth.WithSessionSource(newOAuthSessionSource);
            microsoftBuilder.XboxAuth.WithSessionSource(newXboxSessionSource);
            microsoftBuilder.Build();

            builder.Build();

            Assert.AreEqual(newSessionStorage, builder.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                microsoftBuilder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                microsoftBuilder.XboxAuth.SessionSource);
        }

        [Test]
        public async Task TestSessionSourceReverse()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var newOAuthSessionSource = new InMemorySessionSource<MicrosoftOAuthResponse>();
            var newXboxSessionSource = new InMemorySessionSource<XboxAuthTokens>();

            var builder = loginHandler.AuthenticateSilently();
            var microsoftBuilder = builder.WithMicrosoftOAuth();
            microsoftBuilder.MicrosoftOAuth.WithSessionSource(newOAuthSessionSource);
            microsoftBuilder.XboxAuth.WithSessionSource(newXboxSessionSource);
            builder.WithSessionStorage(newSessionStorage);

            builder.Build();

            Assert.AreEqual(newSessionStorage, builder.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                microsoftBuilder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                microsoftBuilder.XboxAuth.SessionSource);
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
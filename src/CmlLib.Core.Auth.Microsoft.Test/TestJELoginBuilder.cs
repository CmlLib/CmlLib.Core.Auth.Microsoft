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

        [SetUp]
        public void Setup()
        {
            var sessionStorage = new InMemorySessionStorage();
            loginHandler = LoginHandlerBuilder.Create()
                .WithSessionStorage(sessionStorage)
                .ForJavaEdition();
        }

        [Test]
        public void TestDefaultParametersOfAuthenticateSilently()
        {
            var builder = loginHandler.AuthenticateSilently();
            builder.Build();

            Assert.NotNull(builder.HttpClient, "Default HttpClient should be not null");
            Assert.NotNull(builder.SessionStorage, "Default SessionStorage should be not null");
            Assert.IsTrue(builder.UseCaching);
        }

        [Test]
        public void TestDefaultParametersOfMicrosoftOAuth()
        {
            MicrosoftXboxBuilder? xboxBuilder = null;
            loginHandler.AuthenticateSilently()
                .WithMicrosoftOAuth(innerBuilder => 
                {
                    xboxBuilder = innerBuilder;
                })
                .Build();

            Assert.NotNull(xboxBuilder);
            Assert.IsTrue(xboxBuilder!.MicrosoftOAuth.UseCaching);
            Assert.IsInstanceOf<SessionFromStorage<MicrosoftOAuthResponse>>(xboxBuilder!.MicrosoftOAuth.SessionSource);
            Assert.IsTrue(xboxBuilder.XboxAuth.UseCaching);
            Assert.IsInstanceOf<SessionFromStorage<XboxAuthTokens>>(xboxBuilder.XboxAuth.SessionSource);
        }

        [Test]
        public async Task TestSessionStorage()
        {
            var newSessionStorage = new InMemorySessionStorage();
            MicrosoftXboxBuilder? xboxBuilder = null;

            loginHandler.AuthenticateSilently()
                .WithSessionStorage(newSessionStorage)
                .WithMicrosoftOAuth(builder => 
                {
                    xboxBuilder = builder;
                })
                .Build();

            Assert.NotNull(xboxBuilder); // assert that inner code block of WithMicrosoftOAuth is executed
            await AssertSessionSourceIsFromSessionStorage(xboxBuilder!, newSessionStorage);
        }

        [Test]
        public async Task TestSessionStorageReverse()
        {
            var newSessionStorage = new InMemorySessionStorage();
            MicrosoftXboxBuilder? xboxBuilder = null;

            loginHandler.AuthenticateSilently()
                .WithMicrosoftOAuth(builder => 
                {
                    xboxBuilder = builder;
                })
                .WithSessionStorage(newSessionStorage)
                .Build();

            Assert.NotNull(xboxBuilder); // assert that inner code block of WithMicrosoftOAuth is executed
            await AssertSessionSourceIsFromSessionStorage(xboxBuilder!, newSessionStorage);
        }

        private async Task AssertSessionSourceIsFromSessionStorage(
            MicrosoftXboxBuilder builder, ISessionStorage newSessionStorage)
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
            MicrosoftXboxBuilder? xboxBuilder = null;

            loginHandler.AuthenticateSilently()
                .WithSessionStorage(newSessionStorage)
                .WithMicrosoftOAuth(builder => 
                {
                    xboxBuilder = builder;
                    builder.MicrosoftOAuth.WithSessionSource(newOAuthSessionSource);
                    builder.XboxAuth.WithSessionSource(newXboxSessionSource);
                })
                .Build();

            Assert.NotNull(xboxBuilder);
            Assert.AreEqual(newSessionStorage, xboxBuilder!.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                xboxBuilder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                xboxBuilder.XboxAuth.SessionSource);
        }

        [Test]
        public async Task TestSessionSourceReverse()
        {
            var newSessionStorage = new InMemorySessionStorage();
            var newOAuthSessionSource = new InMemorySessionSource<MicrosoftOAuthResponse>();
            var newXboxSessionSource = new InMemorySessionSource<XboxAuthTokens>();
            MicrosoftXboxBuilder? xboxBuilder = null;

            loginHandler.AuthenticateSilently()
                .WithMicrosoftOAuth(builder => 
                {
                    xboxBuilder = builder;
                    builder.MicrosoftOAuth.WithSessionSource(newOAuthSessionSource);
                    builder.XboxAuth.WithSessionSource(newXboxSessionSource);
                })
                .WithSessionStorage(newSessionStorage)
                .Build();

            Assert.NotNull(xboxBuilder);
            Assert.AreEqual(newSessionStorage, xboxBuilder!.SessionStorage);
            await AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                xboxBuilder.MicrosoftOAuth.SessionSource);
            await AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                xboxBuilder.XboxAuth.SessionSource);
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
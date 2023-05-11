using NUnit.Framework;
using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.Game.XboxAuthStrategies;
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
            loginHandler = JELoginHandlerBuilder.BuildDefault();
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
        public void TestSessionStorage()
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
            AssertSessionSourceIsFromSessionStorage(xboxBuilder!, newSessionStorage);
        }

        [Test]
        public void TestSessionStorageReverse()
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
            AssertSessionSourceIsFromSessionStorage(xboxBuilder!, newSessionStorage);
        }

        private void AssertSessionSourceIsFromSessionStorage(
            MicrosoftXboxBuilder builder, ISessionStorage newSessionStorage)
        {
            AssertAreEqualOAuthSessionSource(
                new MicrosoftOAuthSessionSource(newSessionStorage),
                builder.MicrosoftOAuth.SessionSource);
            AssertAreEqualXboxSessionSource(
                new XboxSessionSource(newSessionStorage),
                builder.XboxAuth.SessionSource);
        }

        [Test]
        public void TestSessionSource()
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
            Assert.That(xboxBuilder!.SessionStorage, Is.EqualTo(newSessionStorage));
            AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                xboxBuilder.MicrosoftOAuth.SessionSource);
            AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                xboxBuilder.XboxAuth.SessionSource);
        }

        [Test]
        public void TestSessionSourceReverse()
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
            Assert.That(xboxBuilder!.SessionStorage, Is.EqualTo(newSessionStorage));
            AssertAreEqualOAuthSessionSource(
                newOAuthSessionSource,
                xboxBuilder.MicrosoftOAuth.SessionSource);
            AssertAreEqualXboxSessionSource(
                newXboxSessionSource, 
                xboxBuilder.XboxAuth.SessionSource);
        }

        private void AssertAreEqualOAuthSessionSource(ISessionSource<MicrosoftOAuthResponse>? expected, ISessionSource<MicrosoftOAuthResponse>? actual)
        {
            var mock = new MicrosoftOAuthResponse();
            AssertAreEqualSessionSource(mock, expected, actual);
        }

        private void AssertAreEqualXboxSessionSource(ISessionSource<XboxAuthTokens>? expected, ISessionSource<XboxAuthTokens>? actual)
        {
            var mock = new XboxAuthTokens();
            AssertAreEqualSessionSource(mock, expected, actual);
        }

        private void AssertAreEqualSessionSource<T>(T mock, ISessionSource<T>? expected, ISessionSource<T>? actual) where T : class
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);

            expected!.Set(mock);
            var cached = expected.Get();

            Assert.That(cached, Is.EqualTo(mock));
        }
    }
}
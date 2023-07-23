using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.OAuth.CodeFlow;

namespace XboxAuthNet.Game;

public static class Extensions
{
    public static void AddAuthenticatorWithoutValidator(
        this ICompositeAuthenticator self,
        IAuthenticator authenticator)
    {
        self.AddAuthenticator(StaticValidator.Invalid, authenticator);
    }

    public static void AddAuthenticatorCollection(
        this ICompositeAuthenticator self,
        ISessionValidator validator,
        Action<AuthenticatorCollection> collection)
    {
        var authenticator = new AuthenticatorCollection();
        collection.Invoke(new AuthenticatorCollection());
        self.AddAuthenticator(validator, authenticator);
    }

    public static void AddFallbackAuthenticator(
        this ICompositeAuthenticator self,
        ISessionValidator validator,
        Action<FallbackAuthenticator> fallback)
    {
        var authenticator = new FallbackAuthenticator();
        fallback.Invoke(authenticator);
        self.AddAuthenticator(validator, authenticator);
    }

    public static void AddMicrosoftOAuth(
        this ICompositeAuthenticator self,
        MicrosoftOAuthClientInfo clientInfo,
        Func<MicrosoftOAuthBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new MicrosoftOAuthBuilder(clientInfo);
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticator(builder.Validator(), authenticator);
    }

    public static void AddForceMicrosoftOAuth(
        this ICompositeAuthenticator self,
        MicrosoftOAuthClientInfo clientInfo,
        Func<MicrosoftOAuthBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new MicrosoftOAuthBuilder(clientInfo);
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticatorWithoutValidator(authenticator);
    }

    public static void AddMicrosoftOAuthSignout(
        this ICompositeAuthenticator self,
        MicrosoftOAuthClientInfo clientInfo)
    {
        var builder = new MicrosoftOAuthBuilder(clientInfo);
        var signout = builder.Signout();
        self.AddAuthenticatorWithoutValidator(signout);
    }

    public static void AddMicrosoftOAuthBrowserSignout(
        this ICompositeAuthenticator self,
        MicrosoftOAuthClientInfo clientInfo) =>
        self.AddMicrosoftOAuthBrowserSignout(clientInfo, _ => { });

    public static void AddMicrosoftOAuthBrowserSignout(
        this ICompositeAuthenticator self,
        MicrosoftOAuthClientInfo clientInfo,
        Action<CodeFlowBuilder> builderInvoker)
    {
        var builder = new MicrosoftOAuthBuilder(clientInfo);
        var signout = builder.SignoutWithBrowser(builderInvoker);
        self.AddAuthenticatorWithoutValidator(signout);
    }

    public static void AddXboxAuth(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new XboxAuthBuilder();
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticator(builder.Validator(), authenticator);
    }

    public static void AddForceXboxAuth(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new XboxAuthBuilder();
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticatorWithoutValidator(authenticator);
    }

    public static void AddXboxAuthSignout(this ICompositeAuthenticator self)
    {
        self.AddSessionCleaner(XboxSessionSource.Default);
    }

    public static void AddSessionCleaner<T>(
        this ICompositeAuthenticator self,
        ISessionSource<T> sessionSource)
    {
        self.AddAuthenticatorWithoutValidator(new SessionCleaner<T>(sessionSource));
    }
} 
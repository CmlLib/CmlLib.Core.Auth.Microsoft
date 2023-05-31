using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;

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
        Func<AuthenticatorCollection, AuthenticatorCollection> collection)
    {
        var authenticator = collection.Invoke(new AuthenticatorCollection());
        self.AddAuthenticator(validator, authenticator);
    }

    public static void AddFallbackAuthenticator(
        this ICompositeAuthenticator self,
        ISessionValidator validator,
        Func<FallbackAuthenticator, FallbackAuthenticator> fallback)
    {
        var authenticator = fallback.Invoke(new FallbackAuthenticator());
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

    public static void AddSessionCleaner<T>(
        this ICompositeAuthenticator self,
        ISessionSource<T> sessionSource)
    {
        self.AddAuthenticatorWithoutValidator(new SessionCleaner<T>(sessionSource));
    }
} 
using XboxAuthNet.Game;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.Game.Authenticators;
using CmlLib.Core.Auth.Microsoft.Sessions;
using CmlLib.Core.Auth.Microsoft.Authenticators;
using XboxAuthNet.Game.Accounts;

namespace CmlLib.Core.Auth.Microsoft;

public static class Extensions
{
    public static void AddMicrosoftOAuthForJE(
        this ICompositeAuthenticator self,
        Func<MicrosoftOAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static void AddForceMicrosoftOAuthForJE(
        this ICompositeAuthenticator self,
        Func<MicrosoftOAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddForceMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static void AddXboxAuthForJE(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddXboxAuth(builder => 
        {
            builder.WithRelyingParty(JELoginHandler.RelyingParty);
            return builderInvoker(builder);
        });

    public static void AddForceXboxAuthForJE(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddForceXboxAuth(builder =>
        {
            builder.WithRelyingParty(JELoginHandler.RelyingParty);
            return builderInvoker(builder);
        });

    public static void AddJEAuthenticator(this ICompositeAuthenticator @this)
    {
        var builder = new JEAuthenticatorBuilder();
        @this.AddAuthenticator(builder.TokenValidator(), builder.TokenAuthenticator());
        @this.AddAuthenticator(StaticValidator.Invalid, builder.ProfileAuthenticator());
    }

    public static void AddJEAuthenticator(
        this ICompositeAuthenticator self,
        Func<JEAuthenticatorBuilder, IAuthenticator> authenticatorBuilder)
    {
        var builder = new JEAuthenticatorBuilder();
        var authenticator = authenticatorBuilder.Invoke(builder);
        self.AddAuthenticator(builder.TokenAndProfileValidator(), authenticator);
    }

    public static void AddForceJEAuthenticator(this ICompositeAuthenticator self) =>
        self.AddForceJEAuthenticator(builder => builder.Build());

    public static void AddForceJEAuthenticator(
        this ICompositeAuthenticator self,
        Func<JEAuthenticatorBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new JEAuthenticatorBuilder();
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticator(StaticValidator.Invalid, authenticator);
    }

    public static void AddJESignout(this ICompositeAuthenticator self)
    {
        var builder = new JEAuthenticatorBuilder();
        self.AddAuthenticatorWithoutValidator(builder.SessionCleaner());
    }

    public static JEGameAccount GetJEAccountByUsername(this XboxGameAccountCollection self, string username)
    {
        return (JEGameAccount)self.First(account =>
        {
            if (account is JEGameAccount jeAccount)
            {
                return jeAccount.Profile?.Username == username;
            }
            else
            {
                return false;
            }
        });
    }

    public static async Task<MSession> ExecuteForLauncherAsync(this NestedAuthenticator self)
    {
        var session = await self.ExecuteAsync();
        var account = JEGameAccount.FromSessionStorage(session);
        return account.ToLauncherSession();
    }
}
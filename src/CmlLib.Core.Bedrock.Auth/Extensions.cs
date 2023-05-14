using System;
using XboxAuthNet.Game.Builders;

namespace CmlLib.Core.Bedrock.Auth;

public static class Extensions
{
    public static XboxGameAuthenticationBuilder<BESession> WithInteractiveMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<BESession> self) =>
        self.WithInteractiveMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo);

    public static XboxGameAuthenticationBuilder<BESession> WithInteractiveMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<BESession> self,
        Action<MicrosoftXboxBuilder> builderInvoker) =>
        self.WithInteractiveMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static XboxGameAuthenticationBuilder<BESession> WithSilentMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<BESession> self) =>
        self.WithSilentMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo);

    public static XboxGameAuthenticationBuilder<BESession> WithSilentMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<BESession> self,
        Action<MicrosoftXboxBuilder> builderInvoker) =>
        self.WithSilentMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static XboxGameAuthenticationBuilder<BESession> WithMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<BESession> self,
        Action<MicrosoftXboxBuilder> builderInvoker) =>
        self.WithMicrosoftOAuth(BELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static XboxGameAuthenticationBuilder<BESession> WithBEAuthenticator(
        this XboxGameAuthenticationBuilder<BESession> self)
    {
        return self.WithGameAuthenticator(self => 
        {
            var authenticator = new BEAuthenticator(self.HttpClient);
            return authenticator;
        });
    }
}
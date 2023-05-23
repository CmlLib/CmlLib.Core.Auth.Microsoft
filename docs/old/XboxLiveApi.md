# XboxLiveApi

Log in to the xbox through the Microsoft OAuth token.  
There are several ways to get XSTS tokens.

## Basic XboxAuthNetApi

Default setting. Get XSTS tokens directly without any additional work.  
Users under the age of 18 may not be able to log in. (`8015dc0c`, `8015dc0d`, `8015dc0e` error)

## With Device token

```csharp
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
using CmlLib.Core.Auth.Microsoft.XboxLive;
```

```csharp
var loginHandler = new LoginHandler()
    .ForJavaEdition()
    .WithMicrosoftOAuthApi(builder => builder
        .WithWebUI(new WebView2WebUI()))
    .WithXboxAuthNetApi(builder => builder
        .WithDummyDeviceTokenApi())
    .Build()

// login with loginHandler
```

With the device token, Underage users can also log into xbox.

## SisuAuthFlow

```csharp
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
using CmlLib.Core.Auth.Microsoft.XboxLive;
```

```csharp
var loginHandler = new LoginHandler()
    .ForJavaEdition()
    .WithMicrosoftOAuthApi(builder => builder
        .WithWebUI(new WebView2WebUI()))
    .With((builder, context) =>
    {
        var keyGenerator = KeyPairGeneratorFactory.CreateDefaultAsymmetricKeyPair();
        builder.WithXboxSisuAuthApi(builder => builder
            .WithECKeyPairGenerator(keyGenerator)
            .WithWin32Device()
            .WithTokenPrefix(XboxSecureAuth.XboxTokenPrefix));
    })
    .Build()

// login with loginHandler
```

Most age-related issues can be resolved through sisu auth flow. However, this login flow does not accept Microsoft OAuth token from Azure app. **This means, you can't use MsalClient with SisuAuthApi.**


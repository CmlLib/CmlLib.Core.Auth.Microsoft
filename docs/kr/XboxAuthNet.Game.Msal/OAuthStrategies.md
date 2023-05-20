# OAuthStrategies

MSAL 을 통해 Microsoft OAuth 를 진행하는 방법을 제공합니다. 

먼저 [ClientID](./ClientID.md) 를 통해 [IPublicClientAppliction 를 초기화](./MsalClientHelper.md)해야 합니다.

For example:

```csharp
using XboxAuthNet.Game.Msal;

var app = await MsalClientHelper.BuildApplicationWithCache("<CLIENT-ID>");
```

## UseInteractiveStrategy

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.UseInteractiveStrategy(app)
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. 로그인 페이지를 어떻게 표시할 지는 MSAL 에서 결정합니다. 

## UseEmbeddedWebViewStrategy

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.UseEmbeddedWebViewStrategy(app)
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. WebView2 를 사용해 로그인 페이지를 표시합니다.

## UseSystemBrowserStrategy

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.UseSystemBrowserStrategy(app)
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. 시스템 기본 브라우저를 열어 로그인 페이지를 표시합니다. 

## UseSilentStrategy

```csharp
var session = await loginHandler.AuthenticateSilently()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.UseSilentStrategy(app)
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

MSAL 에 캐시된 계정 정보를 이용해 로그인을 시도합니다.  

## UseDeviceCodeStrategy

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.UseDeviceCodeStrategy(app, deviceCode => 
        {
            Console.WriteLine(deviceCode.Message);
            return Task.CompletedTask;
        })
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

DeviceCode 방식으로 로그인을 시도합니다. 

## FromParameterBuilder

```csharp
var parameterBuilder = app.AcquireTokenInteractive(MsalClientHelper.XboxScopes)
    .WithPrompt(Prompt.SelectAccount);

var session = await loginHandler.AuthenticateInteractively()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.FromParameterBuilder(parameterBuilder)
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

MSAL 에서 제공하는 Parameter Builder API 를 직접 이용해 Microsoft OAuth 방식을 결정할 수 있습니다. 

## FromAuthenticationResult

```csharp
var authenticationResult = await app.AcquireTokenInteractive(MsalClientHelper.XboxScopes)
    .WithPrompt(Prompt.SelectAccount)
    .ExecuteAsync();
var session = await loginHandler.AuthenticateInteractively()
    .WithMsalOAuth(builder => builder
        .MsalOAuth.FromAuthenticationResult(authenticationResult))
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

MSAL 로그인 결과로 마인인크래프트 로그인을 시도합니다. 
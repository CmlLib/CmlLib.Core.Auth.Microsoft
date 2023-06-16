# OAuthStrategies

MSAL 을 통해 Microsoft OAuth 를 진행하는 방법을 제공합니다. 

먼저 [ClientID](./ClientID.md) 를 통해 [IPublicClientAppliction 를 초기화](./MsalClientHelper.md)해야 합니다.

For example:

```csharp
using XboxAuthNet.Game.Msal;

var app = await MsalClientHelper.BuildApplicationWithCache("<CLIENT-ID>");
```

## Interactive

```csharp
authenticator.AddMsalOAuth(app, msal => msal.Interactive());
```

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. 로그인 페이지를 어떻게 표시할 지는 MSAL 에서 결정합니다. 

## EmbeddedWebView

```csharp
authenticator.AddMsalOAuth(app, msal => msal.EmbeddedWebView())
```

<img src="https://user-images.githubusercontent.com/17783561/154946636-960d3673-bb51-4f3a-ae92-f36940b8e3ad.png" width="500">

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. WebView2 를 사용해 로그인 페이지를 표시합니다.

## SystemBrowser

```csharp
authenticator.AddMsalOAuth(app, msal => msal.SystemBrowser());
```

<img src="https://user-images.githubusercontent.com/17783561/154945056-2f0d961b-f69b-4cea-a08a-9c3b050995f6.png" width="500">  

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. 시스템 기본 브라우저를 열어 로그인 페이지를 표시합니다. 

## Silent

```csharp
authenticator.AddMsalOAuth(app, msal => msal.Silent());
```

MSAL 에 캐시된 계정 정보를 이용해 로그인을 시도합니다.  

## DeviceCode

```csharp
authenticator.AddMsalOAuth(app, msal => msal.DeviceCode());
```

<img src="https://user-images.githubusercontent.com/17783561/154950501-4ffbd21f-b780-4217-bd83-641ae3ac5e95.png" width="500">
<img src="https://user-images.githubusercontent.com/17783561/154950743-823d5ecf-b303-4caf-a9cc-a1167007dd7c.png" width="500">

DeviceCode 방식으로 로그인을 시도합니다. 

## FromResult

```csharp
var result = await app.AcquireTokenInteractive().ExecuteAsync();
authenticator.AddMsalOAuth(app, msal => msal.FromResult(result));
```

MSAL 로그인 결과를 바로 사용합니다. 
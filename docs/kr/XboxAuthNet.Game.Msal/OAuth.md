# OAuth

MSAL 을 통해 Microsoft OAuth 를 진행하는 방법을 제공합니다. 

먼저 [ClientID](./ClientID.md) 를 통해 [IPublicClientAppliction 를 초기화](./MsalClientHelper.md)해야 합니다.

## Interactive

```csharp
authenticator.AddMsalOAuth(app, msal => msal.Interactive());
```

사용자에게 마이크로소프트 계정을 입력하도록 요청합니다. 로그인 페이지를 어떻게 표시할 지는 MSAL 에서 결정합니다. 

## EmbeddedWebView

```csharp
authenticator.AddMsalOAuth(app, msal => msal.EmbeddedWebView());
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

authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(deviceCode =>
{
    Console.WriteLine(deviceCode.Message);
    return Task.CompletedTask;
}));
```

DeviceCode 방식으로 로그인을 시도합니다. 이 방식은 클라이언트에서 웹 브라우저나 UI 가 필요하지 않은 대신 다른 기기에서 로그인을 진행할 수 있도록 해줍니다. 

콘솔에서만 작동하는 런처를 제작한다면 이 방식을 사용하여 로그인을 진행하세요. 로그인은 유저가 웹 브라우저를 직접 열어 [https://www.microsoft.com/link] 으로 접속하여 진행할 수 있습니다. 이때 로그인은 런처를 실행하는 기기와 전혀 다른 기기에서도 진행할 수 있습니다. 예를 들어 유저는 자신의 휴대폰으로 위 링크에 접속해 로그인을 진행할 수 있습니다. 

예제 코드는 콘솔에 다음과 같은 메세지를 출력합니다:

```
To sign in, use a web browser to open the page https://www.microsoft.com/link and enter the code XXXXXXXX to authenticate.
```

## FromResult

```csharp
var result = await app.AcquireTokenInteractive(MsalClientHelper.XboxScopes).ExecuteAsync();
authenticator.AddMsalOAuth(app, msal => msal.FromResult(result));
```

MSAL 로그인 결과를 바로 사용합니다. 
# OAuthStrategies

마이크로소프트 OAuth 를 진행하는 방법을 나타냅니다.

## InteractiveMicrosoftOAuthStrategy

(이미지)

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMicrosoftOAuth(builder => builder
        .MicrosoftOAuth.UseInteractiveStrategy()
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

사용자에게 마이크로소프트 계정을 입력하도록 OAuth 페이지를 표시합니다. 페이지는 WebView2 를 통해 표시되며, 윈도우 플랫폼에서만 이 기능을 사용할 수 있습니다. 다른 플랫폼에서 사용하기 위해서 [MSAL](../XboxAuthNet.Game.Msal/Home.md) 확장을 사용하세요. 

## SilentMicrosoftOAuthStrategy

```csharp
var session = await loginHandler.AuthenticateSilently()
    .WithMicrosoftOAuth(builder => builder
        .MicrosoftOauth.UseSilentStrategy())
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

캐시된 MicrosoftOAuth 계정 정보를 이용하여 사용자와 상호작용 없이 로그인을 시도합니다. 캐시된 MicrosoftOAuth 토큰이 만료되지 않은 상태라면 토큰을 그대로 사용하고, 만료된 상황이라면 RefreshToken 으로 토큰 재발급을 시도합니다. 재발급에 실패한다면 `MicrosoftOAuthException` 예외가 발생합니다. 
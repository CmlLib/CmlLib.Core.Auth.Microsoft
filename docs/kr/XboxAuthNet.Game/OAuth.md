# OAuth

마이크로소프트 OAuth 설정 방법을 설명합니다. 

## 설정 방법

`ICompositeAuthenticator` 의 확장 메서드를 통해 `Authenticator` 를 추가합니다. 

```csharp
var clientInfo = new MicrosoftOAuthClientInfo("<MICROSOFT_OAUTH_CLIENT_ID>", "<MICROSOFT_OAUTH_SCOPES>");

// 예시 1
authenticator.AddForceMicrosoftOAuth(clientInfo, oauth => oauth.Interactive());

// 예시 2
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Silent());
```

## AddMicrosoftOAuth / AddForceMicrosoftOAuth 차이

기본적으로 캐시된 Microsoft OAuth 세션의 유효성을 검사하여 세션이 유효하다면 로그인을 진행하지 않고, 다음 로그인 단계로 넘어갑니다.

Force 메서드는 Microsoft OAuth 세션의 유효성을 검사하지 않고 무조건 로그인을 진행합니다. 

## 로그인 모드 설정

### Interactive

```csharp
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Interactive());
```

```csharp
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Interactive(new MicrosoftOAuthParameters
{
    // OAuth 설정
    // 예시: Prompt 설정
    Prompt = MicrosoftOAuthPromptModes.SelectAccount
}));
```

<img src="https://user-images.githubusercontent.com/17783561/154854388-38c473f1-7860-4a47-bdbe-622de37eef8b.png" width="500">

유저가 Microsoft 계정의 이메일, 비밀번호를 입력하도록 창을 띄우고 로그인을 진행합니다. 

### Silent

```csharp
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Silent());
```

유저에게 로그인 입력 창을 띄우지 않고 로그인을 진행합니다. 캐시된 세션이 만료되지 않은 상태라면 토큰을 그대로 사용하고, 만료된 상황이라면 재발급을 시도합니다. 재발급에 실패한다면 `MicrosoftOAuthException` 예외가 발생합니다. 

# XboxAuth

Xbox 로그인을 진행하는 방법을 나타냅니다.

## Basic

```csharp
authenticator.AddXboxAuth(xbox => xbox.Basic("relyingParty"));
```

가장 기본적인 방식입니다. 로그인에 필요한 최소 정보(UserToken, XstsToken)만 받아옵니다.

나이 인증이 되지 않은 계정, 18세 미만인 계정은 이 방식으로 로그인할때 문제가 발생할 수 있습니다. (오류 코드 `8015dc0c`, `8015dc0d`, `8015dc0e`)  
[Full](#full) 방식이나 [Sisu](#sisu) 방식을 사용하면 이 문제를 해결할 수 있습니다. 

## Full

```csharp
authenticator.AddXboxAuth(xbox => xbox.Full("relyingParty"));
```

UserToken, DeviceToken, XstsToken 를 받아옵니다. 

## Sisu

```csharp
authenticator.AddXboxAuth(xbox => xbox.Sisu("relyingParty",  "<CLIENT-ID>"));
```

SISU 로그인 방식을 사용합니다. UserToken, DeviceToken, TitleToken, XstsToken 모든 토큰을 받아옵니다. 대부분의 나이 관련 문제는 이 방식으로 해결할 수 있습니다. 

이 방식은 `<CLIENT-ID>` 가 엑스박스 게임과 관련된 것일때만 작동합니다. (예시: 마인크래프트 런처에서 사용하는 CLIENT-ID)

개인이 발급한 Azure ID 를 사용할 수 없습니다. 즉 [MSAL](../XboxAuthNet.Game.Msal/Home.md) 와 함께 사용할 수 없습니다. 

## Device Options

```csharp
authenticator.AddXboxAuth(xbox => xbox
    .WithDeviceType(XboxDeviceTypes.Win32)
    .WithDeviceVersion("0.0.0")
    .Full("relyingParty"));
```

DeviceToken 을 받아오는 로그인 방식을 사용할 때, Device 설정을 적용할 수 있습니다. 로그인 방식을 정하기 전에 `.WithDeviceType()`, `.WithDeviceVersion()` 을 호출하세요. 

## Token Prefix

```csharp
authenticator.AddXboxAuth(xbox => xbox
    .WithAzureTokenPrefix()
    .Full("relyingParty"));
```

`WithAzureTokenPrefix()` 를 호출하면 Azure 클라이언트를 통해 Microsoft OAuth 세션을 얻은 경우 (MSAL 로 로그인), `WithXboxTokenPrefix()` 를 호출하면 엑스박스 게임 관련 클라이언트를 통해 Microsoft OAuth 세션을 얻은 경우 (예시: `JELoginHandler.DefaultMicrosoftOAuthClientInfo`) 사용합니다. 
`WithTokenPrefix("t=")` 처럼 TokenPrefix 를 직접 설정할 수 있습니다. 

## 오류 처리

Xbox 인증 시 다양한 오류 시나리오가 있습니다. 인증 시 오류가 발생하면 `XboxAuthException` 이 발생하며 ErrorCode 와 ErrorMessage 를 얻을 수 있습니다.

모든 ErrorCode 는 [여기서](./Errors.md) 확인할 수 있습니다. 

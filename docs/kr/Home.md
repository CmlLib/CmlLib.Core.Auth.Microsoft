# CmlLib.Core.Auth.Microsoft

마이크로소프트 엑스박스 계정으로 마인크래프트: 자바 에디션에 로그인하는 기능을 제공하는 라이브러리입니다.

## 설치

Nuget 패키지 `CmlLib.Core.Auth.Microsoft` 를 설치해 주세요.

## 사용 방법

### 간단한 사용 방법 

```csharp
using CmlLib.Core.Auth.Microsoft;

var loginHandler = JELoginHandlerBuilder.BuildDefault();
var session = await loginHandler.Authenticate();
```

CmlLib.Core 라이브러리로 게임을 실행시킬 때 session 변수와 함께 실행하면 됩니다.

## 자세한 사용 방법

매우 다양한 로그인 방식과 옵션이 있습니다. 더 자세한 설명은 [이 문서](./CmlLib.Core.Auth.Microsoft/Home.md)를 참고하세요.

## 예제

[WinFormTest](/examples/WinFormTest/)

# CmlLib.Core.Bedrock.Auth

마이크로소프트 엑스박스 계정으로 마인크래프트: 배드락 에디션 서버에 접속하기 위한 인증 토큰을 발급하는 기능을 제공하는 라이브러리입니다.

## 설치

Nuget 패키지 `CmlLib.Core.Bedrock.Auth` 를 설치해 주세요.

## 사용 방법

`CmlLib.Core.Auth.Microsoft` 와 사용 방법이 같습니다. 자세한 사용 방법은 [이 문서](./CmlLib.Core.Auth.Microsoft/Home.md)를 참고하세요.

```csharp
using CmlLib.Core.Bedrock.Auth;

var loginHandler = BELoginHandlerBuilder.BuildDefault();
var session = await loginHandler.Authenticate();
```

session 변수에 서버에 접속할 때 사용되는 인증 토큰이 담겨져 있습니다. 

# XboxAuthNet.Game.Msal

기본적으로 `CmlLib.Core.Auth.Microsoft`, `CmlLib.Core.Bedrock.Auth` 는 윈도우에서만 작동합니다. Linux, macOS 등 다른 플랫폼에서 사용하기 위해서는 `XboxAuthNet.Game.Msal` 확장이 필요합니다.

## 설치

Nuget 패키지 `XboxAuthNet.Game.Msal` 을 설치하세요. 

## 사용 방법

```csharp
using XboxAuthNet.Game.Msal;
```

자세한 사용 방법은 [이 문서](./XboxAuthNet.Game.Msal/Home.md)를 참고하세요.
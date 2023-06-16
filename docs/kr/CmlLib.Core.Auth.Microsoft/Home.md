# CmlLib.Core.Auth.Microsoft

마인크래프트: 자바 에디션에 로그인하는 기능을 제공하는 라이브러리입니다.

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

## 예제

[WinFormTest](/examples/WinFormTest/)

[ConsoleTest](/examples/ConsoleTest/Program.cs)

## 자세한 사용 방법

### [JELoginHandlerBuilder](./JELoginHandlerBuilder.md)

JELoginHandler 의 인스턴스를 만들어 주는 객체입니다. 

### [JELoginHandler](./JELoginHandler.md)

로그인, 로그아웃 기능과 계정 관리 기능을 제공합니다. 

### [AccountManager](./AccountManager.md)

여러 계정을 저장하고 불러올 수 있습니다. 

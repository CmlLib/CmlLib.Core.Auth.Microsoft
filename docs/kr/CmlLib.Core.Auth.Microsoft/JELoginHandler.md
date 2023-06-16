# JELoginHandler

로그인, 로그아웃, 계정 관리 기능을 제공합니다.

## loginHandler 인스턴스 만들기

```csharp
var loginHandler = JELoginHandlerBuilder.BuildDefault();
```

계정 저장 방식 지정, HttpClient 설정 등 더 자세한 설정 방법은 [JELoginHandlerBuilder](./JELoginHandlerBuilder.md) 문서를 참고하세요.

## 기본 로그인

```csharp
var session = await loginHandler.Authenticate();
```

계정 목록을 불러와 가장 최근에 플레이한 계정으로 로그인을 시도하고, 아무 계정도 저장되어 있지 않다면 새로운 계정을 추가하여 로그인합니다. 

## 새로운 계정으로 로그인

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .ExecuteForLauncherAsync();
```

<img src="https://user-images.githubusercontent.com/17783561/154854388-38c473f1-7860-4a47-bdbe-622de37eef8b.png" width="500">

새로운 계정을 추가하여 로그인합니다. 사용자에게 Microsoft OAuth 페이지를 표시하여 마이크로소프트 계정을 입력하도록 합니다. 

## 가장 최근에 플레이한 계정으로 로그인

```csharp
var session = await loginHandler.AuthenticateSilently()
    .ExecuteForLauncherAsync();
```

가장 최근에 플레이한 계정 정보를 이용하여 로그인합니다. 로그인 정보가 이미 저장되어 있기 때문에 사용자가 마이크로소프트 계정을 입력할 필요 없이 자동으로 로그인이 완료됩니다. 로그인 정보가 저장되어 있지 않거나 만료된 로그인 정보를 가지고 있다면 예외를 발생합니다. 

## 선택한 계정으로 로그인 

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var selectedAccount = accounts.ElementAt(1);
var session = await loginHandler.Authenticate(selectedAccount);
```

계정 목록을 불러온 후 두번째 계정 (index number 1) 으로 로그인을 시도합니다. 

## 가장 최근에 로그인한 계정을 로그아웃 

```csharp
await loginHandler.Signout();
```

## 선택한 계정 로그아웃

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var selectedAccount = accounts.ElementAt(1);
await loginHandler.Signout(selectedAccount);
```

계정 목록을 불러온 후 두번째 계정 (index number 1) 으로 로그아웃을 시도합니다. 

## 로그인 설정

로그인 과정을 자세하게 설정할 수 있습니다. 

```csharp
using XboxAuthNet.Game;

var authenticator = loginHandler.CreateAuthenticator(account, default);
authenticator.AddMicrosoftOAuthForJE(oauth => oauth.Interactive()); // Microsoft OAuth
authenticator.AddXboxAuthForJE(xbox => xbox.Basic()); // XboxAuth
authenticator.AddJEAuthenticator(); // JEAuthenticator
var session = await authenticator.ExecuteForLauncherAsync();
```

로그인은 크게 네 과정을 거칩니다. 

### 1. CreateAuthenticator

`Authenticator` 를 만듭니다.

#### CreateAuthenticator(XboxGameAccount account, CancellationToken cancellationToken)

설정한 `account` 로 `Authenticator` 를 만듭니다. 

#### CreateAuthenticatorWithNewAccount(CancellationToken cancellationToken)

새로운 계정으로 로그인하기 위한 `Authenticator` 를 만듭니다. 

#### CreateAuthenticatorWithDefaultAccount(CancellationToken cancellationToken)

가장 최근에 로그인한 계정으로 `Authenticator` 를 만듭니다. 

### 2. Microsoft OAuth

기본 로그인 방식은 [여기를 참고](./OAuth.md)하세요. 

#### AddMicrosoftOAuthForJE(oauthBuilder)

`AddMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, oauthBuilder)` 와 같습니다. 

#### AddForceMicrosoftOAuthForJE(oauthBuilder)

`AddForceMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, oauthBuilder)` 와 같습니다. 

[MSAL](../XboxAuthNet.Game.Msal/Home.md) 로그인 방식은 [여기를 참고](../XboxAuthNet.Game.Msal/OAuthStrategies.md)하세요.

### 3. XboxAuth

[XboxAuth](./XboxAuth.md)를 참고하세요. 

#### AddXboxAuthForJE(xboxBuilder) 

JE 의 기본 RelyingParty 를 설정한 xboxBuilder 를 제공합니다. 

### 4. JEAuthenticator 

[JEAuthenticator](./JEAuthenticator.md)를 참고하세요. 

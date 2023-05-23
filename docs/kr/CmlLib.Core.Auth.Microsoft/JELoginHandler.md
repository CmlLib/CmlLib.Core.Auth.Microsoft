# JELoginHandler

로그인, 로그아웃, 계정 관리 기능을 제공합니다.

## Build an instance

```csharp
var loginHandler = JELoginHandlerBuilder.BuildDefault();
```

계정 저장 방식 지정, HttpClient 설정 등 더 자세한 설정 방법은 [JELoginHandlerBuilder](./JELoginHandlerBuilder.md) 문서를 참고하세요.

## 계정 불러오기

저장되어 있는 모든 계정을 출력합니다. 

```csharp
var accounts = loginHandler.GetAccounts();
foreach (var account in accounts)
{
    Console.WriteLine($"Identifier: {account.Identifier}");
    if (account is JEGameAccount jeAccount)
    {
        Console.WriteLine($"Username: {account.Session?.Profile?.Username}");
    }
}
```

## 로그인

```csharp
var session = await loginHandler.Authenticate();
```

먼저 계정 목록을 불러와 가장 최근에 플레이한 계정으로 로그인을 시도하고, 아무 계정도 저장되어 있지 않다면 새로운 계정을 추가하여 로그인합니다. 

### 새로운 계정으로 로그인

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .ExecuteForLauncherAsync();
```

(이미지)

새로운 계정을 추가하여 로그인합니다. 사용자에게 Microsoft OAuth 페이지를 표시하여 마이크로소프트 계정을 입력하도록 합니다. 

### 가장 최근에 플레이한 계정으로 조용히 로그인

```csharp
var session = await loginHandler.AuthenticateSilently()
    .ExecuteForLauncherAsync();
```

가장 최근에 플레이한 계정 정보를 이용하여 로그인합니다. 로그인 정보가 이미 저장되어 있기 때문에 사용자가 마이크로소프트 계정을 입력할 필요 없이 바로 로그인이 완료됩니다. 

### 선택한 계정으로 로그인 

```csharp
var accounts = loginHandler.GetAccounts().ToList();
var selectedAccount = accounts[1];
var session = await loginHandler.Authenticate(selectedAccount);
```

계정 목록을 불러온 후 두번째 계정 (index number 1) 으로 로그인을 시도합니다. 

## 로그아웃

### 가장 최근에 로그인한 계정을 로그아웃 

```csharp
await loginHandler.Signout();
```

### 선택한 계정 로그아웃

```csharp
var accounts = loginHandler.GetAccounts().ToList();
var selectedAccount = accounts[1];
await loginHandler.Signout(selectedAccount);
```

계정 목록을 불러온 후 두번째 계정 (index number 1) 으로 로그아웃을 시도합니다. 

### 계정 정보 초기화

```csharp
loginHandler.AccountManager.ClearAccounts();
```

## 로그인 정보가 저장되는 방식

자세한 정보는 [Accounts](./Accounts.md) 문서를 참고하세요. 

### ISessionStorage 지정

```csharp
var sessionStorage = new InMemorySessionStorage();
var session = await loginHandler.AuthenticateSilently()
    .WithSessionStorage(sessionStorage)
    .ExecuteForLauncherAsync();
```

*note: `AuthenticateSilently()`, `AuthenticateInteractively()` 모두 사용 가능합니다.*

### IXboxGameAccount 지정

```csharp
var account = loginHandler.GetAccounts().First();
var session = await loginHandler.AuthenticateSilently()
    .WithAccount(account);
    .ExecuteForLauncherAsync();
```

### IXboxGameAccountManager 지정 

```csharp
var session = await loginHandler.AuthenticateSilently()
    .WithDefaultAccount(loginHandler.AccountManager)
    //.WithNewAccount(loginHandler.AccountManager)
    .ExecuteForLauncherAsync();
```

`WithDefaultAccount` 메서드는 가장 최근에 로그인한 계정을, `WithNewAccount` 메서드는 새로운 계정을 AccountManager 에게 요청하여 로그인합니다.

```csharp
var session = await loginHandler.AuthenticateSilently()
    .WithAccountManager(loginHandler.AccountManager)
    .ExecuteForLauncherAsync();
```

`WithAccountManager` 메서드는 로그인 후 AccountManager 에게 계정 정보 저장을 요청합니다.
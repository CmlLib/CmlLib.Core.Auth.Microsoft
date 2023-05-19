# Accounts

## ISessionStorage

로그인 과정에서 얻는 각종 토큰, 세션은 `ISessionStorage` 에 저장됩니다. 하나의 계정은 하나의 `ISessionStorage` 인스턴스에 저장됩니다. 예를 들어, `Notch` 라는 유저가 로그인을 진행하면 Microsoft OAuth 토큰, Xbox 토큰, 마인크래프트 JE 토큰을 얻게 됩니다. 이 세가지 정보는 `ISessionStorage` 인스턴스 하나에 모두 저장되며, 오직 `Notch` 유저와 관련된 로그인 정보만 담게 됩니다.  

`ISessionStorage` 의 구현체는 모든 정보를 메모리에 저장하는 `InMemorySessionStorage`, 메모리 내 Json 객체로 관리하는 `JsonSessionStorage`, Json 파일로 관리하는 `JsonFileSessionStorage` 가 있습니다. 

### 정보 저장하고 불러오기

```csharp
var sessionStorage = new InMemorySessionStorage();

// save data
sessionStorage.Set<string>("myData", "HelloWorld");

// load data
var myData = sessionStorage.Get<string>("myData");

// save and load data via ISessionSource
var sessionSource = new MicrosoftOAuthSessionSource(sessionStorage);
sessionSource.Set(new MicrosoftOAuthResponse());
var oauth = sessionSource.Get();
```

## IXboxGameAccount

여러 계정을 관리하기 위해서 `IXboxGameAccount` 와 `IXboxGameAccountManager` 인터페이스가 있습니다.

`IXboxGameAccount` 는 `ISessionStorage` 를 대표하는 문자열(Identifier)을 만들어 다른 `ISessionStorage` 와 구분할 수 있는 방법을 제공합니다. 예를 들어 `CmlLib.Core.Auth.Microsoft` 는 기본적으로 `JEGameAccount` 구현체를 사용하는데, 이는 마인크래프트 JE 유저의 UUID 값을 Identifier 로 사용합니다. 

### JEGameAccount

```csharp
var account = loginHandler.GetAccounts().First();

```

## IXboxGameAccountManager

`IXboxGameAccountManager` 는 `XboxGameAccountCollection`을 저장하고 불러오는 방법을 제공합니다. `JsonXboxGameAccountManager` 는 모든 `IXboxGameAccount` 를 하나의 Json 파일에 저장하고 불러옵니다. 

```csharp
var jsonAccountManager = new JsonXboxGameAccountManager("accounts.json", JEGameAccount.FromSessionStorage);
jsonAccountManager.Load();
foreach (var account in jsonAccountManager.Accounts)
{
    Console.WriteLine(account.Identifier);
}
jsonAccountManager.Save();
```
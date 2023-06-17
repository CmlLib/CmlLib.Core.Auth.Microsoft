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
var sessionSource = MicrosoftOAuthSessionSource.Default;
sessionSource.Set(sessionStorage, new MicrosoftOAuthResponse());
var oauth = sessionSource.Get(sessionStorage);
```

## XboxGameAccount

XboxGameAccount 는 내부적으로 ISessionStorage 를 가지며 추가적인 기능을 제공합니다.

- ISessionStorage 를 구분할 수 있도록 식별자(identifier)를 제공
- ISessionStorage 가 가지고 있는 세션 정보에 쉽게 접근하기 위한 프로퍼티 제공 (예: LastAccess, XboxTokens)

### Identifier

여러 계정을 관리하기 위해서는 여러 ISessionStorage 관리해야 합니다. 이때 각 ISessionStorage 를 구분하기 위한 식별자가 필요합니다. 

두 계정의 Identifier 가 같다면 ISessionStorage 가 서로 다른 데이터를 가지고 있다 하더라도 같은 계정이라고 판단합니다. 

예를 들어 마인크래프트 자바 에디션 계정을 나타내는 `JEGameAccount` 는 유저의 UUID 를 식별자로 사용합니다. 

### LastAccess

마지막으로 이 계정에 접근한 시간을 나타냅니다. 

### XboxTokens

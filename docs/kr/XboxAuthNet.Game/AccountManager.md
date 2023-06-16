# AccountManager

여러 계정을 관리하는 방법을 설명합니다. 
각 계정은 `identifier` 이라는 고유한 값으로 식별됩니다. 

## 모든 계정 불러오기

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
foreach (var account in accounts)
{
    Console.WriteLine(account.Identifier);
}
```

## Identifier 으로 계정 불러오기

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var account = accounts.GetAccount("identifier");
```

## 가장 최근에 플레이한 계정 가져오기

```csharp
var account = loginHandler.AccountManager.GetDefaultAccount();
```

## 새로운 계정 만들기

```csharp
var account = loginHandler.AccountManager.NewAccount();
```

## 모든 계정 정보 삭제

```csharp
loginHandler.AccountManager.ClearAccounts();
```

## 계정 정보 저장

```csharp
loginHandler.AccountManager.SaveAccounts();
```

## 계정으로부터 마인크래프트: 자바 에디션 유저 정보 가져오기  

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var myAccount = accounts.First();

if (myAccount is JEGameAccount jeAccount)
{
    Console.WriteLine(jeAccount.Profile?.Username); // 유저이름 출력 
    Console.WriteLine(jeAccount.Profile?.UUID); // UUID 출력 
    Console.WriteLine(jeAccount.Token?.AccessToken); // 엑세스 토큰 출력 
}
else
{
    // 자바 에디션의 게임 계정이 아닌 경우 
}
```

## 계정으로부터 CmlLib.Core 에서 사용할 MSession 가져오기

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var myAccount = accounts.First();

MSession session = myAccount.ToLauncherSession();
```

# AccountManager

Describes how to manage multiple accounts. 
Each account is identified by a unique value called `identifier`.

## Get All Accounts

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
foreach (var account in accounts)
{
    Console.WriteLine(account.Identifier);
}
```

## Get Account by Identifier

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var account = accounts.GetAccount("identifier");
```

## Get the Most Recent Account

```csharp
var account = loginHandler.AccountManager.GetDefaultAccount();
```

## Create New Empty Account

```csharp
var account = loginHandler.AccountManager.NewAccount();
```

## Clear All Accounts

```csharp
loginHandler.AccountManager.ClearAccounts();
```

## Save

```csharp
loginHandler.AccountManager.SaveAccounts();
```

## Get Minecraft:JE user information from the Account  

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var myAccount = accounts.First();

if (myAccount is JEGameAccount jeAccount)
{
    Console.WriteLine(jeAccount.Profile?.Username);
    Console.WriteLine(jeAccount.Profile?.UUID);
    Console.WriteLine(jeAccount.Token?.AccessToken);
}
else
{
    // if the account is not Minecraft: JE account
}
```

## Get MSession for CmlLib.Core from the account

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var myAccount = accounts.First();

MSession session = myAccount.ToLauncherSession();
```

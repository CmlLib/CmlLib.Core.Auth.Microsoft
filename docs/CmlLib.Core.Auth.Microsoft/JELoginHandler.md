# JELoginHandler

Login, logout, account managements.

## Creating JELoginHandler instance

```csharp
var loginHandler = JELoginHandlerBuilder.BuildDefault();
```

For more detailed initialization, which includes specifying how accounts are stored, setting up HttpClient, and more, please refer to [JELoginHandlerBuilder](./JELoginHandlerBuilder.md).

## Basic Authentication

```csharp
var session = await loginHandler.Authenticate();
```

Load the account list and attempt to log in with the most recent account. If no account is saved, a new empty account will be used.

## Authenticating with New Account

```csharp
var session = await loginHandler.AuthenticateInteractively();
```

<img src="https://user-images.githubusercontent.com/17783561/154854388-38c473f1-7860-4a47-bdbe-622de37eef8b.png" width="500">

Add a new account to sign in. Show the user the Microsoft OAuth page to enter their Microsoft account.

## Authenticating with the Most Recent Account

```csharp
var session = await loginHandler.AuthenticateSilently();
```

Using the account information of the most account, log in. Since the login information is already saved, the login process will be completed without the need for the user to enter their Microsoft account. If there is no saved login information or if the stored login information has expired, an exception will be thrown.

## Authenticating with the Selected Account 

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var selectedAccount = accounts.ElementAt(1);
var session = await loginHandler.Authenticate(selectedAccount);
```

Load account list and authenticate with second account (index number 1). 

## Signing out from the most Recent Account

```csharp
await loginHandler.Signout();
```

## Signing out from Selected Account

```csharp
var accounts = loginHandler.AccountManager.GetAccounts();
var selectedAccount = accounts.ElementAt(1);
await loginHandler.Signout(selectedAccount);
```

Load account list and sign out from second account (index number 1). 

## Authenticating with More Options

```csharp
using XboxAuthNet.Game;

var authenticator = loginHandler.CreateAuthenticator(account, default);
authenticator.AddMicrosoftOAuthForJE(oauth => oauth.Interactive()); // Microsoft OAuth
authenticator.AddXboxAuthForJE(xbox => xbox.Basic()); // XboxAuth
authenticator.AddJEAuthenticator(); // JEAuthenticator
var session = await authenticator.ExecuteForLauncherAsync();
```

There are four main steps in authentication.

### 1. CreateAuthenticator

Initialize `Authenticator` instance.

#### CreateAuthenticator(XboxGameAccount account, CancellationToken cancellationToken)

Initialize `Authenticator` with specified `account`.

#### CreateAuthenticatorWithNewAccount(CancellationToken cancellationToken)

Initialize `Authenticator` with new empty account.

#### CreateAuthenticatorWithDefaultAccount(CancellationToken cancellationToken)

Initialize `Authenticator` with the most recent account. 

### 2. Microsoft OAuth

See [OAuth](../XboxAuthNet.Game/OAuth.md).

#### AddMicrosoftOAuthForJE(oauthBuilder)

Same as `AddMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, oauthBuilder)`

#### AddForceMicrosoftOAuthForJE(oauthBuilder)

Same as `AddForceMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, oauthBuilder)`

To authenticate with [MSAL](../XboxAuthNet.Game.Msal/Home.md), please refer to [here](../XboxAuthNet.Game.Msal/OAuth.md)하세요.

### 3. XboxAuth

See [XboxAuth](../XboxAuthNet.Game/XboxAuth.md).

#### AddXboxAuthForJE(xboxBuilder) 

Provides preset `xboxBuilder` with the relyingParty as Minecraft: JE's relyingParty.

### 4. JEAuthenticator 

See [JEAuthenticator](./JEAuthenticator.md).

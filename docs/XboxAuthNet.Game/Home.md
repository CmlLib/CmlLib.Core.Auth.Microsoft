# XboxAuthNet.Game

Provides the foundation for signing in to Xbox games. 

It provides common functionality for signing in to Xbox games, including Microsoft sign-in functionality, Xbox sign-in functionality, and account management functionality. 

For example, the common functionality of [CmlLib.Core.Auth.Microsoft](../CmlLib.Core.Auth.Microsoft/Home.md) for logging into Minecraft Java Edition and [CmlLib.Core.Bedrock.Auth](../CmlLib.Core.Bedrock.Auth/Home.md) for logging into Badrock Edition are both provided by this library.

## Authenticator

### [OAuth](./OAuth.md)

Provides OAuth sign-in with a Microsoft account.

### [XboxAuth](./XboxAuth.md)

Provides for Xbox authentication with a Microsoft OAuth token.

### Extensions

Authentication are designed to be easily extensible: you can easily add new authenticator, and you can freely reorder them.

For example, there is extension library for Microsoft OAuth using MSAL library, [XboxAuthNet.Game.Msal](../XboxAuthNet.Game.Msal/Home.md). 

## Account

### [AccountManager](./AccountManager.md)

Manage account list.

### [Accounts](./Accounts.md)

Manage each account.

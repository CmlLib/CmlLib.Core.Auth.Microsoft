# CmlLib.Core.Auth.Microsoft

Login, logout, and account management in Minecraft: Java Edition

## Install

Install nuget package [CmlLib.Core.Auth.Microsoft](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft)

## Getting Started

```csharp
using CmlLib.Core.Auth.Microsoft;

var loginHandler = JELoginHandlerBuilder.BuildDefault();
var session = await loginHandler.Authenticate();
```

Run the game with the CmlLib.Core library with the `session` variable.

## Example

[WinFormTest](/examples/WinFormTest/)

[ConsoleTest](/examples/ConsoleTest/Program.cs)

## Usage

### [JELoginHandlerBuilder](./JELoginHandlerBuilder.md)

Builder for initializing an instance of `JELoginHandler`.

### [JELoginHandler](./JELoginHandler.md)

Login, logout, and account managements.

### [AccountManager](../XboxAuthNet.Game/AccountManager.md)

Manage account list.

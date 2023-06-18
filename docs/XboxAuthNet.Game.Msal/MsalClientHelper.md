# MsalClientHelper

Provides helper methods for initializing `IPublicClientApplication`.

## Example

```csharp
using XboxAuthNet.Game.Msal;

IPublicClientApplication app = await MsalClientHelper.BuildApplicationWithCache("<CLIENT-ID>");
```

Fill your Azure App Id in `<CLIENT-ID>`. For more information, see the [ClientID](./ClientID.md).

## CreateDefaultApplicationBuilder(string cid)

Initializing a `PublicClientApplicationBuilder` instance set up for Xbox authentication.

## RegisterCache(IPublicClientApplication app, MsalCacheSettings cacheSettings)

Apply the cache settings object, `cacheSettings`, to `app`.

## RegisterCache(IPublicClientApplication app, StorageCreationProperties storageProperties)

Apply the cache settings object `storageProperties` to `app`.

## BuildApplication(string cid)

Initializing an `IPublicClientApplication` set up for Xbox authentication.

## BuildApplicationWithCache(string cid)

Initializing an `IPublicClientApplication` set up for Xbox authentication and returns it with the default account cache settings applied.

The default cache settings are:

## BuildApplicationWithCache(string cid, MsalCacheSettings cacheSettings)

Initializing an `IPublicClientApplication` set up for Xbox authentication, applies the cache settings, `cacheSettings`, and returns it.

## BuildApplicationWithCache(string cid, StorageCreationProperties storageProperties)

Create an `IPublicClientApplication` set up for Xbox authentication and return it with the cache settings, `storageProperties`.

## ToMicrosoftOAuthResponse(AuthenticationResult result)

Convert the MSAL login result, `AuthenticationResult`, to a `MicrosoftOAuthResponse` object used by `XboxAuthNet`.
# MsalClientHelper

`IPublicClientApplication` 초기화를 위한 Helper 메서드를 제공합니다.

## Example

```csharp
using XboxAuthNet.Game.Msal;

IPublicClientApplication app = await MsalClientHelper.BuildApplicationWithCache("<CLIENT-ID>");
```

`<CLIENT-ID>` 에 발급받은 Azure App Id 를 입력하면 됩니다. 자세한 내용은 [ClientID](./ClientID.md) 문서를 참고하세요. 

## CreateDefaultApplicationBuilder(string cid)

Xbox authentication 을 위해 설정된 `PublicClientApplicationBuilder` 인스턴스를 만들어 반환합니다.

## RegisterCache(IPublicClientApplication app, MsalCacheSettings cacheSettings)

캐시 설정 객체인 `cacheSettings` 를 `app` 에 적용합니다. 

## RegisterCache(IPublicClientApplication app, StorageCreationProperties storageProperties)

캐시 설정 객체인 `storageProperties` 를 `app` 에 적용합니다. 

## BuildApplication(string cid)

Xbox authentication 을 위해 설정된 `IPublicClientApplication` 을 만들어 반환합니다. 

## BuildApplicationWithCache(string cid)

Xbox authentication 을 위해 설정된 `IPublicClientApplication` 을 만들고 기본 계정 캐시 설정을 적용하여 반환합니다.

기본 캐시 설정은 다음과 같습니다: 

## BuildApplicationWithCache(string cid, MsalCacheSettings cacheSettings)

Xbox authentication 을 위햇 설정된 `IPublicClientApplication` 을 만들고 캐시 설정인 `cacheSettings` 를 적용하여 반환합니다. 

## BuildApplicationWithCache(string cid, StorageCreationProperties storageProperties)

Xbox authentication 을 위햇 설정된 `IPublicClientApplication` 을 만들고 캐시 설정인 `storageProperties` 를 적용하여 반환합니다. 

## ToMicrosoftOAuthResponse(AuthenticationResult result)

MSAL 로그인 결과인 `AuthenticationResult` 를 `XboxAuthNet` 와 `CmlLib.Core.Auth.Microsoft` 에서 사용하는 `MicrosoftOAuthResponse` 객체로 변환합니다. 

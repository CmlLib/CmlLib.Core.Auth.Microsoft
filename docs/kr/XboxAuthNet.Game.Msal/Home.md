# XboxAuthNet.Game.Msal

MSAL 을 사용하여 마이크로소프트 OAuth 를 진행할 수 있는 확장 메서드를 제공합니다. 

MSAL 을 사용하면 윈도우 뿐만 아니라 Linux, macOS 등 모든 플랫폼에서 로그인을 진행할 수 있습니다. 

## Install

Nuget package [XboxAuthNet.Game.Msal](https://www.nuget.org/packages/XboxAuthNet.Game.Msal)

이 패키지를 사용하기 위해서는 반드시 `IPublicClientApplication` 을 적절하게 초기화해야 합니다. 

## [ClientID](./ClientID.md)

`IPublicClientApplication` 사용을 위한 Azure App Id 를 발급받는 방법을 설명합니다. 

## [MsalClientHelper](./MsalClientHelper.md)

마인크래프트 로그인을 위한 `IPublicClientApplication` 초기화 방법을 설명합니다. 

## [OAuth](./OAuth.md)

MSAL 을 이용해 마이크로소프트 OAuth 를 진행할 수 있는 방법을 제공합니다. 

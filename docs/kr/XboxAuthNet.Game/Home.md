# XboxAuthNet.Game

엑스박스 게임에 로그인하기 위한 기반을 제공합니다. 

마이크로소프트 로그인 관련 기능, 엑스박스 로그인 관련 기능, 그리고 계정 관리 기능 등등 엑스박스 게임 로그인 기능을 위한 공통 기능을 제공합니다. 

예를 들어 마인크래프트 자바 에디션에 로그인하기 위한 [CmlLib.Core.Auth.Microsoft](../CmlLib.Core.Auth.Microsoft/Home.md), 배드락 에디션에 로그인하기 위한 [CmlLib.Core.Bedrock.Auth](../CmlLib.Core.Bedrock.Auth/Home.md) 의 공통 기능은 모두 이 라이브러리에서 제공합니다. 

## Authenticator

### [OAuth](./OAuth.md)

마이크로소프트 계정으로 OAuth 로그인 기능을 제공합니다.

### [XboxAuth](./XboxAuth.md)

마이크로소프트 OAuth 로그인 결과로 Xbox 계정에 로그인하는 기능을 제공합니다.

### Extensions

로그인 방식은 쉽게 확장할 수 있도록 설계하였습니다. 새로운 로그인 방식을 쉽게 추가할 수 있고, 로그인 순서도 자유롭게 바꿀 수 있습니다. 

예를 들어 MSAL 을 통한 OAuth 로그인 기능을 제공하는 [XboxAuthNet.Game.Msal](../XboxAuthNet.Game.Msal/Home.md) 라이브러리가 있습니다. 

## Account

### [AccountManager](./AccountManager.md)

여러 계정을 관리하는 기능을 제공합니다. 

### [Accounts](./Accounts.md)

각 계정을 관리하는 방법을 설명합니다. 

# CmlLib.Core.Auth.Microsoft 3.5.0

[#28](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/pull/28)의 Minecraft Java 프로필 마지막 조회 시각을 포함한다. 정상 조회에서 `JEProfile.LastRefreshedAt`을 UTC로 기록하고, 실패 시 저장된 프로필과 시각을 보존한다. 갱신 시각이 없는 기존 계정 파일도 읽을 수 있다.

기존 NuGet 배포본 3.4.0과 같은 의존성을 유지하도록 프로젝트 참조의 패키지 버전을 이미 배포된 `XboxAuthNet.Game` 1.5.0과 `XboxAuthNet` 3.1.0에 맞춘다. 이번 배포 대상은 `CmlLib.Core.Auth.Microsoft` 하나다.

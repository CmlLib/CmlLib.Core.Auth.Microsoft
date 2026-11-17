# Minecraft Java 프로필 갱신 시각

런처는 `JEGameAccount.Profile?.LastRefreshedAt`으로 저장된 프로필을 마지막으로 조회한 UTC 시각을 읽을 수 있다. CmlLib는 Minecraft 프로필 응답에서 UUID와 이름을 읽고 현재 검사를 통과한 뒤, 시각을 프로필과 함께 계정 저장소에 기록한다.

조회가 실패하면 이전 프로필과 갱신 시각이 유지된다. 이전 버전에서 저장된 프로필은 갱신 시각이 `null`로 읽히며, 다음 프로필 조회가 성공하면 값이 기록된다. `lastAccess`는 계정 사용 시각으로 별도 관리한다.

시각이 없는 경우 계정 저장소의 기존 JSON 설정에 따라 `lastRefreshedAt`을 생략한다.

이 변경은 [Snowfrost의 프로필 조회 실패 처리 계획](https://github.com/NamuLauncher/ml-codebase/issues/136)에서 확인한 갱신 시각 기록 요구를 반영한다.

# LoginHandler

거의 모든 예외는 MicrosoftOAuthException, XboxAuthException, MojangAuthException 세 가지로 분류되어 InnerException 설정을 해야 함

## Methods

### void ClearCache()

ICacheManager 에 빈 객체를 저장

### virtual Task<MSession?> LoginFromCache()

`ICacheManager` 를 통해 캐시된 세션을 가져온 후 유효한 토큰이라면 바로 반환하고, 만료된 토큰이면 재발급을 시도하고, 재발급을 실패한 경우 null 을 반환한다.

### virtual string CreateOAuthUrl()

OAuth 로그인을 위한 url 을 생성한다.

### virtual bool CheckOAuthLoginSuccess(string url)

주어진 리다이렉트 url 을 보고 OAuth 로그인이 성공했는지, 실패했는지 확인

### Task<MSession> LoginFromOAuth()

`CheckOAuthLoginSuccess` 메서드의 반환 결과가 true 인 경우, 객체 내부적으로 `MicrosoftOAuthAuthCode` 가 저장된다. 이것을 통해 로그인을 시도한다. 

### Task<MSession> LoginFromOAuth(MicrosoftOAuthAuthCode oauth)

주어진 `MicrosoftOAuthAuthCode` 를 통해 로그인을 시도한다.

### virtual Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)

주어진 `MicrosoftOAuthResponse` 를 통해 로그인을 시도한다.

### virtual Task<XboxAuthResponse> LoginXbox(MicrosoftOAuthResponse msToken)

주어진 `MicrosoftOAuthResponse` 를 통해 xbox live 서비스의 로그인을 시도한다. relyingParty 는 `rp://api.minecraftservices.com/` 으로 설정된다.

### virtual Task<XboxAuthResponse> LoginXbox(MicrosoftOAuthResponse msToken, string relyingParty)

주어진 `MicrosoftOAuthResponse` 를 통해 xbox live 서비스의 로그인을 시도한다.

### virtual Task<MojangXboxLoginResponse> LoginMinecraft(string userHash, string xsts)

userHash 와 xsts 토큰을 통해 로그인을 시도한다. userHash 값은 

### virtual Task<MSession> CreateMinecraftSession(MojangXboxResponse xboxToken)
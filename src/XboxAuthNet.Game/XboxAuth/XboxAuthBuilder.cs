using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Crypto;

namespace XboxAuthNet.Game.XboxAuth;

public class XboxAuthBuilder
{
    public string? RelyingParty { get; set; }
    public string DeviceType { get; set; } = XboxDeviceTypes.Win32;
    public string DeviceVersion { get; set; } = "0.0.0";
    public string TokenPrefix { get; set; } = XboxAuthConstants.XboxTokenPrefix;
    public bool UseXuiClaimsAuth { get; set; } = true;

    private ISessionSource<MicrosoftOAuthResponse>? _oauthSessionSource;
    public ISessionSource<MicrosoftOAuthResponse> OAuthSessionSource
    {
        get => _oauthSessionSource ??= MicrosoftOAuthSessionSource.Default;
        set => _oauthSessionSource = value;
    }

    private ISessionSource<XboxAuthTokens>? _sessionSource;
    public ISessionSource<XboxAuthTokens> SessionSource
    {
        get => _sessionSource ??= XboxSessionSource.Default;
        set => _sessionSource = value;
    }

    private IXboxRequestSigner? _requestSigner;
    public IXboxRequestSigner RequestSigner
    {
        get => _requestSigner ??= new XboxRequestSigner(new ECDCertificatePopCryptoProvider());
        set => _requestSigner = value;
    }

    public XboxAuthBuilder WithRelyingParty(string relyingParty)
    {
        RelyingParty = relyingParty;
        return this;
    }

    public XboxAuthBuilder WithOAuthSessionSource(ISessionSource<MicrosoftOAuthResponse> oauthSessionSource)
    {
        OAuthSessionSource = oauthSessionSource;
        return this;
    }

    public XboxAuthBuilder WithSessionSource(ISessionSource<XboxAuthTokens> sessionSource)
    {
        SessionSource = sessionSource;
        return this;
    }

    public XboxAuthBuilder WithXboxTokenPrefix()
    {
        return WithTokenPrefix(XboxAuthConstants.XboxTokenPrefix);
    }

    public XboxAuthBuilder WithAzureTokenPrefix()
    {
        return WithTokenPrefix(XboxAuthConstants.AzureTokenPrefix);
    }

    public XboxAuthBuilder WithTokenPrefix(string tokenPrefix)
    {
        TokenPrefix = tokenPrefix;
        return this;
    }

    public XboxAuthBuilder WithDeviceType(string deviceType)
    {
        DeviceType = deviceType;
        return this;
    }

    public XboxAuthBuilder WithDeviceVersion(string deviceVersion)
    {
        DeviceVersion = deviceVersion;
        return this;
    }

    public XboxAuthBuilder WithRequestSigner(IXboxRequestSigner signer)
    {
        RequestSigner = signer;
        return this;
    }

    public XboxAuthBuilder WithXuiClaimsAuth(bool use)
    {
        UseXuiClaimsAuth = use;
        return this;
    }

    public ISessionValidator Validator() =>
        new XboxSessionValidator(SessionSource);

    public IAuthenticator UserTokenAuth() =>
        new XboxUserTokenAuth(OAuthSessionSource, SessionSource);

    public IAuthenticator SignedUserTokenAuth() =>
        new XboxSignedUserTokenAuth(RequestSigner, OAuthSessionSource, SessionSource);

    public IAuthenticator DeviceTokenAuth() =>
        new XboxDeviceTokenAuth(DeviceType, DeviceVersion, RequestSigner, SessionSource);

    public IAuthenticator XstsTokenAuth() => XstsTokenAuth(tryGetRelyingParty());
    public IAuthenticator XstsTokenAuth(string relyingParty)
    {
        if (string.IsNullOrEmpty(relyingParty))
            throw new ArgumentNullException(nameof(relyingParty));

        return new XboxXstsTokenAuth(relyingParty, SessionSource);
    }

    public ISessionValidator XuiClaimsValidator() =>
        XuiClaimsValidator(new [] { XboxAuthXuiClaimNames.Gamertag, XboxAuthXuiClaimNames.XboxUserId });
    
    public ISessionValidator XuiClaimsValidator(string[] claimNames) =>
        new XboxXuiClaimsValidator(claimNames, SessionSource);

    public IAuthenticator XuiClaimsAuth() =>
        XuiClaimsAuth(new [] { XboxAuthXuiClaimNames.Gamertag, XboxAuthXuiClaimNames.XboxUserId });

    public IAuthenticator XuiClaimsAuth(string[] claimNames) =>
        new XboxXuiClaimsAuth(claimNames, SessionSource);

    public IAuthenticator Basic() => Basic(tryGetRelyingParty());
    public IAuthenticator Basic(string relyingParty)
    {
        if (string.IsNullOrEmpty(relyingParty))
            throw new ArgumentNullException(nameof(relyingParty));

        var collection = new AuthenticatorCollection();
        collection.AddAuthenticatorWithoutValidator(UserTokenAuth());
        collection.AddAuthenticatorWithoutValidator(XstsTokenAuth(relyingParty));
        if (UseXuiClaimsAuth)
            collection.AddAuthenticatorWithoutValidator(XuiClaimsAuth());
        return collection;
    }

    public IAuthenticator Full() => Full(tryGetRelyingParty());
    public IAuthenticator Full(string relyingParty)
    {
        if (string.IsNullOrEmpty(relyingParty))
            throw new ArgumentNullException(nameof(relyingParty));

        var collection = new AuthenticatorCollection();
        collection.AddAuthenticatorWithoutValidator(UserTokenAuth());
        collection.AddAuthenticatorWithoutValidator(DeviceTokenAuth());
        collection.AddAuthenticatorWithoutValidator(XstsTokenAuth(relyingParty));
        if (UseXuiClaimsAuth)
            collection.AddAuthenticatorWithoutValidator(XuiClaimsAuth());
        return collection;
    }

    public IAuthenticator Sisu(string clientId) => Sisu(tryGetRelyingParty(), clientId);
    public IAuthenticator Sisu(string relyingParty, string clientId)
    {
        if (string.IsNullOrEmpty(relyingParty))
            throw new ArgumentNullException(nameof(relyingParty));

        var collection = new AuthenticatorCollection();
        collection.AddAuthenticatorWithoutValidator(DeviceTokenAuth());
        collection.AddAuthenticatorWithoutValidator(new XboxSisuAuth(
            clientId, 
            TokenPrefix, 
            relyingParty,
            RequestSigner,
            OAuthSessionSource,
            SessionSource));
        if (UseXuiClaimsAuth)
            collection.AddAuthenticatorWithoutValidator(XuiClaimsAuth());
        return collection;
    }

    public IAuthenticator ClearSession() =>
        new SessionCleaner<XboxAuthTokens>(SessionSource);

    private string tryGetRelyingParty() => RelyingParty ?? 
        throw new InvalidOperationException(
            "RelyingParty was not set. " + 
            "Call `WithRelyingParty(\"relyingParty\")` first or " + 
            "Call method with relyingParty parameter.");
}

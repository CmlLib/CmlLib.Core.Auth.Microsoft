using CmlLib.Core.Auth.Microsoft.Sessions;
using System.ComponentModel;

namespace WinFormTest
{
    internal class JEGameAccountProperties
    {
        private readonly JEGameAccount account;

        public JEGameAccountProperties(JEGameAccount account)
        {
            this.account = account;
        }

        [Category("Account")]
        public string? Identifier => account.Identifier;
        [Category("Account")]
        public string? Gamertag => account.Gamertag;
        [Category("Account")]
        public DateTime? LastAccess => account.LastAccess;

        [Category("Profile")]
        public string? Username => account.Profile?.Username;
        [Category("Profile")]
        public string? UUID => account.Profile?.UUID;
        [Category("Profile")]
        public object? Skins => account.Profile?.Skins;
        [Category("Profile")]
        public object? Capes => account.Profile?.Capes;

        [Category("Token")]
        public string? TokenUsername => account.Token?.Username;
        [Category("Token")]
        public string? AccessToken => account.Token?.AccessToken;
        [Category("Token")]
        public string? TokenType => account.Token?.TokenType;
        [Category("Token")]
        public int? ExpiresIn => account.Token?.ExpiresIn;
        [Category("Token")]
        public DateTime? ExpiresOn => account.Token?.ExpiresOn;
        [Category("Token")]
        public string[]? Roles => account.Token?.Roles;

        public override string ToString() => Identifier ?? string.Empty;
    }
}

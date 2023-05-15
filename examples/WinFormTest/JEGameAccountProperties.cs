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
        public DateTime? LastAccess => account.LastAccess;

        [Category("Profile")]
        public string? Username => account.Session?.Profile?.Username;
        [Category("Profile")]
        public string? UUID => account.Session?.Profile?.UUID;
        [Category("Profile")]
        public object? Skins => account.Session?.Profile?.Skins;
        [Category("Profile")]
        public object? Capes => account.Session?.Profile?.Capes;

        [Category("Token")]
        public string? TokenUsername => account.Session?.Token?.Username;
        [Category("Token")]
        public string? AccessToken => account.Session?.Token?.AccessToken;
        [Category("Token")]
        public string? TokenType => account.Session?.Token?.TokenType;
        [Category("Token")]
        public int? ExpiresIn => account.Session?.Token?.ExpiresIn;
        [Category("Token")]
        public DateTime? ExpiresOn => account.Session?.Token?.ExpiresOn;
        [Category("Token")]
        public string[]? Roles => account.Session?.Token?.Roles;

        public override string ToString() => Identifier ?? string.Empty;
    }
}

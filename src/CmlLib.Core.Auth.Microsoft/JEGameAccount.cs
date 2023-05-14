using CmlLib.Core.Auth.Microsoft.JE;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JEGameAccount : XboxGameAccount
    {
        public new static JEGameAccount FromSessionStorage(ISessionStorage sessionStorage)
        {
            return new JEGameAccount(sessionStorage);
        }

        private readonly JESessionSource _jeSessionSource;

        public JEGameAccount(ISessionStorage sessionStorage) : base(sessionStorage)
        {
            _jeSessionSource = new JESessionSource(sessionStorage);
        }

        public JESession? Session => _jeSessionSource.Get();
        public JEProfile? Profile => Session?.Profile;

        protected override string? GetIdentifier() => 
            Session?.Profile?.UUID;

        public MSession? ToLauncherSession() =>
            Session?.ToLauncherSession();
    }
}
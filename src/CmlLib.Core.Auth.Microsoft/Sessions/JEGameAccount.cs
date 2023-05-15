using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Sessions
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

        protected override string? GetIdentifier() => 
            Session?.Profile?.UUID;

        public MSession? ToLauncherSession() =>
            Session?.ToLauncherSession();
    }
}
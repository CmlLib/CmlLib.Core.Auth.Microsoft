using System.Text.Json;
using XboxAuthNet.Game;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    public class TestJsonSessionStorageCollection
    {
        public async Task a()
        {
            var loginHandler = LoginHandlerBuilder.Create().ForJavaEdition();

            foreach (var item in loginHandler.Accounts)
            {

            }

            var account = loginHandler.Accounts.GetAccount("identifier");

            var result = await loginHandler.AuthenticateInteractively()
                .WithSessionStorage(account.SessionStorage)
                .ExecuteForLauncherAsync();

            loginHandler.SaveAccounts();
        }
    }
}
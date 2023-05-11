using System.IO;
using System.Net.Http;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;

namespace CmlLib.Core.Auth.Microsoft
{
    public class JELoginHandlerBuilder
    {
        public static JELoginHandler BuildDefault() =>
            new JELoginHandlerBuilder().Build();

        private HttpClient? _httpClient;
        public HttpClient HttpClient
        {
            get => _httpClient ??= HttpHelper.DefaultHttpClient.Value;
            set => _httpClient = value;
        }

        private IXboxGameAccountManager? _accountManager;
        public IXboxGameAccountManager AccountManager
        {
            get => _accountManager ??= createDefaultAccountManager();
            set => _accountManager = value;
        }

        public JELoginHandlerBuilder WithHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
            return this;
        }

        public JELoginHandlerBuilder WithAccountManager(string filePath)
        {
            return WithAccountManager(createAccountManager(filePath));
        }

        public JELoginHandlerBuilder WithAccountManager(IXboxGameAccountManager accountManager)
        {
            AccountManager = accountManager;
            return this;
        }

        private IXboxGameAccountManager createDefaultAccountManager()
        {
            var defaultFilePath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_accounts.json");
            return createAccountManager(defaultFilePath);
        }

        private IXboxGameAccountManager createAccountManager(string filePath)
        {
            return new JsonXboxGameAccountManager(filePath, JEGameAccount.FromSessionStorage);
        }

        public JELoginHandler Build()
        {
            return new JELoginHandler(HttpClient, AccountManager);
        }
    }
}

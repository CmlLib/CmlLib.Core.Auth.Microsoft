using CmlLib.Core.Auth.Microsoft;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;

namespace WinFormTest
{
    public partial class AuthForm : Form
    {
        JELoginHandler loginHandler = JELoginWrapper.Instance.LoginHandler;
        IPublicClientApplication? msalApp;

        public AuthForm()
        {
            InitializeComponent();
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            selectDefaultLoginSettings();
            listAccounts();
        }

        private void selectDefaultLoginSettings()
        {
            cbLoginMode.SelectedIndex = 0;
            cbDevicePreset.SelectedIndex = 0;
            cbOAuthLoginMode.SelectedIndex = 0;
            txtScope.Text = XboxAuthConstants.XboxScope;
            cbXboxLoginMode.SelectedIndex = 0;
            txtXboxRelyingParty.Text = JELoginHandler.RelyingParty;
        }

        private void listAccounts()
        {
            var accounts = loginHandler.AccountManager.GetAccounts();
            cbAccounts.Items.Clear();
            foreach (var account in accounts)
            {
                cbAccounts.Items.Add(account.Identifier);
            }
            cbAccounts.Items.Add("<New Account>");
            cbAccounts.SelectedItem = "<New Account>";
        }

        private void cbDevicePreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbDevicePreset.Text)
            {
                case "MinecraftJavaEdition":
                    txtClientId.Text = XboxGameTitles.MinecraftJava;
                    txtDeviceType.Text = XboxDeviceTypes.Win32;
                    break;
                case "Nintendo":
                    txtClientId.Text = XboxGameTitles.MinecraftNintendoSwitch;
                    txtDeviceType.Text = XboxDeviceTypes.Nintendo;
                    break;
                case "iOS":
                    txtClientId.Text = XboxGameTitles.XboxAppIOS;
                    txtDeviceType.Text = XboxDeviceTypes.iOS;
                    break;
            }
            txtDeviceVersion.Text = "0.0.0";
        }

        private void cbOAuthLoginMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOAuthLoginMode.Text.Contains("Msal"))
            {
                cbAzure.Checked = true;
                txtClientId.Text = "";
            }
            else
            {
                cbAzure.Checked = false;
                txtClientId.Text = XboxGameTitles.MinecraftJava;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                IXboxGameAccount account;

                var selectedAccountIdentifier = cbAccounts.SelectedItem?.ToString();
                if (selectedAccountIdentifier == null || selectedAccountIdentifier == "<New Account>")
                    account = loginHandler.AccountManager.NewAccount();
                else
                    account = loginHandler.AccountManager.GetAccounts().GetAccount(selectedAccountIdentifier);

                await authenticate(cbLoginMode.Text, account);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

            this.Enabled = true;
        }

        private async Task authenticate(string loginMode, IXboxGameAccount account)
        {
            CompositeAuthenticator builder;

            var defaultOAuth = cbOAuthLoginMode.SelectedIndex == 0;
            
            if (loginMode == "JEAuthentication")
            {
                try
                {
                    await authenticate("SilentJEAuthentication", account);
                }
                catch (Exception)
                {
                    await authenticate("InteractiveJEAuthentication", account);
                }

                return;
            }
            else if (loginMode == "SilentJEAuthentication")
            {
                if (defaultOAuth)
                    cbOAuthLoginMode.SelectedItem = "SilentMicrosoftOAuth";
                builder = loginHandler.AuthenticateSilently(account);
            }
            else if (loginMode == "InteractiveJEAuthentication")
            {
                if (defaultOAuth)
                    cbOAuthLoginMode.SelectedItem = "InteractiveMicrosoftOAuth";
                builder = loginHandler.AuthenticateInteractively(account);
            }
            else
            {
                throw new InvalidOperationException("unknown loginMode");
            }

            await setAuthStrategies(builder);
            if (defaultOAuth)
                cbOAuthLoginMode.SelectedIndex = 0;
            
            var result = await builder.ExecuteAsync();
            MessageBox.Show(result.Profile?.Username);
        }

        private async Task setAuthStrategies(XboxGameAuthenticationBuilder<JESession> builder)
        {
            var strategy = cbOAuthLoginMode.Text;
            if (strategy.Contains("OAuth"))
            {
                var oauthClient = JELoginWrapper.Instance.GetOAuthClientInfo(txtClientId.Text);
                builder.WithMicrosoftOAuth(oauthClient, builder =>
                {
                    setMicrosoftOAuth(builder.MicrosoftOAuth, strategy);
                    setXboxAuth(builder.XboxAuth);
                });
            }
            else if (cbOAuthLoginMode.Text.Contains("Msal"))
            {
                msalApp = await JELoginWrapper.Instance.GetMsalAppAsync(txtClientId.Text);
                builder.WithMsalOAuth(builder =>
                {
                    setMsalOAuth(builder.MsalOAuth, strategy);
                    setXboxAuth(builder.XboxAuth);
                });
            }
        }

        private void setMicrosoftOAuth<T>(MicrosoftOAuthStrategyBuilder<T> builder, string strategy)
        {
            if (strategy == "InteractiveMicrosoftOAuth")
            {
                builder.UseInteractiveStrategy(new MicrosoftOAuthParameters
                {
                    Prompt = MicrosoftOAuthPromptModes.SelectAccount
                });
            }
            else if (strategy == "SilentMicrosoftOAuth")
            {
                builder.UseSilentStrategy();
            }
            else
            {
                throw new InvalidOperationException("Can't recognize OAuth option: " + strategy);
            }
        }

        private void setMsalOAuth<T>(MsalOAuthStrategyBuilder<T> builder, string strategy)
        {
            if (msalApp == null)
                throw new InvalidOperationException("initialize msalApp first");
            
            if (strategy == "InteractiveMsalOAuth")
            {
                builder.UseInteractiveStrategy(msalApp);
            }
            else if (strategy == "SilentMsalOAuth")
            {
                builder.UseSilentStrategy(msalApp);
            }
            else if (strategy == "DeviceCodeOAuth")
            {
                builder.UseDeviceCodeStrategy(msalApp, deviceCode =>
                {
                    var deviceCodeForm = new DeviceCodeForm();
                    deviceCodeForm.SetDeviceCodeResult(deviceCode);
                    deviceCodeForm.ShowDialog();
                    return Task.CompletedTask;
                });
            }
            else
            {
                throw new InvalidOperationException("Can't recognize OAuth option: " + strategy);
            }
        }

        private void setXboxAuth<T>(XboxAuthStrategyBuilder<T> builder)
        {
            var strategy = cbXboxLoginMode.Text;
            builder.WithDeviceType(txtDeviceType.Text);
            builder.WithDeviceVersion(txtDeviceVersion.Text);
            if (strategy == "Basic")
            {
                builder.UseBasicStrategy();
            }
            else if (strategy == "Full")
            {
                builder.UseFullStrategy();
            }
            else if (strategy == "Sisu")
            {
                builder.UseSisuStrategy(txtClientId.Text);
            }
        }
    }
}

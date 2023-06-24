using CmlLib.Core.Auth.Microsoft;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Msal;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using XboxAuthNet.Game.OAuth;
using CmlLib.Core.Auth.Microsoft.Authenticators;

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
            cbLoginPreset.SelectedIndex = 0;
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

        private void cbLoginPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLoginPreset.SelectedIndex == 0) // interactive
            {
                cbOAuthValidation.Checked = false;
                cbJEValidatation.Checked = false;
                cbAccounts.SelectedItem = "<New Account>";
                cbOAuthLoginMode.SelectedItem = "InteractiveMicrosoftOAuth";
            }
            else if (cbLoginPreset.SelectedIndex == 1) // silent
            {
                cbOAuthValidation.Checked = true;
                cbJEValidatation.Checked = true;
                cbAccounts.SelectedItem = JELoginWrapper.Instance.LoginHandler.AccountManager.GetDefaultAccount().Identifier;
                cbOAuthLoginMode.SelectedItem = "SilentMicrosoftOAuth";
            }
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

                await authenticate(account, default);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

            this.Enabled = true;
        }

        private async Task authenticate(IXboxGameAccount account, CancellationToken cancellationToken)
        {
            var authenticator = JELoginWrapper.Instance.LoginHandler.CreateAuthenticator(account, cancellationToken);
            addOAuth(authenticator);
            addXboxAuth(authenticator);
            addJEAuth(authenticator);
            var result = await authenticator.ExecuteForLauncherAsync();
            MessageBox.Show(result.Username);
        }

        private async void addOAuth(NestedAuthenticator authenticator)
        {
            var mode = cbOAuthLoginMode.Text;
            if (mode.Contains("OAuth"))
            {
                addMicrosoftOAuth(authenticator, mode);
            }
            else if (mode.Contains("Msal"))
            {
                await addMsalOAuth(authenticator, mode);
            }
        }

        private async Task addMsalOAuth(NestedAuthenticator authenticator, string mode)
        {
            msalApp = await JELoginWrapper.Instance.GetMsalAppAsync(txtClientId.Text);
            authenticator.AddMsalOAuth(msalApp, msal =>
            {
                if (mode == "InteractiveMsalOAuth")
                {
                    return msal.Interactive();
                }
                else if (mode == "SilentMsalOAuth")
                {
                    return msal.Silent();
                }
                else if (mode == "DeviceCodeOAuth")
                {
                    return msal.DeviceCode(deviceCode =>
                    {
                        var deviceCodeForm = new DeviceCodeForm();
                        deviceCodeForm.SetDeviceCodeResult(deviceCode);
                        deviceCodeForm.ShowDialog();
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    throw new InvalidOperationException("Can't recognize OAuth option: " + mode);
                }
            });
        }

        private void addMicrosoftOAuth(NestedAuthenticator authenticator, string mode)
        {
            var oauthClient = JELoginWrapper.Instance.GetOAuthClientInfo(txtClientId.Text);
            var oauthSelector = (MicrosoftOAuthBuilder oauth) =>
            {
                if (mode == "InteractiveMicrosoftOAuth")
                {
                    return oauth.Interactive();
                }
                else if (mode == "SilentMicrosoftOAuth")
                {
                    return oauth.Silent();
                }
                else
                {
                    throw new InvalidOperationException("Can't recognize OAuth option: " + mode);
                }
            };
            if (cbOAuthValidation.Checked)
            {
                authenticator.AddMicrosoftOAuthForJE(oauthSelector);
            }
            else
            {
                authenticator.AddForceMicrosoftOAuthForJE(oauthSelector);
            }
        }

        private void addXboxAuth(NestedAuthenticator authenticator)
        {
            var mode = cbXboxLoginMode.Text;
            var clientId = txtClientId.Text;
            authenticator.AddXboxAuthForJE(xbox =>
            {
                xbox.WithDeviceType(txtDeviceType.Text);
                xbox.WithDeviceVersion(txtDeviceVersion.Text);
                if (mode == "Basic")
                {
                    return xbox.Basic();
                }
                else if (mode == "Full")
                {
                    return xbox.Full();
                }
                else if (mode == "Sisu")
                {
                    return xbox.Sisu(clientId);
                }
                else
                {
                    throw new InvalidOperationException("Can't recognize XboxAuth option: " + mode);
                }
            });
        }

        private void addJEAuth(NestedAuthenticator authenticator)
        {
            var jeBuilder = (JEAuthenticatorBuilder builder) =>
            {
                if (cbJEGameOwnershipChecker.Checked)
                    builder.WithGameOwnershipChecker();
                return builder.Build();
            };

            if (cbJEValidatation.Checked)
            {
                authenticator.AddJEAuthenticator(jeBuilder);
            }
            else
            {
                authenticator.AddForceJEAuthenticator(jeBuilder);
            }
        }
    }
}

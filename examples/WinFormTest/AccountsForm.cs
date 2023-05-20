using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using CmlLib.Core.Auth.Microsoft.GameAuthenticators;
using XboxAuthNet.XboxLive;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.Msal;
using Microsoft.Identity.Client;

namespace WinFormTest
{
    public partial class AccountsForm : Form
    {
        public AccountsForm()
        {
            InitializeComponent();
        }

        private JELoginHandler? loginHandler;
        private MicrosoftOAuthClientInfo? oauthClient;
        private IPublicClientApplication? msalApp;

        private bool checkOAuthInitializingRequired()
        {
            return oauthClient?.ClientId != txtClientId.Text;
        }

        private void initializeOAuthClient()
        {
            oauthClient = new MicrosoftOAuthClientInfo
            {
                ClientId = txtClientId.Text,
                Scopes = JELoginHandler.DefaultMicrosoftOAuthClientInfo.Scopes
            };
        }

        private bool checkMsalInitializingRequired()
        {
            return msalApp?.AppConfig?.ClientId != txtClientId.Text;
        }

        private async Task initializeMsalApp()
        {
            msalApp = await MsalClientHelper.BuildApplicationWithCache(txtClientId.Text);
        }

        private void AccountsForm_Load(object sender, EventArgs e)
        {
            initializeLoginHandler();
            listAccounts();
            selectDefaultLoginSettings();
        }

        private void initializeLoginHandler()
        {
            loginHandler = JELoginHandlerBuilder.BuildDefault();
        }

        private void listAccounts()
        {
            if (loginHandler == null)
                throw new InvalidOperationException("loginHandler was not initialized");

            var accounts = loginHandler.GetAccounts();
            lbAccounts.Items.Clear();
            foreach (var account in accounts)
            {
                if (string.IsNullOrEmpty(account.Identifier))
                    continue;
                if (account is JEGameAccount jeAccount)
                    lbAccounts.Items.Add(new JEGameAccountProperties(jeAccount));
            }
        }

        private void lbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgAccount.SelectedObject = lbAccounts.SelectedItem;
        }

        private void selectDefaultLoginSettings()
        {
            cbLoginMode.SelectedIndex = 0;
            cbDevicePreset.SelectedIndex = 0;
            txtScope.Text = XboxAuthConstants.XboxScope;
            cbXboxLoginMode.SelectedIndex = 0;
            txtXboxRelyingParty.Text = JEAuthenticationApi.RelyingParty;
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

        private async void btnAddAccount_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            var loginMode = cbLoginMode.Text;
            await authenticate(loginMode);
            this.Enabled = true;
        }

        private async Task authenticate(string loginMode)
        {
            if (loginHandler == null)
                throw new InvalidOperationException("initialize loginHandler first");

            XboxGameAuthenticationBuilder<JESession> builder;

            if (loginMode == "JEAuthentication")
            {
                try 
                {
                    await authenticate("SilentJEAuthentication");
                }
                catch (Exception)
                {
                    await authenticate("InteractiveJEAuthentication");
                }

                return;
            }
            else if (loginMode == "SilentJEAuthentication")
            {
                builder = loginHandler.AuthenticateSilently();
            }
            else if (loginMode == "InteractiveJEAuthentication")
            {
                builder = loginHandler.AuthenticateInteractively();
            }
            else
            {
                throw new InvalidOperationException("unknown loginMode");
            }

            await setAuthStrategies(builder);

            var result = await builder.ExecuteAsync();
            setAuthenticationResult(result);
        }

        private void setAuthenticationResult(JESession result)
        {
            MessageBox.Show(result.Profile?.Username);
            listAccounts();
        }

        private async Task setAuthStrategies(XboxGameAuthenticationBuilder<JESession> builder)
        {
            if (cbOAuthLoginMode.Text.Contains("OAuth"))
            {
                if (checkOAuthInitializingRequired())
                    initializeOAuthClient();

                builder.WithMicrosoftOAuth(oauthClient!, builder => 
                {
                    setMicrosoftOAuth(builder.MicrosoftOAuth);
                    setXboxAuth(builder.XboxAuth);
                });
            }
            else if (cbOAuthLoginMode.Text.Contains("Msal"))
            {
                if (checkMsalInitializingRequired())
                    await initializeMsalApp();

                builder.WithMsalOAuth(builder => 
                {
                    setMsalOAuth(builder.MsalOAuth);
                    setXboxAuth(builder.XboxAuth);
                });
            }
        }

        private void setMicrosoftOAuth<T>(MicrosoftOAuthStrategyBuilder<T> builder)
        {
            var strategy = cbOAuthLoginMode.Text;
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
        }

        private void setMsalOAuth<T>(MsalOAuthStrategyBuilder<T> builder)
        {
            if (msalApp == null)
                throw new InvalidOperationException();

            var strategy = cbOAuthLoginMode.Text;
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

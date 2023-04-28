using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using XboxAuthNet.Game;
using XboxAuthNet.XboxLive;

namespace WinFormTest
{
    public partial class AccountsForm : Form
    {
        public AccountsForm()
        {
            InitializeComponent();
        }

        private JELoginHandler? loginHandler;

        private void AccountsForm_Load(object sender, EventArgs e)
        {
            initializeLoginHandler();
            listAccounts();
            selectDefaultLoginSettings();
        }

        private void initializeLoginHandler()
        {
            loginHandler = LoginHandlerBuilder.Create().ForJavaEdition();
        }

        private void listAccounts()
        {
            if (loginHandler == null)
                throw new InvalidOperationException("loginHandler was not initialized");

            var accounts = loginHandler.GetAccounts();
            foreach (var account in accounts)
            {
                if (string.IsNullOrEmpty(account.Identifier))
                    continue;
                lbAccounts.Items.Add(new JEGameAccountProperties(account));
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
            cbOAuthLoginMode.SelectedIndex = 0;
            txtScope.Text = XboxAuthConstants.XboxScope;
            cbXboxLoginMode.SelectedIndex = 0;
            txtXboxRelyingParty.Text = JEAuthenticationApi.RelyingParty;
        }

        private void cbLoginMode_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            switch (cbOAuthLoginMode.Text)
            {
                case "InteractiveMicrosoftOAuth":
                    break;
                case "SilentMicrosoftOAuth":
                    break;
                case "InteractiveMsal":
                    break;
                case "DeviceCodeMsal":
                    break;
                case "SilentMsal":
                    break;
            }
        }

        private void cbXboxLoginMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbXboxLoginMode.Text)
            {
                case "Basic":
                    break;
                case "Full":
                    break;
                case "Sisu":
                    break;
            }
        }

        private async void btnAddAccount_Click(object sender, EventArgs e)
        {
            var result = await loginHandler.AuthenticateInteractively()
                .ExecuteAsync();
            loginHandler.Save();
            MessageBox.Show(result.Profile?.Username);
            listAccounts();
        }
    }
}

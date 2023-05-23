using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using CmlLib.Core.Auth.Microsoft.GameAuthenticators;
using XboxAuthNet.XboxLive;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.Msal;
using Microsoft.Identity.Client;
using CmlLib.Core.Auth;

namespace WinFormTest
{
    public partial class AccountsForm : Form
    {
        public AccountsForm()
        {
            InitializeComponent();
        }

        private void AccountsForm_Load(object sender, EventArgs e)
        {
            listAccounts();
        }

        private void listAccounts()
        {
            var accounts = JELoginWrapper.Instance.LoginHandler.GetAccounts();
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

        private async void btnAddAccount_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            try
            {
                var loginHandler = JELoginWrapper.Instance.LoginHandler;
                var session = await loginHandler.AuthenticateInteractively()
                    .WithInteractiveMicrosoftOAuth(builder => builder
                        .MicrosoftOAuth.UseInteractiveStrategy(new MicrosoftOAuthParameters
                        {
                            Prompt = MicrosoftOAuthPromptModes.SelectAccount
                        })
                        .XboxAuth.UseBasicStrategy())
                    .ExecuteForLauncherAsync();
                successLogin(session);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            this.Enabled = true;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var selectedAccountIdentifier = lbAccounts.SelectedItem?.ToString();
            if (selectedAccountIdentifier == null)
            {
                MessageBox.Show("Select account to login");
                return;
            }

            this.Enabled = false;

            try
            {
                var loginHandler = JELoginWrapper.Instance.LoginHandler;
                var selectedAccount = loginHandler.GetAccounts().GetAccount(selectedAccountIdentifier);
                var session = await loginHandler.Authenticate(selectedAccount);
                successLogin(session);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            this.Enabled = true;
        }

        private async void btnRemoveAccount_Click(object sender, EventArgs e)
        {
            var selectedAccountIdentifier = lbAccounts.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedAccountIdentifier))
            {
                MessageBox.Show("Select account to delete");
                return;
            }

            var loginHandler = JELoginWrapper.Instance.LoginHandler;
            var selectedAccount = loginHandler.GetAccounts().GetAccount(selectedAccountIdentifier);
            await loginHandler.Signout(selectedAccount);

            listAccounts();
        }

        private void btnAdvOptions_Click(object sender, EventArgs e)
        {
            var form = new AuthForm();
            form.ShowDialog();
            listAccounts();
        }

        private void successLogin(MSession session)
        {
            MessageBox.Show(session.Username);
            listAccounts();
        }
    }
}

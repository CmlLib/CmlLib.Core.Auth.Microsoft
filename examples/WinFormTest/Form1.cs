using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Msal;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.XboxLive;
using System.ComponentModel;
using Microsoft.Identity.Client;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        IPublicClientApplication? _msalApp;
        JELoginHandler _loginHandler;
        MSession? _session;

        readonly string msalClientId = "499c8d36-be2a-4231-9ebd-ef291b7bb64c";
        bool msalMode = false;

        public Form1()
        {
            _loginHandler = LoginHandlerBuilder.Create().ForJavaEdition();
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            setUIEnable(false);
            cbPresets.SelectedIndex = 0;

            try
            {
                var session = await authenticateSilently();
                loginSuccess(session);
            }
            catch (Exception ex)
            {
                setUIEnable(true);
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            setUIEnable(false);

            try
            {
                MSession session;
                if (cbSisuAuth.Checked)
                {
                    session = await authenticateInteractivelyWithSisu();
                }
                else
                {
                    session = await authenticateInteractively();
                }
                loginSuccess(session);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                setUIEnable(true);
            }
        }

        private async Task<MSession> authenticateSilently()
        {
            var session = await _loginHandler.AuthenticateSilently()
                .ExecuteForLauncherAsync();
            return session;
        }

        private async Task<MSession> authenticateInteractively()
        {
            var clientInfo = getMicrosoftOAuthClientInfo();
            var session = await _loginHandler.AuthenticateInteractively()
                .WithMicrosoftOAuth(clientInfo, builder => builder
                    .MicrosoftOAuth.UseInteractiveStrategy(builder => builder
                        .WithUIParent(this))
                    .XboxAuth.WithTokenPrefix(cbAzure.Checked ? XboxAuthConstants.AzureTokenPrefix : ""))
                .ExecuteForLauncherAsync();
            return session;
        }

        private async Task<MSession> authenticateInteractivelyWithSisu()
        {
            var clientInfo = getMicrosoftOAuthClientInfo();
            var session = await _loginHandler.AuthenticateInteractively()
                .WithMicrosoftOAuth(clientInfo, builder => builder
                    .MicrosoftOAuth.UseInteractiveStrategy(builder => builder
                        .WithUIParent(this))
                    .XboxAuth.WithDeviceType(txtDeviceType.Text)
                    .XboxAuth.WithDeviceVersion(txtDeviceVersion.Text)
                    .XboxAuth.WithTokenPrefix(cbAzure.Checked ? XboxAuthConstants.AzureTokenPrefix : XboxAuthConstants.XboxTokenPrefix))
                .ExecuteForLauncherAsync();
            return session;
        }

        private async Task<MSession> msalAuthenticateSilently()
        {
            var app = await getMsalApp();
            var session = await _loginHandler.AuthenticateSilently()
                .WithMsalOAuth(builder => builder
                    .MsalOAuth.UseSilentStrategy(app))
                .ExecuteForLauncherAsync();
            return session;
        }

        private async Task<MSession> msalAuthenticateInteractively()
        {
            var app = await getMsalApp();
            var session = await _loginHandler.AuthenticateInteractively()
                .WithMsalOAuth(builder => builder
                    .MsalOAuth.UseInteractiveStrategy(app))
                .ExecuteForLauncherAsync();
            return session;
        }

        private async Task<MSession> msalAuthenticateWithDeviceCode()
        {
            var deviceCodeForm = new DeviceCodeForm();
            var handler = new Func<DeviceCodeResult, Task>(result => 
            {
                var tcs = new TaskCompletionSource();
                deviceCodeForm.Invoke(() => 
                {
                    deviceCodeForm.SetDeviceCodeResult(result);
                    deviceCodeForm.ShowDialog();
                    tcs.SetResult();
                });
                return tcs.Task;
            });

            var app = await getMsalApp();
            var session = await _loginHandler.AuthenticateInteractively()
                .WithMsalOAuth(builder => builder
                    .MsalOAuth.UseDeviceCodeStrategy(app, handler))
                .ExecuteForLauncherAsync();
            return session;
        }

        private async Task<IPublicClientApplication> getMsalApp()
        {
            return _msalApp ??= await createMsalApp();
        }

        private async Task<IPublicClientApplication> createMsalApp()
        {
            return await MsalClientHelper.BuildApplicationWithCache(msalClientId);
        }

        private MicrosoftOAuthClientInfo getMicrosoftOAuthClientInfo()
        {
            return new MicrosoftOAuthClientInfo()
            {
                ClientId = txtClientId.Text,
                Scopes = cbAzure.Checked ? "XboxLive.signin" : XboxAuthConstants.XboxScope
            };
        }

        private void setUIEnable(bool value)
        {
            btnLogin.Enabled = value;
            btnLogout.Enabled = value;
            btnStart.Enabled = value;
        }

        private void loginSuccess(MSession session)
        {
            this._session = session;
            btnLogin.Enabled = false;
            btnLogout.Enabled = true;
            txtAccessToken.Text = session.AccessToken;
            txtUsername.Text = session.Username;
            txtUUID.Text = session.UUID;
            btnStart.Enabled = true;
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            await this._loginHandler.CreateSignout()
                .AddMicrosoftOAuthSignout()
                .ExecuteAsync();

            txtAccessToken.Clear();
            txtUsername.Clear();
            txtUUID.Clear();
            btnStart.Enabled = false;
            btnLogin.Enabled = true;
            btnLogout.Enabled = true;
            MessageBox.Show("Done");
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            btnLogout.Enabled = false;
            btnStart.Enabled = false;

            var path = new MinecraftPath(); // default game path
            var launcher = new CMLauncher(path);

            // register event handlers
            launcher.FileChanged += Launcher_FileChanged;
            launcher.ProgressChanged += Launcher_ProgressChanged;

            // check and download game files
            var process = await launcher.CreateProcessAsync("1.19.2", new MLaunchOption
            {
                Session = this._session,
                ServerIp = "mc.hypixel.net",
                MaximumRamMb = 4096
            });

            // start game
            process.Start();
        }

        private void Launcher_FileChanged(CmlLib.Core.Downloader.DownloadFileChangedEventArgs e)
        {
            progressBar1.Maximum = e.TotalFileCount;
            progressBar1.Value = e.ProgressedFileCount;
            lbStatus.Text = e.FileName;
        }

        private void Launcher_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
        }

        private void cbPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDeviceVersion.Text = "0.0.0";

            switch (cbPresets.Text)
            {
                case "Win32":
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
                default:
                    return;
            }

            cbAzure.Checked = false;
        }
    }
}

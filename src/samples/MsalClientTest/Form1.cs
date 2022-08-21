using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;

namespace MsalClientTest
{
    public partial class Form1 : Form
    {
        // Use your own client id
        private readonly string ClientID = "499c8d36-be2a-4231-9ebd-ef291b7bb64c";

        public Form1()
        {
            InitializeComponent();
            loginCancel = new CancellationTokenSource();
        }

        IPublicClientApplication? app;
        JavaEditionLoginHandler? loginHandler;
        MSession? session;
        CancellationTokenSource loginCancel;

        private async void Form1_Shown(object sender, EventArgs e)
        {
            app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(ClientID);
            setLoginButtonEnabled(true);
            btnStart.Enabled = false;
        }

        private async void btnLoginInteractive_Click(object sender, EventArgs e)
        {
            if (app == null) return;

            lbStatus.Text = "CreateInteractiveApi(), LoginFromOAuth()";
            loginHandler = new JavaEditionLoginHandlerBuilder()
                .WithMsalOAuth(app, factory => factory.CreateInteractiveApi())
                .Build();

            await LoginAndShowResultOnUI(loginHandler);
        }

        private async void btnLoginInteractiveEmb_Click(object sender, EventArgs e)
        {
            if (app == null) return;

            lbStatus.Text = "CreateWithEmbeddedWebView(), LoginFromOAuth()";
            loginHandler = new JavaEditionLoginHandlerBuilder()
                .WithMsalOAuth(app, factory => factory.CreateWithEmbeddedWebView())
                .Build();

            await LoginAndShowResultOnUI(loginHandler);
        }

        private async void btnLoginDeviceCode_Click(object sender, EventArgs e)
        {
            if (app == null) return;

            lbStatus.Text = "CreateDeviceCodeApi(), LoginFromOAuth()";
            var deviceCodeForm = new DeviceCodeForm();

            loginHandler = new JavaEditionLoginHandlerBuilder()
                .WithMsalOAuth(app, factory => factory.CreateDeviceCodeApi(result =>
                {
                    Invoke(() =>
                    {
                        deviceCodeForm.SetDeviceCodeResult(result);
                        deviceCodeForm.Show();
                    });

                    return Task.CompletedTask;
                }))
                .Build();

            await LoginAndShowResultOnUI(loginHandler);
            deviceCodeForm.Close();
        }

        private async Task LoginAndShowResultOnUI(JavaEditionLoginHandler loginHandler)
        {
            setLoginButtonEnabled(false);

            try
            {
                var session = await loginHandler.LoginFromOAuth();
                loginSuccess(session.GameSession);
            }
            catch (Exception ex)
            {
                lbStatus.Text += ": Fail";
                MessageBox.Show(ex.ToString());
                setLoginButtonEnabled(true);
            }
        }

        private void btnCancelLogin_Click(object sender, EventArgs e)
        {
            loginCancel?.Cancel();
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            lbStatus.Text = "Removing accounts";
            if (loginHandler != null)
                await loginHandler.ClearCache();

            txtAccessToken.Clear();
            txtUUID.Clear();
            txtUsername.Clear();

            setLoginButtonEnabled(true);

            btnStart.Enabled = false;
            session = null;

            lbStatus.Text = "Logout success";
            MessageBox.Show("Logout success");
        }

        private void loginSuccess(MSession session)
        {
            lbStatus.Text = "Login success";
            this.session = session;
            txtAccessToken.Text = session.AccessToken;
            txtUUID.Text = session.UUID;
            txtUsername.Text = session.Username;

            btnStart.Enabled = true;

            this.BringToFront();
            this.Activate();
        }

        private void setLoginButtonEnabled(bool value)
        {
            btnLoginDeviceCode.Enabled = value;
            btnLoginInteractive.Enabled = value;
            btnLoginInteractiveEmb.Enabled = value;
        }
        private async void btnStart_Click(object sender, EventArgs e)
        {
            lbStatus.Text = "Starting game";
            btnStart.Enabled = false;

            var launcher = new CMLauncher(new MinecraftPath());
            launcher.FileChanged += Launcher_FileChanged;
            launcher.ProgressChanged += Launcher_ProgressChanged;
            var process = await launcher.CreateProcessAsync("1.18.1", new MLaunchOption
            {
                Session = session,
                ServerIp = "mc.hypixel.net"
            });
            process.Start();

            lbStatus.Text = "Game started";
        }

        private void Launcher_ProgressChanged(object? sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar2.Maximum = 100;
            progressBar2.Value = e.ProgressPercentage;
        }

        private void Launcher_FileChanged(CmlLib.Core.Downloader.DownloadFileChangedEventArgs e)
        {
            lbFileName.Text = e.FileName;
            progressBar1.Maximum = e.TotalFileCount;
            progressBar1.Value = e.ProgressedFileCount;
        }
    }
}
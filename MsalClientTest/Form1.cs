using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;
using System.Reflection;

namespace MsalClientTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IPublicClientApplication? app;
        MSession? session;
        CancellationTokenSource? loginCancel;

        private async void Form1_Load(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            btnStart.Enabled = false;

            lbStatus.Text = "Building Application";
            app = await MsalMinecraftLoginHelper.BuildApplicationWithCache("499c8d36-be2a-4231-9ebd-ef291b7bb64c");

            lbStatus.Text = "LoginSilent()";
            try
            {
                var session = await MsalMinecraftLoginHelper.LoginSilent(app);
                loginSuccess(session);
            }
            catch (Exception)
            {
                lbStatus.Text = "Login Required";
                btnLogin.Enabled = true;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lbStatus.Text = "Start login";
            btnLogin.Enabled = false;
            try
            {
                loginCancel = new CancellationTokenSource();
                var session = await MsalMinecraftLoginHelper.Login(app, loginCancel.Token, useEmbeddedWebView: true);
                loginSuccess(session);
            }
            catch (Exception ex)
            {
                btnLogin.Enabled = true;
                lbStatus.Text = "Login failed";

                if (ex is MsalClientException msalEx)
                    MessageBox.Show("MsalClientException: " + msalEx.ErrorCode);
                else if (ex.InnerException is MsalClientException msalInEx)
                    MessageBox.Show("MsalClientException: " + msalInEx.ErrorCode);
                else
                    MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelLogin_Click(object sender, EventArgs e)
        {
            loginCancel?.Cancel();

        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            lbStatus.Text = "Removing accounts";
            await MsalMinecraftLoginHelper.RemoveAccounts(app);
            txtAccessToken.Clear();
            txtUUID.Clear();
            txtUsername.Clear();
            btnLogin.Enabled = true;
            btnStart.Enabled = false;
            session = null;

            lbStatus.Text = "Logout success";
        }

        private void loginSuccess(MSession session)
        {
            lbStatus.Text = "Login success";
            this.session = session;
            txtAccessToken.Text = session.AccessToken;
            txtUUID.Text = session.UUID;
            txtUsername.Text = session.Username;

            btnStart.Enabled = true;
            btnLogin.Enabled = false;

            this.Focus();
            MessageBox.Show("Login success!");
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
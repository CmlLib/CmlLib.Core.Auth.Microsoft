using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Reflection;

namespace MsalClientTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Use your own client id
        private readonly string ClientID = "499c8d36-be2a-4231-9ebd-ef291b7bb64c";

        MsalMinecraftLoginHandler? handler;
        MSession? session;
        CancellationTokenSource? loginCancel;

        private async void Form1_Shown(object sender, EventArgs e)
        {
            setLoginButtonEnabled(false);
            btnStart.Enabled = false;

            lbStatus.Text = "Building Application";

            // Initialize login handler
            if (handler == null)
            {
                var app = await MsalMinecraftLoginHelper.BuildApplicationWithCache(ClientID);
                handler = new MsalMinecraftLoginHandler(app);
            }

            lbStatus.Text = "LoginSilent()";
            try
            {
                var session = await handler.LoginSilent();
                loginSuccess(session);
            }
            catch (Exception)
            {
                lbStatus.Text = "Login Required";
                setLoginButtonEnabled(true);
            }
        }

        private async void btnLoginInteractive_Click(object sender, EventArgs e)
        {
            if (handler == null)
                return;
            setLoginButtonEnabled(false);

            lbStatus.Text = "LoginInteractive()";
            try
            {
                loginCancel = new CancellationTokenSource();
                var session = await handler.LoginInteractive(loginCancel?.Token);
                loginSuccess(session);
            }
            catch (Exception ex)
            {
                lbStatus.Text += ": Fail";
                MessageBox.Show(ex.ToString());
                setLoginButtonEnabled(true);
            }
        }

        private async void btnLoginInteractiveEmb_Click(object sender, EventArgs e)
        {
            if (handler == null)
                return;
            setLoginButtonEnabled(false);

            lbStatus.Text = "LoginInteractive(useEmbeddedWebView: true)";
            try
            {
                loginCancel = new CancellationTokenSource();
                var session = await handler.LoginInteractive(loginCancel?.Token, useEmbeddedWebView: true);
                loginSuccess(session);
            }
            catch (Exception ex)
            {
                lbStatus.Text += ": Fail";
                MessageBox.Show(ex.ToString());
                setLoginButtonEnabled(true);
            }
        }

        private async void btnLoginDeviceCode_Click(object sender, EventArgs e)
        {
            if (handler == null)
                return;
            setLoginButtonEnabled(false);

            lbStatus.Text = "LoginDeviceCode()";
            try
            {
                var deviceCodeForm = new DeviceCodeForm();
                var session = await handler.LoginDeviceCode(result =>
                {
                    Invoke(() =>
                    {
                        deviceCodeForm.SetDeviceCodeResult(result);
                        deviceCodeForm.Show();
                    });
                    
                    return Task.CompletedTask;
                });

                try
                {
                    deviceCodeForm.Close();
                }
                catch { }

                loginSuccess(session);
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
            if (handler != null)
                await handler.RemoveAccounts();

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
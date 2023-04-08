﻿using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using XboxAuthNet.Utils;
using XboxAuthNet.XboxLive;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        JavaEditionLoginHandler? _loginHandler;
        MSession? _session;

        public Form1()
        {
            InitializeComponent();
            btnStart.Enabled = false;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .Build();
            
            setUIEnable(false);

            cbPresets.SelectedIndex = 0;

            try
            {
                var result = await loginHandler.LoginFromCache();
                loginSuccess(result.GameSession);
            }
            catch (Exception ex)
            {
                setUIEnable(true);
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var scope = cbAzure.Checked ?
                "XboxLive.signin" :
                XboxAuth.XboxScope;

            this._loginHandler = new LoginHandlerBuilder()
                .WithClientId(txtClientId.Text)
                .ForJavaEdition()
                .WithMicrosoftOAuthApi(builder => builder
                    .WithScope(scope)
                    .WithWebUI(new WebView2WebUI(this)))
                .With((builder, context) =>
                {
                    if (cbSisuAuth.Checked)
                    {
                        var keyGenerator = KeyPairGeneratorFactory.CreateDefaultAsymmetricKeyPair();
                        builder.WithXboxSisuAuthApi(builder => builder
                            .WithECKeyPairGenerator(keyGenerator)
                            .WithDeviceType(txtDeviceType.Text)
                            .WithDeviceVersion(txtDeviceVersion.Text)
                            .WithTokenPrefix(cbAzure.Checked ? XboxSecureAuth.AzureTokenPrefix : XboxSecureAuth.XboxTokenPrefix));
                    }
                    else
                    {
                        builder.WithXboxLiveApi(
                            new XboxAuthNetApi(
                            new XboxAuth(context.HttpClient), 
                            cbAzure.Checked ? "d=" : null, null, null));
                    }
                })
                .Build();

            setUIEnable(false);

            try
            {
                var result = await this._loginHandler.LoginFromOAuth();
                loginSuccess(result.GameSession);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                setUIEnable(true);
            }
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
            if (this._loginHandler == null)
                throw new InvalidOperationException("_loginHandler was null");

            await this._loginHandler.ClearCache();
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

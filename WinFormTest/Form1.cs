using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        MSession Session;

        public Form1()
        {
            InitializeComponent();
            btnStart.Enabled = false;
        }

        private async Task<MicrosoftLoginForm> CreateForm()
        {
            var loginHandler = new LoginHandler();
            MicrosoftLoginForm form = new MicrosoftLoginForm(loginHandler);

            //var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CmlLib.Core.Auth.Microsoft.UI.WinForm.TestApp");
            //form.WebView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: dataPath);

            return form;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var form = await CreateForm();

            try
            {
                MSession session = await form.ShowLoginDialog(); // show login form
                this.Session = session;

                txtAccessToken.Text = session.AccessToken;
                txtUsername.Text = session.Username;
                txtUUID.Text = session.UUID;
                btnStart.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            MicrosoftLoginForm form = await CreateForm();
            form.ShowLogoutDialog(); // show logout form

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
            var process = await launcher.CreateProcessAsync("1.18.2", new MLaunchOption
            {
                Session = this.Session,
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
    }
}

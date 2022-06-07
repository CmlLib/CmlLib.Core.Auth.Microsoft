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
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<Form1> logger;

        MSession Session;

        public Form1()
        {
            loggerFactory = LoggerFactory.Create(conf =>
            {
                conf.AddFilter(level => level >= LogLevel.Trace);
                conf.AddSimpleConsole();
                conf.AddDebug();
            });
            logger = loggerFactory.CreateLogger<Form1>();
            logger.LogTrace("LogTrace ready");

            InitializeComponent();
            btnStart.Enabled = false;
        }

        private async Task<MicrosoftLoginForm> CreateForm()
        {
            var loginHandler = new LoginHandler(builder =>
            {
                builder.SetLogger(loggerFactory);
            });
            MicrosoftLoginForm form = new MicrosoftLoginForm(loginHandler);

            //var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CmlLib.Core.Auth.Microsoft.UI.WinForm.TestApp");
            //form.WebView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: dataPath);

            return form;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var form = await CreateForm();

            // localize message
            // 한글화
            //form.MessageStrings = new Dictionary<string, string>
            //{
            //    ["mslogin_fail"] = "마이크로소프트 로그인 실패",
            //    ["mstoken_null"] = "mstoken이 null입니다",
            //    ["xbox_error_child"] = "미성년자 계정입니다. 성인인증을 하거나 가족 계정에 추가하세요",
            //    ["xbox_error_noaccount"] = "Xbox 계정을 찾을 수 없습니다",
            //    ["mctoken_null"] = "mctoken이 null입니다",
            //    ["mojang_nogame"] = "Minecraft JE를 구매하지 않았습니다"
            //};

            MSession session = await form.ShowLoginDialog(); // show login form

            if (session != null) // login success
            {
                this.Session = session;

                txtAccessToken.Text = session.AccessToken;
                txtUsername.Text = session.Username;
                txtUUID.Text = session.UUID;
                btnStart.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to login");
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

        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
        }
    }
}

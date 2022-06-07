using CmlLib.Core.Auth.Microsoft.Mojang;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.UI.WinForm
{
    public partial class MicrosoftLoginForm : Form
    {
        public MicrosoftLoginForm()
            : this(new LoginHandler())
        {

        }

        public MicrosoftLoginForm(LoginHandler handler)
        {
            this.LoginHandler = handler;
            InitializeComponent();
        }

        public Dictionary<string, string> MessageStrings { get; set; } = new Dictionary<string, string>
        {
            ["mslogin_fail"] = "Failed to Microsoft login",
            ["xboxlogin_fail"] = "Failed to Xbox login",
            ["mclogin_fail"] = "Failed to Minecraft login",
            ["xbox_error_child"] = "Your account seems like a child. Verify your age or add your account into a Family.",
            ["xbox_error_noaccount"] = "Your account doens't have an Xbox account",
            ["mojang_nogame"] = "You don't have a Minecraft JE",
            ["mojang_noprofile"] = "No Minecraft JE profile",
            ["empty_token"] = "Token was empty",
            ["empty_userhash"] = "UserHash was empty",
            ["no_error_msg"] = "No error message"
        };

        public string LoadingText
        {
            get => lbLoading.Text;
            set => lbLoading.Text = value;
        }
        
        public CoreWebView2Environment? WebView2Environment { get; set; }

        protected MSession? Session { get; set; }
        protected string? ActionName { get; private set; }
        protected LoginHandler LoginHandler { get; private set; }

        public async Task<MSession?> ShowLoginDialog()
        {
            try
            {
                // try to get session without UI
                var session = await LoginHandler.LoginFromCache();
                if (session == null)
                {
                    ActionName = "login"; // need UI
                    this.ShowDialog();
                }

                return session;
            }
            catch (Exception ex)
            {
                if (!OnException(ex)) throw;
                return null;
            }
        }

        public void ShowLogoutDialog()
        {
            ActionName = "logout";
            this.ShowDialog();
        }

        private async void Window_Loaded(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ActionName))
            {
                throw new InvalidOperationException("Use ShowLoginDialog() or ShowLogoutDialog()");
            }
            else if (ActionName == "login")
            {
                await login();
            }
            else if (ActionName == "logout")
            {
                await signout();
            }
            else
            {
                throw new InvalidOperationException(ActionName);
            }

            ActionName = null;
            Session = null;
        }

        WebView2? wv;
        #region Create/Remove WebView2 control

        protected virtual async Task InitializeWebView2(WebView2 wv)
        {
            await wv.EnsureCoreWebView2Async(WebView2Environment);
        }

        // Show webview on form
        private async Task createWv()
        {
            wv = new WebView2();
            wv.NavigationStarting += Wv_NavigationStarting;
            wv.Dock = DockStyle.Fill;
            this.Controls.Add(wv);
            this.Controls.SetChildIndex(wv, 0);
            await InitializeWebView2(wv);
        }

        // Remove webview on form
        private void removeWv()
        {
            if (wv != null)
            {
                try
                {
                    this.Controls.Remove(wv);
                    //wv.Dispose();
                    wv = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        #endregion


        private async Task login()
        {
            var url = LoginHandler.CreateOAuthUrl(); // oauth
            await createWv();
            wv.Source = new Uri(url);
        }

        private async void Wv_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.IsRedirected && LoginHandler.CheckOAuthLoginSuccess(e.Uri)) // microsoft browser login success
            {
                removeWv(); // remove webview control
                try
                {
                    this.Session = await LoginHandler.LoginFromOAuth();
                }
                catch (Exception ex)
                {
                    if (!OnException(ex)) throw;
                }
                this.Close();
            }
        }

        private async Task signout()
        {
            LoginHandler.ClearCache();

            await createWv(); // show webview control
            wv.Source = new Uri(MicrosoftOAuth.GetSignOutUrl());
        }

        protected virtual bool OnException(Exception ex)
        {
            string msg = "";

            switch (ex)
            {
                case MicrosoftOAuthException msEx:
                    msg =
                        $"{GetMessage("mslogin_fail")} : {msEx.Error}\n" +
                        $"ErrorDescription : {msEx.ErrorDescription}\n" +
                        $"ErrorCodes : {string.Join(",", msEx.ErrorCodes ?? new int[0])}";
                    break;
                case XboxAuthException xboxEx:
                    msg =
                        $"{GetMessage("xboxlogin_fail")} : {xboxEx.ErrorCode}\n" +
                        $"{GetMessage(xboxEx.Message)}";
                    break;
                case MinecraftAuthException mcEx:
                    msg =
                        $"{GetMessage("mclogin_fail")} : {GetMessage(mcEx.Message)}";
                    break;
                case ArgumentNullException _:
                    msg = ex.Message + " was null";
                    break;
                default:
                    return false;
            }

            msg += "\n" + ex.StackTrace;

            MessageBox.Show(msg);
            this.Session = null;
            this.Close();
            return true;
        }

        protected string GetMessage(string? key)
        {
            if (MessageStrings.TryGetValue(key ?? "", out string? value))
                return value;
            else
                return key ?? "";
        }

        private void MicrosoftLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            removeWv(); // remove webview control
        }
    }
}

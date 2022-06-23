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

        public string LoadingText
        {
            get => lbLoading.Text;
            set => lbLoading.Text = value;
        }
        
        public CoreWebView2Environment? WebView2Environment { get; set; }

        private Exception? Error { get; set; }
        private MSession? Session { get; set; }
        private string? ActionName { get; set; }
        protected LoginHandler LoginHandler { get; private set; }

        public async Task<MSession> ShowLoginDialog()
        {
            try
            {
                return await LoginHandler.LoginFromCache();
            }
            catch (Exception ex) when (
                ex is MicrosoftOAuthException ||
                ex is XboxAuthException ||
                ex is MinecraftAuthException)
            {
                ActionName = "login"; // need UI
                this.ShowDialog();

                if (Error != null)
                    throw Error;

                if (this.Session == null)
                    throw new LoginCancelledException("User cancelled login");

                return this.Session;
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

            this.ActionName = null;
            this.Session = null;
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
            if (e.IsRedirected && LoginHandler.CheckOAuthCodeResult(new Uri(e.Uri), out var authCode)) // microsoft browser login success
            {
                removeWv(); // remove webview control
                //this.Hide();

                if (authCode.IsSuccess)
                {
                    try
                    {
                        this.Session = await LoginHandler.LoginFromOAuth();
                    }
                    catch (Exception ex)
                    {
                        this.Error = ex;
                    }
                }
                else
                {
                    this.Error = new LoginCancelledException(authCode.Error + ", " + authCode.ErrorDescription);
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

        private void MicrosoftLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            removeWv(); // remove webview control
        }
    }
}

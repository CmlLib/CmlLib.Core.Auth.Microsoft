using CmlLib.Core.Mojang;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            this.loginHandler = handler;
            InitializeComponent();
        }

        public Dictionary<string, string> MessageStrings = new Dictionary<string, string>
        {
            ["mslogin_fail"] = "Failed to microsoft login",
            ["xbox_error_child"] = "Your account seems like a child. Verify your age or add your account into a Family.",
            ["xbox_error_noaccount"] = "Your account doens't have an Xbox account",
            ["mojang_nogame"] = "You don't have a Minecraft JE"
        };

        public string LoadingText
        {
            get => lbLoading.Text;
            set => lbLoading.Text = value;
        }
        
        private MSession Session;
        private string ActionName;
        private LoginHandler loginHandler;

        public MSession ShowLoginDialog()
        {
            ActionName = "login";
            this.ShowDialog();
            return this.Session;
        }

        public void ShowLogoutDialog()
        {
            ActionName = "logout";
            this.ShowDialog();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ActionName))
            {
                throw new InvalidOperationException("Use ShowLoginDialog() or ShowLogoutDialog()");
            }
            else if (ActionName == "login")
            {
                login();
            }
            else if (ActionName == "logout")
            {
                signout();
            }
            else
            {
                throw new NotImplementedException(ActionName);
            }

            ActionName = null;
            Session = null;
        }

        WebView2 wv;
        #region Create/Remove WebView2 control

        // Show webview on form
        private void CreateWV()
        {
            wv = new WebView2();
            wv.NavigationStarting += Wv_NavigationStarting;
            wv.Dock = DockStyle.Fill;
            this.Controls.Add(wv);
            this.Controls.SetChildIndex(wv, 0);
        }

        // Remove webview on form
        private void RemoveWV()
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


        private void login()
        {
            new Thread(() =>
            {
                try
                {
                    this.Session = loginHandler.LoginFromCache();
                    Invoke(new Action(() =>
                    {
                        if (this.Session == null)
                        {
                            var url = loginHandler.CreateOAuthUrl(); // oauth
                            CreateWV();
                            wv.Source = new Uri(url);
                        }
                        else
                            this.Close();
                    }));
                }
                catch (Exception ex)
                {
                    ErrorClose(ex);
                }
            }).Start();
        }

        private void Wv_NavigationStarting(object sender, global::Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.IsRedirected && loginHandler.CheckOAuthLoginSuccess(e.Uri)) // microsoft browser login success
            {
                RemoveWV(); // remove webview control

                new Thread(() =>
                {
                    try
                    {
                        this.Session = loginHandler.LoginFromOAuth();
                        Invoke(new Action(() =>
                        {
                            this.Close();
                        }));
                    }
                    catch (Exception ex)
                    {
                        ErrorClose(ex);
                    }
                }).Start();
            }
        }

        private void signout()
        {
            loginHandler.ClearCache();

            CreateWV(); // show webview control
            wv.Source = new Uri(MicrosoftOAuth.GetSignOutUrl());
        }

        private void ErrorClose(string msg)
        {
            BeginInvoke(new Action(() =>
            {
                MessageBox.Show(msg);
                this.Session = null;
                this.Close();
            }));
        }

        private void ErrorClose(Exception ex)
        {
            BeginInvoke(new Action(() =>
            {
                string msg = "";

                switch (ex)
                {
                    case MicrosoftOAuthException msEx:
                        msg =
                            $"{l("mslogin_fail")} : {msEx.Error}\n" +
                            $"ErrorDescription : {msEx.ErrorDescription}\n" +
                            $"ErrorCodes : {string.Join(",", msEx.ErrorCodes)}";
                        break;
                    case XboxAuthException xboxEx:
                        msg =
                            $"{l("mclogin_fail")} : {xboxEx.Message}";
                        break;
                    case ArgumentNullException _:
                        msg = ex.Message + " was null";
                        break;
                    default:
                        msg = l(ex.Message);
                        break;
                }
                
                MessageBox.Show(msg);
                this.Session = null;
                this.Close();
            }));
        }

        private string l(string key)
        {
            if (MessageStrings.TryGetValue(key, out string value))
                return value;
            else
                return key;
        }

        private void MicrosoftLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveWV(); // remove webview control
        }
    }
}

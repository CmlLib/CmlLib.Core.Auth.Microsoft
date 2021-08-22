using CmlLib.Core.Mojang;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.UI.Wpf
{
    /// <summary>
    /// MicrosoftLoginWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MicrosoftLoginWindow : Window
    {
        public MicrosoftLoginWindow()
            : this(new LoginHandler())
        {

        }

        public MicrosoftLoginWindow(LoginHandler handler)
        {
            this.loginHandler = handler;
            InitializeComponent();
        }

        public Dictionary<string, string> MessageStrings = new Dictionary<string, string>
        {
            ["mslogin_fail"] = "Failed to microsoft login",
            ["mclogin_fail"] = "Failed to minecraft login",
            ["xbox_error_child"] = "Your account seems like a child. Verify your age or add your account into a Family.",
            ["xbox_error_noaccount"] = "Your account doens't have an Xbox account",
            ["mojang_nogame"] = "You don't have a Minecraft JE",
            ["empty_token"] = "Token was empty",
            ["empty_userhash"] = "UserHash was empty",
            ["no_error_msg"] = "No error message"
        };

        public static DependencyProperty LoadingTextProperty = 
            DependencyProperty.Register(
                nameof(LoadingText),
                typeof(string),
                typeof(MicrosoftLoginWindow),
                new PropertyMetadata("Microsoft Login\n     Loading"));

        public string LoadingText
        {
            get => (string)GetValue(LoadingTextProperty);
            set => SetValue(LoadingTextProperty, value);
        }
        
        private MSession session;
        private string actionName;
        private readonly LoginHandler loginHandler;

        public MSession ShowLoginDialog()
        {
            actionName = "login";
            this.ShowDialog();
            return this.session;
        }

        public void ShowLogoutDialog()
        {
            actionName = "logout";
            this.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                throw new InvalidOperationException("Use ShowLoginDialog() or ShowLogoutDialog()");
            }
            else if (actionName == "login")
            {
                login();
            }
            else if (actionName == "logout")
            {
                signout();
            }
            else
            {
                throw new InvalidOperationException(actionName);
            }

            actionName = null;
            session = null;
        }

        WebView2 wv;
        #region Create/Remove WebView2 control

        // Show webview on form
        private void createWv()
        {
            wv = new WebView2();
            wv.NavigationStarting += Wv_NavigationStarting;
            grid.Children.Add(wv);
        }

        // Remove webview on form
        private void removeWv()
        {
            if (wv != null)
            {
                grid.Children.Remove(wv);
                wv.Dispose();
                wv = null;
            }
        }

        #endregion

        private void login()
        {
            new Thread(() =>
            {
                try
                {
                    this.session = loginHandler.LoginFromCache();
                    Dispatcher.Invoke(() =>
                    {
                        if (this.session == null)
                        {
                            var url = loginHandler.CreateOAuthUrl(); // oauth
                            createWv();
                            wv.Source = new Uri(url);
                        }
                        else
                            this.Close();
                    });
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
                removeWv(); // remove webview control

                new Thread(() =>
                {
                    try
                    {
                        this.session = loginHandler.LoginFromOAuth();
                        Dispatcher.Invoke(() =>
                        {
                            this.Close();
                        });
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

            createWv(); // show webview control
            wv.Source = new Uri(MicrosoftOAuth.GetSignOutUrl());
        }

        private void ErrorClose(string msg)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(msg);
                this.session = null;
                this.Close();
            }));
        }

        private void ErrorClose(Exception ex)
        {
            Dispatcher.BeginInvoke(new Action(() =>
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
                this.session = null;
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            removeWv(); // remove webview control
        }
    }
}

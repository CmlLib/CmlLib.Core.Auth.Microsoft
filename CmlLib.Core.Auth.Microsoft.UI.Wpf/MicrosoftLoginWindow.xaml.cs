using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
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

        public CoreWebView2Environment? WebView2Environment { get; set; }

        protected MSession? Session { get; set; }
        protected string? ActionName { get; private set; }
        protected LoginHandler LoginHandler { get; private set; }

        public MSession? ShowLoginDialog()
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
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

        // Show webview on form
        protected virtual async Task<WebView2> InitializeWebView2()
        {
            var wv = new WebView2();
            await wv.EnsureCoreWebView2Async(WebView2Environment);
            return wv;
        }

        private async Task createWv()
        {
            wv = await InitializeWebView2();
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
                    this.Session = LoginHandler.LoginFromCache();
                    Dispatcher.Invoke(async () =>
                    {
                        if (this.Session == null)
                        {
                            var url = LoginHandler.CreateOAuthUrl(); // oauth
                            await createWv();
                            wv.Source = new Uri(url);
                        }
                        else
                            this.Close();
                    });
                }
                catch (Exception ex)
                {
                    bool result = true;
                    Dispatcher.Invoke(() =>
                    {
                        result = OnException(ex);
                    });
                    if (!result)
                        throw;
                }
            }).Start();
        }

        private void Wv_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.IsRedirected && LoginHandler.CheckOAuthLoginSuccess(e.Uri)) // microsoft browser login success
            {
                removeWv(); // remove webview control

                new Thread(() =>
                {
                    try
                    {
                        this.Session = LoginHandler.LoginFromOAuth();
                        Dispatcher.Invoke(() =>
                        {
                            this.Close();
                        });
                    }
                    catch (Exception ex)
                    {
                        bool result = true;
                        Dispatcher.Invoke(() =>
                        {
                            result = OnException(ex);
                        });
                        if (!result)
                            throw;
                    }
                }).Start();
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
                        $"{GetMessage("xboxlogin_fail")} : {GetMessage(xboxEx.Message)}";
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            removeWv(); // remove webview control
        }
    }
}

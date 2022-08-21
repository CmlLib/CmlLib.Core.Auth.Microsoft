using CmlLib.Core.Auth.Microsoft.Mojang;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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

            browserTimeoutTimer = new DispatcherTimer();
            browserTimeoutTimer.Tick += BrowserTimeoutTimer_Tick;

            InitializeComponent();
        }

        private void BrowserTimeoutTimer_Tick(object? sender, EventArgs e)
        {
            browserTimeoutTimer.Stop();
            this.Error = new WebView2RuntimeNotFoundException();
            this.Close();
        }

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
        public int BrowserTimeout { get; set; } = 10 * 1000;

        private readonly DispatcherTimer browserTimeoutTimer;
        private Exception? Error { get; set; }
        private MSession? Session { get; set; }
        private string? ActionName { get; set; }
        private LoginHandler LoginHandler { get; set; }

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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
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

        // Show webview on form
        protected virtual async Task InitializeWebView2(WebView2 wv)
        {
            await wv.EnsureCoreWebView2Async(WebView2Environment);
        }

        private async Task<WebView2> createWv()
        {
            wv = new WebView2();
            grid.Children.Add(wv);
            wv.NavigationStarting += Wv_NavigationStarting;
            await InitializeWebView2(wv);

            browserTimeoutTimer.Interval = TimeSpan.FromMilliseconds(BrowserTimeout);
            browserTimeoutTimer.Start();

            return wv;
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

        private async Task login()
        {
            var url = LoginHandler.CreateOAuthUrl(); // oauth
            var wv = await createWv();
            wv.Source = new Uri(url);
        }

        private async void Wv_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            browserTimeoutTimer.Stop();
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
            await LoginHandler.ClearCache();

            var wv = await createWv(); // show webview control
            wv.Source = new Uri(MicrosoftOAuth.GetSignOutUrl());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            removeWv(); // remove webview control
        }
    }
}

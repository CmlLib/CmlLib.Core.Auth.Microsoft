﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;
using Microsoft.Web.WebView2.Core;
using Microsoft.Extensions.Logging;
using CmlLib.Core.Auth.Microsoft;
using XboxAuthNet.OAuth;
using System.Net.Http;
using CmlLib.Core.Version;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<MainWindow> logger;

        public MainWindow()
        {
            loggerFactory = LoggerFactory.Create(conf =>
            {
                conf.AddFilter(level => level >= LogLevel.Trace);
                conf.AddSimpleConsole();
                conf.AddDebug();
            });
            logger = loggerFactory.CreateLogger<MainWindow>();
            logger.LogTrace("LogTrace ready");

            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var httpClient = new HttpClient();

            var loginHandler = new LoginHandler(builder =>
            {
                builder.SetMicrosoftOAuthHandler("0000000048093EE3", "service::user.auth.xboxlive.com::MBI_SSL");
                builder.SetLogger(loggerFactory);
            });
            //loginHandler.RelyingParty = "http://xboxlive.com";

            //var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CmlLib.Core.Auth.Microsoft.UI.WinForm.TestApp");

            MicrosoftLoginWindow window = new MicrosoftLoginWindow(loginHandler);
            //window.WebView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: dataPath);
            MSession session = await window.ShowLoginDialog();

            if (session != null) // login success
            {
                lbAccessToken.Content = session.AccessToken;
                lbUsername.Content = session.Username;
                lbUUID.Content = session.UUID;
            }
            else
            {
                MessageBox.Show("failed to login");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftLoginWindow window = new MicrosoftLoginWindow();
            window.ShowLogoutDialog();

            MessageBox.Show("Done");
        }
    }
}

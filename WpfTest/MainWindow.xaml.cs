using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftLoginWindow window = new MicrosoftLoginWindow();
            MSession session = window.ShowLoginDialog();

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

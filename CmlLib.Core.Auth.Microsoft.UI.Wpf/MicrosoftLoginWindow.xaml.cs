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
        {
            InitializeComponent();
        }

        public Dictionary<string, string> MessageStrings = new Dictionary<string, string>
        {
            ["mslogin_fail"] = "Failed to microsoft login",
            ["mstoken_null"] = "mstoken was null",
            ["xbox_error_child"] = "Your account seems like a child. Verify your age or add your account into a Family.",
            ["xbox_error_noaccount"] = "Your account doens't have an Xbox account",
            ["mctoken_null"] = "mctoken was null",
            ["mojang_nogame"] = "You don't have a Minecraft JE"
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

        private string ClientId = "00000000402B5328";
        private MSession Session;
        private string ActionName;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
            grid.Children.Add(wv);
        }

        // Remove webview on form
        private void RemoveWV()
        {
            if (wv != null)
            {
                grid.Children.Remove(wv);
                wv.Dispose();
                wv = null;
            }
        }

        #endregion

        #region Microsoft token cache

        string microsoftOAuthPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_msa.json");

        // read microsoft token cache file
        private MicrosoftOAuthResponse readMicrosoft()
        {
            if (!File.Exists(microsoftOAuthPath))
                return null;

            var file = File.ReadAllText(microsoftOAuthPath);
            var response = JsonConvert.DeserializeObject<MicrosoftOAuthResponse>(file);

            return response;
        }

        // write microsoft login cache file
        private void writeMicrosoft(MicrosoftOAuthResponse response)
        {
            var json = JsonConvert.SerializeObject(response);
            File.WriteAllText(microsoftOAuthPath, json);
        }

        #endregion

        #region Minecraft token cache

        string minecraftTokenPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_token.json");

        // read minecraft login cache file
        private AuthenticationResponse readMinecraft()
        {
            if (!File.Exists(minecraftTokenPath))
                return null;

            var file = File.ReadAllText(minecraftTokenPath);
            var job = JObject.Parse(file);

            this.Session = job["session"].ToObject<MSession>();
            return job["auth"].ToObject<AuthenticationResponse>();
        }

        // write minecraft login cache file
        private void writeMinecraft(AuthenticationResponse mcToken)
        {
            var obj = new
            {
                auth = mcToken,
                session = Session
            };
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(minecraftTokenPath, json);
        }

        #endregion

        MicrosoftOAuth oauth;

        private void login()
        {
            new Thread(() =>
            {
                try
                {
                    oauth = new MicrosoftOAuth(this.ClientId, XboxAuth.XboxScope);

                    var msToken = readMicrosoft();
                    var mcToken = readMinecraft();

                    if (mcToken == null || DateTime.Now > mcToken.ExpiresOn) // invalid mc session
                    {
                        if (!oauth.TryGetTokens(out msToken, msToken?.RefreshToken)) // failed to refresh ms
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var url = oauth.CreateUrl(); // oauth
                                CreateWV();
                                wv.Source = new Uri(url);
                            }));
                        }
                        else // success to refresh ms
                        {
                            writeMicrosoft(msToken);
                            getMojangToken(msToken);
                        }
                    }
                    else // valid minecraft token
                        SaveSession(mcToken);
                }
                catch (Exception ex)
                {
                    ErrorClose(ex);
                }
            }).Start();
        }

        private void Wv_NavigationStarting(object sender, global::Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.IsRedirected && oauth.CheckLoginSuccess(e.Uri)) // microsoft browser login success
            {
                RemoveWV(); // remove webview control

                new Thread(() =>
                {
                    var result = oauth.TryGetTokens(out MicrosoftOAuthResponse response); // get token
                    if (result)
                    {
                        try
                        {
                            writeMicrosoft(response);
                            getMojangToken(response);
                        }
                        catch (Exception ex)
                        {
                            ErrorClose(ex);
                        }
                    }
                    else
                    {
                        ErrorClose(
                            $"{l("mslogin_fail")} : {response.Error}\n" +
                            $"ErrorDescription : {response.ErrorDescription}\n" +
                            $"ErrorCodes : {string.Join(",", response.ErrorCodes)}");
                    }
                }).Start();
            }
        }

        private void getMojangToken(MicrosoftOAuthResponse response)
        {
            // get minecraft token
            var mcToken = mcLogin(response);
            SaveSession(mcToken);
        }

        private void SaveSession(AuthenticationResponse mcToken)
        {
            if (this.Session == null)
                this.Session = getSession(mcToken); // goto 6
            writeMinecraft(mcToken);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Close();
            }));
        }

        private AuthenticationResponse mcLogin(MicrosoftOAuthResponse msToken)
        {
            if (msToken == null)
                throw new ArgumentNullException(l("mstoken_null"));

            var xbox = new XboxAuth();
            var rps = xbox.ExchangeRpsTicketForUserToken(msToken.AccessToken);
            var xsts = xbox.ExchangeTokensForXSTSIdentity(rps.Token, null, null, XboxMinecraftLogin.RelyingParty, null);

            if (!xsts.IsSuccess)
            {
                var message = createXboxExceptionMessage(xsts);
                throw new XboxAuthException(message, null);
            }

            var mclogin = new XboxMinecraftLogin();
            var mcToken = mclogin.LoginWithXbox(xsts.UserHash, xsts.Token);
            return mcToken;
        }

        private string createXboxExceptionMessage(XboxAuthResponse xsts)
        {
            var msg = "";
            if (xsts.Error == XboxAuthResponse.ChildError || xsts.Error == "2148916236")
                msg = l("xbox_error_child");
            else if (xsts.Error == XboxAuthResponse.NoXboxAccountError)
                msg = l("xbox_error_noaccount");

            string errorCode;
            try
            {
                var errorInt = long.Parse(xsts.Error.Trim());
                errorCode = errorInt.ToString("x");
            }
            catch
            {
                errorCode = xsts.Error;
            }

            return $"{l("xbox_fail")}: {errorCode}\n{xsts.Message}\n{msg}";
        }

        private MSession getSession(AuthenticationResponse mcToken)
        {
            // 6. get minecraft profile (username, uuid)

            if (mcToken == null)
                throw new ArgumentNullException(l("mctoken_null"));

            if (!MojangAPI.CheckGameOwnership(mcToken.AccessToken))
                throw new InvalidOperationException(l("mojang_nogame"));

            var profile = MojangAPI.GetProfileUsingToken(mcToken.AccessToken);
            return new MSession
            {
                AccessToken = mcToken.AccessToken,
                UUID = profile.UUID,
                Username = profile.Name
            };
        }

        private void signout()
        {
            writeMicrosoft(null);
            writeMinecraft(null);

            CreateWV(); // show webview control
            wv.Source = new Uri(MicrosoftOAuth.GetSignOutUrl());
        }

        private void ErrorClose(string msg)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(msg);
                this.Session = null;
                this.Close();
            }));
        }

        private void ErrorClose(Exception ex)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(ex.Message);

                if (!(ex is ArgumentNullException))
                    MessageBox.Show(ex.ToString());

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RemoveWV(); // remove webview control
        }
    }
}

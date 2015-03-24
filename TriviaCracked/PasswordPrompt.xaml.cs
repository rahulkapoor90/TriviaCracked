using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using Windows.System;

namespace TriviaCracked
{
    public partial class PasswordPrompt : UserControl
    {
        private UIElement oldContent;
        private IsolatedStorageSettings settings;
        public delegate void callback(string loginResult);
        callback cb;

        public PasswordPrompt(UIElement _oldContent, callback _cb)
        {
            InitializeComponent();
            oldContent = _oldContent;
            cb = _cb;
            settings = IsolatedStorageSettings.ApplicationSettings;

            try
            {
                emailText.Text = (string)settings["email"];
                passwordBox.Password = (string)settings["password"];
                rememberCheckBox.IsChecked = (bool)settings["checkbox"];
            }
            catch (KeyNotFoundException)
            {
                settings.Add("email", "");
                settings.Add("password", "");
                settings.Add("checkbox", false);
            }
        }

        public void exit(string session)
        {
            this.Content = oldContent;
            cb(session);
        }

        public void doneLogin(string result)
        {
            loginProgress.Visibility = System.Windows.Visibility.Collapsed;
            if (result == "error")
            {
                loginResult.Text = "Incorrect Email/Password, Please try again.";
            }
            else
            {
                exit(result);
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailText.Text;
            string password = passwordBox.Password;
            if (rememberCheckBox.IsChecked.Value)
            {
                //store the creds locally and then auto fill when this page is loaded
                try
                {
                    settings["email"] = email;
                    settings["password"] = password;
                    settings["checkbox"] = rememberCheckBox.IsChecked.Value;
                }
                catch (KeyNotFoundException)
                {
                    settings.Add("email", email);
                    settings.Add("password", password);
                    settings.Add("checkbox", rememberCheckBox.IsChecked.Value);
                }
            }

            CrackApi.login(email, password, doneLogin);

            loginProgress.Visibility = System.Windows.Visibility.Visible;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("http://broinformation.blogspot.in/2015/03/trvia-cracked-windows-phone-tutorial.html", UriKind.Absolute));
        }
    }
}

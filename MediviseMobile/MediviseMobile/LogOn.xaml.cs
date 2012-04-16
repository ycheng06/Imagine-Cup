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
using Microsoft.Phone.Controls;
using MediviseMobile.Authentication;
using System.Diagnostics;

namespace MediviseMobile
{
    public partial class LogOn : PhoneApplicationPage
    {
        public LogOn()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationServiceClient authService = new AuthenticationServiceClient();
            App.AuthenticationToken = new CookieContainer();
            authService.CookieContainer = App.AuthenticationToken;
            authService.LoginCompleted += authService_LoginCompleted;
            authService.LoginAsync(UsernameBox.Text, PasswordBox.Text, "", true);
           
        }

        private void authService_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Login failed");
            }
            else
            {
                AuthenticationServiceClient authService = (AuthenticationServiceClient)sender;
                App.AuthenticationToken = authService.CookieContainer;
                Debug.WriteLine("Ok");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

    }
}
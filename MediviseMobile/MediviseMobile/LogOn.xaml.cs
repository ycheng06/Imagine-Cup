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
using MediviseMobile.AuthenticationService;
using System.Diagnostics;

namespace MediviseMobile
{
    public partial class LogOn : PhoneApplicationPage
    {
        public LogOn()
        {
            InitializeComponent();
        }

        private CookieContainer cc;
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationServiceClient authService = new AuthenticationServiceClient();
            cc = new CookieContainer();
            authService.CookieContainer = cc;
            authService.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(authService_LoginCompleted);
            authService.LoginAsync(UsernameBox.Text, PasswordBox.Text, "", true);
           
        }

        private void authService_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            //if (e.Error != null)
            //{
            //    MessageBox.Show("Login failed");
            //}
            //else
            //{
            //    Debug.WriteLine("Ok");
            //}
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
            }
            else if (e.Result == false)
            {
                MessageBox.Show("error with login, try again");
            }
            else
            {
                Debug.WriteLine("Login successful");
                var y = cc;
                var x = cc.Count.ToString();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            }
        }

    }
}
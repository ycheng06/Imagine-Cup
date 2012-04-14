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

namespace MediviseMobile
{
    public partial class PatientTestUserControl : UserControl
    {
        public PatientTestUserControl()
        {
            InitializeComponent();
        }

        private void TestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestListBox.SelectedIndex == -1) return;

            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("/TestEdit.xaml?selectedIndex=" + TestListBox.SelectedIndex, UriKind.Relative));

            TestListBox.SelectedIndex = -1;
        }
    }
}

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
    public partial class PatientMessageUserControl : UserControl
    {
        public PatientMessageUserControl()
        {
            InitializeComponent();
        }
     private void MessageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageListBox.SelectedIndex == -1) return;

            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("/MessageEdit.xaml?selectedIndex=" + MessageListBox.SelectedIndex, UriKind.Relative));

            MessageListBox.SelectedIndex = -1;
        } 
    }
}

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
    public partial class PatientDrugUserControl : UserControl
    {
        public PatientDrugUserControl()
        {
            InitializeComponent();
        }

        private void DrugListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrugListBox.SelectedIndex == -1) return;

            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("/DrugEdit.xaml?selectedIndex=" + DrugListBox.SelectedIndex, UriKind.Relative));

            DrugListBox.SelectedIndex = -1;
        }
    }
}

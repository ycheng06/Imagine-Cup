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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace MediviseMobile
{
    public partial class PatientListUserControl : UserControl
    {
        public PatientListUserControl()
        {
            InitializeComponent();
        }

        //Handle selection changed on ListBox
        private void SecondListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If selected index is -1 (no selection) do nothing
            if (SecondListBox.SelectedIndex == -1) return;

            int id = App.ViewModel.Patients[SecondListBox.SelectedIndex].PatientId;
            string firstName = App.ViewModel.Patients[SecondListBox.SelectedIndex].FirstName;
            string lastName = App.ViewModel.Patients[SecondListBox.SelectedIndex].LastName;
            //Navigate to new page
            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("/PatientPage.xaml?id=" + id +
                "&firstName=" + firstName + "&lastName=" + lastName, UriKind.Relative));

            //Reset selected index to -1 (no selection)
            SecondListBox.SelectedIndex = -1;
        }

    }
}

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
using System.Diagnostics;

namespace MediviseMobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if(!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadAlert();
            }
        }

        //load pivot on demand
        private void PivotSelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(Pivot1.SelectedIndex.ToString());
            switch (Pivot1.SelectedIndex)
            {
                case 0:
                    App.ViewModel.LoadAlert();
                    break;
                case 1:
                    App.ViewModel.LoadPatient();
                    break;
            }
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
            NavigationService.Navigate(new Uri("/PatientPage.xaml?id=" + id + 
                "&firstName=" + firstName + "&lastName=" + lastName, UriKind.Relative));

            //Reset selected index to -1 (no selection)
            SecondListBox.SelectedIndex = -1;
        }

        private void AppBarRefresh_Click(object sender, EventArgs e)
        {
            //Reload data from OData Service
            App.ViewModel.Refresh();
        }


    }
}
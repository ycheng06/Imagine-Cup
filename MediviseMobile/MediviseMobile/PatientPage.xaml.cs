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
using MediviseMobile.Medivise;
using Microsoft.Phone.Shell;

namespace MediviseMobile
{
    public partial class PatientPage : PhoneApplicationPage
    {
        private int id;
        public PatientPage()
        {
            InitializeComponent();
            DataContext = App.PatientViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string patientId = "";
            if (NavigationContext.QueryString.TryGetValue("id", out patientId))
            {
                string firstName = NavigationContext.QueryString["firstName"];
                string lastName = NavigationContext.QueryString["lastName"];
                string fullName = String.Format("{0} {1}", firstName, lastName);
                this.Pivot2.Title = fullName.ToUpper();
                id = int.Parse(patientId);
                //App.PatientViewModel.LoadPatient(id);
            }
            base.OnNavigatedTo(e);
        }

        private void OnLoadingPivotItem(Object sender, PivotItemEventArgs e)
        {
            if(e.Item.Content != null) return;

            Pivot p = (Pivot)sender;

            if (e.Item == p.Items[0])
            {
                App.PatientViewModel.LoadPatient(id);
                e.Item.Content = new PatientProfileUserControl();
            }
            else if (e.Item == p.Items[1])
            {
                App.PatientViewModel.LoadDrug(id);
                e.Item.Content = new PatientDrugUserControl();
            }
            else if (e.Item == p.Items[2])
            {
                App.PatientViewModel.LoadTest(id);
                e.Item.Content = new PatientTestUserControl();
            }
            else if (e.Item == p.Items[3])
            {
                App.PatientViewModel.LoadMessage(id);
                e.Item.Content = new PatientMessageUserControl();
            }
        }

        private void PivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["ProfileAppBar"]);
                    ApplicationBarIconButton refresh = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                    refresh.Click += AppBarRefresh_Click;
                    ApplicationBarIconButton edit = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
                    edit.Click += AppBarEdit_Click;
                    break;
                default:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["DefaultAppBar"]);
                    ApplicationBarIconButton refresh2 = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                    refresh2.Click += AppBarRefresh_Click;
                    break;
            }
        }

        private void AppBarRefresh_Click(object sender, EventArgs e)
        {
            App.PatientViewModel.Refresh(id, this.Pivot2.SelectedIndex);
        }

        private void AppBarEdit_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ProfileEdit.xaml", UriKind.Relative));
        }
    }
}
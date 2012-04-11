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
                App.PatientViewModel.LoadPatient(id);
            }
            base.OnNavigatedTo(e);
        }

        //private void PivotSelectionChanged(Object sender, SelectionChangedEventArgs e)
        //{
        //    switch (Pivot2.SelectedIndex)
        //    {
        //        case 0:
        //            App.PatientViewModel.LoadPatient(id);
        //            break;
        //        case 1:
        //            break;
        //        case 2:
        //            break;
        //        case 3:
        //            break;
        //    }
        //}

        private void LoadingPivotItem(Object sender, PivotItemEventArgs e)
        {
            if(e.Item.Content != null) return;

            Pivot p = (Pivot)sender;

            if (e.Item == p.Items[0])
            {
                App.PatientViewModel.LoadPatient(id);
            }
            else if (e.Item == p.Items[1])
            {
            }
            else if (e.Item == p.Items[2])
            {
            }
            else if (e.Item == p.Items[3])
            { 
            }


            
        }

        private void AppBarRefresh_Click(object sender, EventArgs e)
        {
            App.PatientViewModel.Refresh(id);
        }

        private void AppBarSave_Click(object sender, EventArgs e)
        {
            App.PatientViewModel.SaveChanges();
        }

    }
}
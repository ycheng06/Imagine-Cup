using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Windows;
using MediviseMobile.Medivise;
using System.Diagnostics;


namespace MediviseMobile
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //Defines the root URI of the data service
        //URI of the public, Medivise data service
        private static Uri rootUri =
            new Uri("http://eosimaginecup.cloudapp.net/MediviseService.svc/");

        //Define the typed DataServiceContext.
        private MediviseEntities context = null;

        //Define the binding collection for Patients and alerts.
        private DataServiceCollection<Patient> patients;
        private DataServiceCollection<Alert> alerts;

        public MainViewModel()
        {
            context = new MediviseEntities(rootUri);
        }

        //Gets and sets the collection of Patient objects from teh feed.
        //This collection is used to bind to the UI
        public DataServiceCollection<Patient> Patients
        {
            get { return patients; }

            private set
            {
                patients = value;

                //Register a handler for LoadCompleted callback
                patients.LoadCompleted += OnPatientsLoaded;

                //Raise the PropertyChanged evetns.
                NotifyPropertyChanged("Patients");
            }
        }
        
        public DataServiceCollection<Alert> Alerts
        {
            get { return alerts; }

            private set
            {
                alerts = value;

                //Register a handler for LoadCompleted callback
                alerts.LoadCompleted += OnAlertsLoaded;

                //Raise the PropertyChanged evetns.
                NotifyPropertyChanged("Alerts");
            }
        }

        //used to determine whether the data is loaded.
        public bool IsDataLoaded { get; private set; }

        public void LoadPatient()
        {
            patients = new DataServiceCollection<Patient>(context);
            //specify an OData query taht returns all patients
            var query = from p in context.Patients
                        orderby p.FirstName
                        select p;

            patients.LoadAsync(query);
        }

        

        //Loads data when the application is initialized
        public void LoadData()
        {
            Debug.WriteLine("loading Data");
            //Instantiate the context and binding collection.
            context = new MediviseEntities(rootUri);
            //LoadPatient();
            LoadAlert();
        }

        public void LoadAlert()
        {
            Debug.WriteLine(context.ToString());
            alerts = new DataServiceCollection<Alert>(context);

            var query = from a in context.Alerts.Expand("Patient").Expand("AlertType")
                        orderby a.AlertDate
                        select a;
            Debug.WriteLine(query.ToString());
            alerts.LoadAsync(query);
        }

        //Displays data from teh stored data context and binding collection
        public void LoadData(MediviseEntities c, DataServiceCollection<Patient> patients,
            DataServiceCollection<Alert> alerts)
        {
            context = c;
            Patients = patients;
            Alerts = alerts;

            IsDataLoaded = true;
        }

        //Handles the DataServiceCollection<T>.LoadCompleted event.
        private void OnPatientsLoaded(object sender, LoadCompletedEventArgs e)
        {
            //Make sure that we load all pages of the Customers feed.
            if (Patients.Continuation != null)
            {
                Patients.LoadNextPartialSetAsync();
            }
            
            IsDataLoaded = true;
        }

        private void OnAlertsLoaded(object sender, LoadCompletedEventArgs e)
        {
            
            //Make sure that we load all pages of the Customers feed.
            if (Alerts.Continuation != null)
            {
                Alerts.LoadNextPartialSetAsync();
            }
            IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*
         * Maintaining Page STate
         */
        public string SaveState()
        {
           
            if (App.ViewModel.IsDataLoaded)
            {
                //create  a new dictionary to store binding collections
                var collections = new Dictionary<string, object>();

                //Add current Patients and Alert to binding collection
                collections["Patients"] = patients;
                collections["Alerts"] = alerts;

                //return the serialized context and bindling collection
                return DataServiceState.Serialize(context, collections);
            }
            else
            {
                return string.Empty;
            }
        }

        public void RestoreState(string appState)
        {
            //create a dictionary to hold any stored binding collections.
            Dictionary<string, object> collections;

            if (!string.IsNullOrEmpty(appState))
            {
                // Deserialize the DataServiceState object.
                DataServiceState state
                    = DataServiceState.Deserialize(appState);

                // Restore the context and binding collections.
                var context = state.Context as MediviseEntities;
                collections = state.RootCollections;

                // Get the binding collection of Patient and Alert objects.
                DataServiceCollection<Patient> customers
                    = collections["Customers"] as DataServiceCollection<Patient>;
                DataServiceCollection<Alert> alerts
                    = collections["Alerts"] as DataServiceCollection<Alert>;


                // Initialize the application with stored data. 
                App.ViewModel.LoadData(context, customers, alerts);
            }
        }

        
    }
}
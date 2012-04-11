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
    public class PatientViewModel : INotifyPropertyChanged
    {
        private static Uri rootUri =
            new Uri("http://eosimaginecup.cloudapp.net/MediviseService.svc/");

        private MediviseEntities context = null;

        private DataServiceCollection<Patient> patients;

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

        public bool IsDataLoaded { get; private set; }

        public void LoadPatient(int patientId)
        {
            Debug.WriteLine("loading Data");
            //Instantiate the context and binding collection.
            context = new MediviseEntities(rootUri);
            Patients = new DataServiceCollection<Patient>(context);

            var query = from p in context.Patients
                        where p.PatientId == patientId
                        select p;
            Debug.WriteLine(query.ToString());
            Patients.LoadAsync(query);
        }

        public void LoadData(MediviseEntities c, DataServiceCollection<Patient> patients)
        {
            context = c;
            Patients = patients;

            IsDataLoaded = true;
        }

        private void OnPatientsLoaded(object sender, LoadCompletedEventArgs e)
        {
            //Make sure that we load all pages of the Customers feed.
            Debug.WriteLine("patient loaded");
            if (Patients.Continuation != null)
            {
                Patients.LoadNextPartialSetAsync();
            }
          
            IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*
        * Application bar functions
        */
        public void Refresh(int id)
        {
            MergeOption cachedOption = context.MergeOption;
            context.MergeOption = MergeOption.OverwriteChanges;

            //Reload data 
            this.LoadPatient(id);

            //Reset the merge option
            context.MergeOption = cachedOption;
        }

        public void SaveChanges()
        {
            this.context.BeginSaveChanges(OnChangesSaved, this.context);
        }

        private void OnChangesSaved(IAsyncResult result)
        {
            //use the Dispatcher to ensure that the 
            //asynchronous call returns in the correct thread.
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.context = result.AsyncState as MediviseEntities;

                    try
                    {
                        //complete the save changes operation
                        this.context.EndSaveChanges(result);
                        MessageBox.Show("saved");
                    }
                    catch (DataServiceRequestException ex)
                    {
                        MessageBox.Show("fail to save");
                    }
                });
        }
    }
}

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
        private DataServiceCollection<Drug> drugs;
        private DataServiceCollection<Test> tests;
        private DataServiceCollection<Message> messages;

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

        public DataServiceCollection<Drug> Drugs
        {
            get { return drugs; }
            private set
            {
                drugs = value;
                drugs.LoadCompleted += OnDrugsLoaded;
                NotifyPropertyChanged("Drugs");
            }
        }

        public DataServiceCollection<Test> Tests
        {
            get { return tests; }
            private set
            {
                tests = value;
                tests.LoadCompleted += OnTestsLoaded;
                NotifyPropertyChanged("Tests");
            }
        }

        public DataServiceCollection<Message> Messages
        {
            get { return messages; }
            private set
            {
                messages = value;
                messages.LoadCompleted += OnMessagesLoaded;
                NotifyPropertyChanged("Messages");
            }
        }

        public bool IsDataLoaded { get; private set; }

        public void LoadPatient(int patientId)
        {
            Debug.WriteLine("loading patient");
            //Instantiate the context and binding collection.
            context = new MediviseEntities(rootUri);
            Patients = new DataServiceCollection<Patient>(context);

            var query = from p in context.Patients
                        where p.PatientId == patientId
                        select p;
            Debug.WriteLine(query.ToString());
            Patients.LoadAsync(query);
        }

        public void LoadDrug(int patientId)
        {
            Debug.WriteLine("loading drug");
            context = new MediviseEntities(rootUri);
            Drugs = new DataServiceCollection<Drug>(context);

            var query = from d in context.Drugs.Expand("DrugInfo")
                        where d.PatientId == patientId
                        select d;
            Debug.WriteLine(query.ToString());
            Drugs.LoadAsync(query);
        }

        public void LoadTest(int patientId)
        {
            context = new MediviseEntities(rootUri);
            Tests = new DataServiceCollection<Test>(context);

            var query = from t in context.Tests.Expand("TestInfo")
                        where t.PatientId == patientId
                        select t;
            Debug.WriteLine(query.ToString());
            Tests.LoadAsync(query);
        }

        public void LoadMessage(int patientId)
        {
            context = new MediviseEntities(rootUri);
            Messages = new DataServiceCollection<Message>(context);

            var query = from m in context.Messages
                        where m.PatientId == patientId
                        select m;
            Debug.WriteLine(query.ToString());
            Messages.LoadAsync(query);
        }

        //public void LoadData(MediviseEntities c, DataServiceCollection<Patient> patients)
        //{
        //    context = c;
        //    Patients = patients;

        //    IsDataLoaded = true;
        //}

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

        private void OnDrugsLoaded(object sender, LoadCompletedEventArgs e)
        {
            if (Drugs.Continuation != null)
            {
                Drugs.LoadNextPartialSetAsync();
            }

            IsDataLoaded = true;
        }

        private void OnTestsLoaded(object sender, LoadCompletedEventArgs e)
        {
            if (Tests.Continuation != null)
            {
                Tests.LoadNextPartialSetAsync();
            }

            IsDataLoaded = true;
        }

        private void OnMessagesLoaded(object sender, LoadCompletedEventArgs e)
        {
            if (Messages.Continuation != null)
            {
                Messages.LoadNextPartialSetAsync();
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
        public void Refresh(int id, int pivotIndex)
        {
            MergeOption cachedOption = context.MergeOption;
            context.MergeOption = MergeOption.OverwriteChanges;
            switch (pivotIndex)
            {
                case 0:
                    this.LoadPatient(id);
                    break;
                case 1:
                    this.LoadDrug(id);
                    break;
                case 2:
                    this.LoadTest(id);
                    break;
                case 3:
                    this.LoadMessage(id);
                    break;
            }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TBTracker.Models
{
    public class Seeder : DropCreateDatabaseAlways<TrackerEntities>
    {
        protected override void Seed(TrackerEntities context)
        {
            Patient patient = new Patient
            {
                FirstName = "Jason",
                LastName = "Cheng",
                Phone = "6175129381",
                FamilyPhone1 = "6175129381",
                FamilyPhone2 = "6175129381",
                Address = "887 Broadway",
            };
            Alert alert = new Alert
            {
                AlertDate = DateTime.Now,
                AlertType = "Miss Medication",
                Patient = patient,
            };
            context.Patients.Add(patient);
            context.Alerts.Add(alert);
        }
    }
}
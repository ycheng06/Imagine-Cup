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
            Patient patient1 = new Patient
            {
                FirstName = "Jason",
                LastName = "Cheng",
                Phone = "6175129381",
                FamilyPhone1 = "6175129381",
                FamilyPhone2 = "6175129381",
                Address = "887 Broadway",
            };
            Patient patient2 = new Patient
            {
                FirstName = "Greg",
                LastName = "Wong",
                Phone = "6175832334",
                FamilyPhone1 = "6175832334",
                FamilyPhone2 = "6175832334",
                Address = "Classified",
            };
            Alert alert1 = new Alert
            {
                AlertDate = DateTime.Now,
                AlertTypeId = 1, 
                Patient = patient1,
            };
            Alert alert2 = new Alert
            {
                AlertDate = DateTime.Now,
                AlertTypeId = 2, 
                Patient = patient2,
            };
            AlertType alertType1 = new AlertType
            {
                Name = "Missed Medication"
            };
            AlertType alertType2 = new AlertType
            {
                Name = "Missed Checkup"
            };
            context.Patients.Add(patient1);
            context.Patients.Add(patient2);
            context.Alerts.Add(alert1);
            context.Alerts.Add(alert2);
            context.AlertTypes.Add(alertType1);
            context.AlertTypes.Add(alertType2);
        }
    }
}
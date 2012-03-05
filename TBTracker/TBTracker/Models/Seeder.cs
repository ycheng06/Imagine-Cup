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
                TimeZone = "Eastern Standard Time",
                Gender = "Male",
                RegisteredBy = "Jason"
            };
            Patient patient2 = new Patient
            {
                FirstName = "Greg",
                LastName = "Wong",
                Phone = "6175832334",
                FamilyPhone1 = "6175832334",
                FamilyPhone2 = "6175832334",
                Address = "Classified",
                TimeZone = "Eastern Standard Time",
                Gender = "Male",
                RegisteredBy = "Jason"
            };
            Alert alert1 = new Alert
            {
                AlertDate = DateTime.UtcNow,
                AlertTypeId = 1, 
                Patient = patient1,
            };
            Alert alert2 = new Alert
            {
                AlertDate = DateTime.UtcNow,
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
            Message message1 = new Message
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                MessageText = "You need to follow your regimen!",
                Patient = patient1
            };
            Message message2 = new Message
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                MessageText = "Calling Greg! Report to base to pick up your regimen!",
                Patient = patient2
            };
            Message message3 = new Message
            {
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow,
                MessageText = "This is intended to be an expired message.",
                Patient = patient1
            };

            context.Patients.Add(patient1);
            context.Patients.Add(patient2);
            context.Alerts.Add(alert1);
            context.Alerts.Add(alert2);
            context.AlertTypes.Add(alertType1);
            context.AlertTypes.Add(alertType2);
            context.Messages.Add(message1);
            context.Messages.Add(message2);
            context.Messages.Add(message3);

            ////////////////////////////////////////////////////////////
            //Sean's tests for patients -> drugs/tests
            DrugInfo drugInfo1 = new DrugInfo
            {
                Name = "Isoniazid (INH)",
                Description = "..spice and everything nice",
            };

            DrugInfo drugInfo2 = new DrugInfo
            {
                Name = "Rifampin (RIF)",
                Description = "that was easy..",
            };

            DrugInfo drugInfo3 = new DrugInfo
            {
                Name = "Ethambutol (EMB)",
                Description = "cheap and effective",
            };

            DrugInfo drugInfo4 = new DrugInfo
            {
                Name = "Pyrazinamide (PZA)",
                Description = "",
            };

            Drug drug1 = new Drug
            {
                StartDate = new DateTime(2010, 1, 1),
                EndDate = new DateTime(2010, 2, 2),
                Type = "none",
                DrugInfo = drugInfo1,
                Patient = patient1,
            };

            Drug drug2 = new Drug
            {
                StartDate = new DateTime(2010, 1, 1),
                EndDate = new DateTime(2010, 2, 2),
                TimesPerWeek = 2,
                Type = "none",
                DrugInfo = drugInfo2,
                Patient = patient1,
            };

            Drug drug3 = new Drug
            {
                StartDate = new DateTime(2010, 1, 1),
                EndDate = new DateTime(2010, 2, 2),
                TimesPerWeek = 3,
                Type = "none",
                DrugInfo = drugInfo3,
                Patient = patient1,
            };

            Drug drug4 = new Drug
            {
                StartDate = new DateTime(2010, 2, 3),
                EndDate = new DateTime(2010, 2, 20),
                TimesPerWeek = 4,
                Type = "none",
                DrugInfo = drugInfo3,
                Patient = patient1,
            };

            TestInfo testInfo1 = new TestInfo
            {
                Name = "TI1: leap of faith",
            };

            TestInfo testInfo2 = new TestInfo
            {
                Name = "TI2: calculus exam",
            };

            Test test1 = new Test
            {
                TestDate = new DateTime(2010, 1, 17),
                TestResult = "positive",
                TestInfo = testInfo1,
                Patient = patient1,
            };

            Test test2 = new Test
            {
                TestDate = new DateTime(2010, 1, 18),
                TestResult = "negative",
                TestInfo = testInfo2,
                Patient = patient1,
            };

            Test test3 = new Test
            {
                TestDate = new DateTime(2010, 1, 19),
                TestResult = "positive",
                TestInfo = testInfo1,
                Patient = patient1,
            };

            context.DrugInfos.Add(drugInfo1);
            context.DrugInfos.Add(drugInfo2);
            context.DrugInfos.Add(drugInfo3);
            context.Drugs.Add(drug1);
            context.Drugs.Add(drug2);
            context.Drugs.Add(drug3);
            context.Drugs.Add(drug4);

            context.TestInfos.Add(testInfo1);
            context.TestInfos.Add(testInfo2);
            context.Tests.Add(test1);
            context.Tests.Add(test2);
            context.Tests.Add(test3);
            context.Patients.Add(patient1);
            context.Patients.Add(patient2);

        }
    }
}
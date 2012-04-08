/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MediviseMVC.Models
{
    public class Seeder : DropCreateDatabaseAlways<MediviseEntities>
    {
        protected override void Seed(MediviseEntities context)
        {
            //Patient patient1 = new Patient
            //{
            //    FirstName = "Jason",
            //    LastName = "Cheng",
            //    Phone = "6469266783",
            //    FamilyPhone1 = "6469266783",
            //    FamilyPhone2 = "6469266783",
            //    Address = "887 Broadway",
            //    TimeZone = "Eastern Standard Time",
            //    Gender = "Male",
            //    RegisteredBy = "bamf",
            //    ResponseReceived = false
            //};
            //Patient patient2 = new Patient
            //{
            //    FirstName = "Susie",
            //    LastName = "Wong",
            //    Phone = "6175832334",
            //    FamilyPhone1 = "6175832334",
            //    FamilyPhone2 = "6175832334",
            //    Address = "Classified",
            //    TimeZone = "Eastern Standard Time",
            //    Gender = "Female",
            //    RegisteredBy = "bamf",
            //    ResponseReceived = false
            //};
            //Alert alert1 = new Alert
            //{
            //    AlertDate = DateTime.UtcNow,
            //    AlertTypeId = 1, 
            //    Patient = patient1,
            //};
            //Alert alert2 = new Alert
            //{
            //    AlertDate = DateTime.UtcNow,
            //    AlertTypeId = 2, 
            //    Patient = patient2,
            //};
            AlertType alertType1 = new AlertType
            {
                AlertTypeId = 1,
                Name = "Missed Medication"
            };
            AlertType alertType2 = new AlertType
            {
                AlertTypeId = 2,
                Name = "Missed Checkup"
            };
            AlertType alertType3 = new AlertType
            {
                AlertTypeId = 3,
                Name = "Completed Treatment"
            };
            //Message message1 = new Message
            //{
            //    StartDate = DateTime.UtcNow,
            //    EndDate = DateTime.UtcNow.AddDays(1),
            //    MessageText = "You need to follow your regimen!",
            //    Patient = patient1
            //};
            //Message message2 = new Message
            //{
            //    StartDate = DateTime.UtcNow,
            //    EndDate = DateTime.UtcNow.AddDays(1),
            //    MessageText = "Calling Greg! Report to base to pick up your regimen!",
            //    Patient = patient2
            //};
            //Message message3 = new Message
            //{
            //    StartDate = DateTime.UtcNow.AddDays(-1),
            //    EndDate = DateTime.UtcNow,
            //    MessageText = "This is intended to be an expired message.",
            //    Patient = patient1
            //};

            //context.Patients.Add(patient1);
            //context.Patients.Add(patient2);
            //context.Alerts.Add(alert1);
            //context.Alerts.Add(alert2);
            context.AlertTypes.Add(alertType1);
            context.AlertTypes.Add(alertType2);
            //context.Messages.Add(message1);
            //context.Messages.Add(message2);
            //context.Messages.Add(message3);

            ////////////////////////////////////////////////////////////
            //Sean's tests for patients -> drugs/tests
            DrugInfo drugInfo1 = new DrugInfo
            {
                Name = "Isoniazid (INH)",
                Description = "First-line",
            };

            DrugInfo drugInfo2 = new DrugInfo
            {
                Name = "Rifampin (RIF)",
                Description = "First-line",
            };

            DrugInfo drugInfo3 = new DrugInfo
            {
                Name = "Ethambutol (EMB)",
                Description = "First-line",
            };

            DrugInfo drugInfo4 = new DrugInfo
            {
                Name = "Pyrazinamide (PZA)",
                Description = "First-line",
            };



            //Drug drug1 = new Drug
            //{
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddDays(4),
            //    Type = "none",
            //    DrugInfo = drugInfo1,
            //    Patient = patient1,
            //};

            //Drug drug2 = new Drug
            //{
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddDays(10),
            //    TimesPerWeek = 2,
            //    Type = "none",
            //    DrugInfo = drugInfo2,
            //    Patient = patient1,
            //};

            //Drug drug3 = new Drug
            //{
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddDays(10),
            //    TimesPerWeek = 3,
            //    Type = "none",
            //    DrugInfo = drugInfo3,
            //    Patient = patient1,
            //};

            //Drug drug4 = new Drug
            //{
            //    StartDate = new DateTime(2010, 2, 3),
            //    EndDate = new DateTime(2010, 2, 20),
            //    TimesPerWeek = 4,
            //    Type = "none",
            //    DrugInfo = drugInfo3,
            //    Patient = patient1,
            //};

            TestInfo testInfo1 = new TestInfo
            {
                Name = "CXR",
            };

            TestInfo testInfo2 = new TestInfo
            {
                Name = "AFB Smear",
            };

            TestInfo testInfo3 = new TestInfo
            {
                Name = "Culture",
            };

            //Test test1 = new Test
            //{
            //    TestDate = DateTime.Now.AddDays(2),
            //    TestResult = "positive",
            //    TestInfo = testInfo1,
            //    Patient = patient1,
            //};

            //Test test2 = new Test
            //{
            //    TestDate = DateTime.Now.AddDays(3),
            //    TestResult = "negative",
            //    TestInfo = testInfo2,
            //    Patient = patient1,
            //};

            //Test test3 = new Test
            //{
            //    TestDate = new DateTime(2010, 1, 19),
            //    TestResult = "positive",
            //    TestInfo = testInfo1,
            //    Patient = patient1,
            //};

            context.DrugInfos.Add(drugInfo1);
            context.DrugInfos.Add(drugInfo2);
            context.DrugInfos.Add(drugInfo3);
            context.DrugInfos.Add(drugInfo4);


            context.DrugInfos.Add( new DrugInfo {
                   Name = "Streptomycin (STM)",
                   Description = "Streptomycin is no longer considered as a first line drug by ATS/IDSA/CDC because of high rates of resistance",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Amikacin (AMK)",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Kanamycin (KM)",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Capreomycin",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Viomycin",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Enviomycin",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Ciprofloxacin (CIP)",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Levofloxacin",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Moxifloxacin (MXF)",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Ethionamide",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Prothionamide",
                   Description = "Second-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Ribafutin",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Clarithromycin (CLR)",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Linezolid (LZD)",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Thioacetazone (T)",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "R207910",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Thioridazine",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Arginine",
                   Description = "Third-line",
               });
            context.DrugInfos.Add( new DrugInfo {
                   Name = "Vitamin D",
                   Description = "Third-line",
               });
            //context.Drugs.Add(drug1);
            //context.Drugs.Add(drug2);
            //context.Drugs.Add(drug3);
            //context.Drugs.Add(drug4);

            context.TestInfos.Add(testInfo1);
            context.TestInfos.Add(testInfo2);
            //context.Tests.Add(test1);
            //context.Tests.Add(test2);
            //context.Tests.Add(test3);
            //context.Patients.Add(patient1);
            //context.Patients.Add(patient2);

        }
    }
}
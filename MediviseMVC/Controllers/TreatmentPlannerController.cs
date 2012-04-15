/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediviseMVC.Models;
using System.Data;
using System.Data.Entity;

namespace MediviseMVC.Controllers
{
    [Authorize]
    public class TreatmentPlannerController : Controller
    {
        MediviseEntities trackerDB = new MediviseEntities();

        // GET: /TreatmentPlanner/Edit/1
        public ActionResult Edit(int id) //id == patientId
        {

            var patient = trackerDB.Patients.Find(id);
            return View(patient);
        }

        //****************Drug***************
        // GET: /TreatmentPlanner/AddDrug
        public ActionResult AddDrug(int pid)
        {
            ViewData["PatientId"] = pid;
            populateDrugNames(1);
            return View(new Drug
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = true,
            });
        }

        // POST: /TreatmentPlanner/AddDrug
        [HttpPost]
        public ActionResult AddDrug(Drug drug)
        {
            if (ModelState.IsValid)
            {
                trackerDB.Patients.Find(drug.PatientId).Drugs.Add(drug);
                trackerDB.SaveChanges();

                drug.TimesPerWeek = getTimesPerWeek(drug);
                updatePatientTreatmentStartDate(drug.PatientId, drug.StartDate);
                updatePatientTreatmentEndDate(drug.PatientId, drug.EndDate);
                return RedirectToAction("Edit", new { id = drug.PatientId });
            }
            populateDrugNames(1);
            return View(drug);
        }

        // GET: /TreatmentPlanner/EditDrug
        public ActionResult EditDrug(int id)
        {
            var drug = trackerDB.Drugs.Find(id);
            populateDrugNames(drug.DrugInfoId);
            return View(drug);
        }

        // POST: /TreatmentPlanner/EditDrug
        [HttpPost]
        public ActionResult EditDrug(Drug drug)
        {
            if (ModelState.IsValid)
            {
                trackerDB.Entry(drug).State = EntityState.Modified;
                trackerDB.SaveChanges();

                drug.TimesPerWeek = getTimesPerWeek(drug);
                updatePatientTreatmentStartDate(drug.PatientId, drug.StartDate);
                updatePatientTreatmentEndDate(drug.PatientId, drug.EndDate);
                return RedirectToAction("Edit", new { id = drug.PatientId });
            }
            populateDrugNames(drug.DrugInfoId);
            return View();
        }

        private int getTimesPerWeek(Drug drug) //done every time a drug is modified/added (and NOT every time it is displayed)
        {
            int total = 0;

            if (drug.Monday)
                total++;
            if (drug.Tuesday)
                total++;
            if (drug.Wednesday)
                total++;
            if (drug.Thursday)
                total++;
            if (drug.Friday)
                total++;
            if (drug.Saturday)
                total++;
            if (drug.Sunday)
                total++;

            return total;
        }

        // GET: /TreatmentPlanner/DeleteDrug/3
        public ActionResult DeleteDrug(int id)
        {
            var Drug = trackerDB.Drugs.Find(id);
            return View(Drug);
        }

        // POST: /TreatmentPlanner/DeleteDrug/3
        [HttpPost, ActionName("DeleteDrug")]
        public ActionResult DeleteDrugConfirmed(int id)
        {
            var Drug = trackerDB.Drugs.Find(id);
            var pid = Drug.PatientId;
            trackerDB.Drugs.Remove(Drug);
            trackerDB.SaveChanges();
            return RedirectToAction("Edit", new { id = pid });
        }

        //**********************Test Messages***********************************
        public ActionResult AddTest(int pid)
        {
            ViewData["PatientId"] = pid;
            populateTestNames();
            var testmsg = new Test { TestDate = DateTime.UtcNow };
            return View(testmsg);
        }
        [HttpPost]
        public ActionResult AddTest(Test test)
        {
            if (ModelState.IsValid)
            {
                trackerDB.Patients.Find(test.PatientId).Tests.Add(test);
                trackerDB.SaveChanges();

                updatePatientTreatmentStartDate(test.PatientId, test.TestDate);
                updatePatientTreatmentEndDate(test.PatientId, test.TestDate);
                return RedirectToAction("Edit", new { id = test.PatientId });
            }
            populateTestNames();
            return View(test);
        }
        public ActionResult EditTest(int id)
        {
            var test = trackerDB.Tests.Find(id);
            populateTestNames();
            return View(test);
        }
        [HttpPost]
        public ActionResult EditTest(Test test)
        {
            if (ModelState.IsValid)
            {
                trackerDB.Entry(test).State = EntityState.Modified;
                trackerDB.SaveChanges();

                updatePatientTreatmentStartDate(test.PatientId, test.TestDate);
                updatePatientTreatmentEndDate(test.PatientId, test.TestDate);
                return RedirectToAction("Edit", new { id=test.PatientId});
            }
            populateTestNames();
            return View(test);
        }
        public ActionResult DeleteTest(int id)
        {
            var test = trackerDB.Tests.Find(id);
            return View(test);
        }
        [HttpPost, ActionName("DeleteTest")]
        public ActionResult DeleteTestConfirmed(int id)
        {
            var test = trackerDB.Tests.Find(id);
            var pid = test.PatientId;
            trackerDB.Tests.Remove(test);
            trackerDB.SaveChanges();
            return RedirectToAction("Edit", new { id = pid });
        }
        //****************************Custom Messages****************************
        public ActionResult AddMsg(int pid)
        {
            ViewData["PatientId"] = pid;
            var msg = new Message { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1) };
            return View(msg);
        }
        [HttpPost]
        public ActionResult AddMsg(Message msg)
        {
            if (ModelState.IsValid)
            {
                trackerDB.Patients.Find(msg.PatientId).Messages.Add(msg);
                trackerDB.SaveChanges();

                updatePatientTreatmentStartDate(msg.PatientId, msg.StartDate);
                updatePatientTreatmentEndDate(msg.PatientId, msg.EndDate);
                return RedirectToAction("Edit", new { id = msg.PatientId });
            }
            return View(msg);
        }
        public ActionResult EditMsg(int id)
        {
            var msg = trackerDB.Messages.Find(id);
            return View(msg);
        }
        [HttpPost]
        public ActionResult EditMsg(Message msg)
        {
            if (ModelState.IsValid)
            {
                trackerDB.Entry(msg).State = EntityState.Modified;
                trackerDB.SaveChanges();

                updatePatientTreatmentStartDate(msg.PatientId, msg.StartDate);
                updatePatientTreatmentEndDate(msg.PatientId, msg.EndDate);
                return RedirectToAction("Edit", new { id=msg.PatientId});
            }
            return View(msg);
        }
        public ActionResult DeleteMsg(int id)
        {
            var msg = trackerDB.Messages.Find(id);
            return View(msg);
        }
        [HttpPost, ActionName("DeleteMsg")]
        public ActionResult DeleteMsgConfirmed(int id)
        {
            var msg = trackerDB.Messages.Find(id);
            trackerDB.Messages.Remove(msg);
            trackerDB.SaveChanges();
            return RedirectToAction("Edit", new { id = msg.PatientId });
        }
        //**********************Helper methods*****************
        private void populateTestNames()
        {
            var testnames = from t in trackerDB.TestInfos
                            orderby t.Name
                            select t;
            ViewData["TestInfoId"] = new SelectList(testnames, "TestInfoId", "Name");
        }
        private void populateDrugNames(int drugInfoId)
        {
            var drugnames = from d in trackerDB.DrugInfos
                            orderby d.Name
                            select d;
            ViewData["DrugInfoId"] = new SelectList(drugnames, "DrugInfoId","Name", drugInfoId);
        }
        private bool isUrlValid(Patient patient)
        {
            //check of url hacking
            if (patient == null || !patient.IsRegisteredBy(User.Identity.Name))
            {
                return false;
            }
            return true;
        }
        private void updatePatientTreatmentStartDate(int patientId, DateTime startDate)  //done every time a drug is modified/added (and NOT every time it is displayed)
        {
            //look up corresponding patient
            var patient = trackerDB.Patients.Find(patientId);

            if (startDate < patient.TreatmentStartDate)
            {
                patient.TreatmentStartDate = startDate;
                trackerDB.Entry(patient).State = EntityState.Modified;
                trackerDB.SaveChanges();
            }
            else if (patient.TreatmentStartDate < startDate)
            {
                replaceWithSmallestStartDate(patient.PatientId, startDate);
            }

        }
        private void replaceWithSmallestStartDate(int patientId, DateTime updatedDate)
        {
            //go through drug, test, messages and find largest enddate
            var patient = trackerDB.Patients.Find(patientId);
            DateTime startDate = updatedDate;

            //drug
            List<Drug> drugs = trackerDB.Drugs.Where(d => d.PatientId == patientId).ToList();
            foreach (Drug d in drugs)
            {
                if (d.StartDate < startDate)
                {
                    startDate = d.StartDate;
                }
            }

            //test
            List<Test> tests = trackerDB.Tests.Where(t => t.PatientId == patientId).ToList();
            foreach (Test t in tests)
            {
                if (t.TestDate < startDate)
                {
                    startDate = t.TestDate;
                }
            }
            
            //message
            List<Message> messages = trackerDB.Messages.Where(m => m.PatientId == patientId).ToList();
            foreach (Message m in messages)
            {
                if (m.EndDate < startDate)
                {
                    startDate = m.StartDate;
                }
            }

            patient.TreatmentStartDate = startDate;
            trackerDB.Entry(patient).State = EntityState.Modified;
            trackerDB.SaveChanges();
        }
        private void updatePatientTreatmentEndDate(int patientId, DateTime endDate)  //done every time a drug is modified/added (and NOT every time it is displayed)
        {
            //look up corresponding patient
            var patient = trackerDB.Patients.Find(patientId);

            if (endDate > patient.TreatmentEndDate)
            {
                patient.TreatmentEndDate = endDate;
                trackerDB.Entry(patient).State = EntityState.Modified;
                trackerDB.SaveChanges();
            }
            else if (patient.TreatmentEndDate > endDate)
            {
                replaceWithGreatestEndDate(patient.PatientId, endDate);
            }
        }
        private void replaceWithGreatestEndDate(int patientId, DateTime latestDate)
        {
            //go through drug, test, messages and find largest enddate
            var patient = trackerDB.Patients.Find(patientId);
            DateTime endDate = latestDate;

            //drug
            List<Drug> drugs = trackerDB.Drugs.Where(d => d.PatientId == patientId).ToList();
            foreach (Drug d in drugs)
            {
                if (d.EndDate > endDate)
                {
                    endDate = d.EndDate;
                }
            }

            //test
            List<Test> tests = trackerDB.Tests.Where(t => t.PatientId == patientId).ToList();
            foreach (Test t in tests)
            {
                if (t.TestDate > endDate)
                {
                    endDate = t.TestDate;
                }
            }
            
            //message
            List<Message> messages = trackerDB.Messages.Where(m => m.PatientId == patientId).ToList();
            foreach (Message m in messages)
            {
                if (m.EndDate > endDate)
                {
                    endDate = m.EndDate;
                }
            }

            //change TreatmentEndDate
            patient.TreatmentEndDate = endDate;
            trackerDB.Entry(patient).State = EntityState.Modified;
            trackerDB.SaveChanges();
        }
        protected override void Dispose(bool disposing)
        {
            trackerDB.Dispose();
            base.Dispose(disposing);
        }
    }
}

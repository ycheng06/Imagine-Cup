using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;
using System.Data;
using System.Data.Entity;

namespace TBTracker.Controllers
{
    public class MsgTemplateController : Controller
    {
        TrackerEntities trackerDB = new TrackerEntities();

        //
        // GET: /Timeline/
        public ActionResult Index()
        {
            var patients = trackerDB.Patients.ToList();
            return View(patients);
        }
        // GET: /Timeline/Edit/1
        public ActionResult Edit(int id) //id == patientId
        {
            var Patient = trackerDB.Patients.Find(id);
            return View(Patient);
        }
        // GET: /MsgTemplate/AddDrug
        public ActionResult AddDrug(int pid)
        {
            ViewData["PatientId"] = pid;
            populateDrugNames();
            return View();
        }
        // POST: /MsgTemplate/AddDrug
        [HttpPost]
        public ActionResult AddDrug(Drug drug)
        {
            if (ModelState.IsValid)
            {
                var x = drug.PatientId;
                var z = drug.DrugInfoId;
                var a = drug.DrugId;
                trackerDB.Patients.Find(drug.PatientId).Drugs.Add(drug);
                trackerDB.SaveChanges();
                return RedirectToAction("Edit", new { id = drug.PatientId });
            }
            populateDrugNames();
            return View();
        }
        // GET: /MsgTemplate/EditDrug
        public ActionResult EditDrug(int id)
        {
            var Drug = trackerDB.Drugs.Find(id);
            populateDrugNames();
            return View(Drug);
        }
        // POST: /MsgTemplate/EditDrug
        [HttpPost]
        public ActionResult EditDrug(Drug drug)
        {
            if (ModelState.IsValid)
            {
                var x = drug.PatientId;
                var y = drug.DrugInfoId;
                var gid = drug.DrugId;
                var z = drug.EndDate;
                trackerDB.Entry(drug).State = EntityState.Modified;
                trackerDB.SaveChanges();
                return RedirectToAction("Edit", new { id = drug.PatientId });
            }
            populateDrugNames();
            return View();
        }
        // GET: /MsgTemplate/DeleteDrug
        public ActionResult DeleteDrug(int id)
        {
            var Drug = trackerDB.Drugs.Find(id);
            return View(Drug);
        }
        [HttpPost, ActionName("DeleteDrug")]
        public ActionResult DeleteConfirmed(int id)
        {
            var Drug = trackerDB.Drugs.Find(id);
            var pid = Drug.PatientId;
            trackerDB.Drugs.Remove(Drug);
            trackerDB.SaveChanges();
            return RedirectToAction("Edit", new { id = pid });
        }
        private void populateDrugNames()
        {
            var drugnames = from d in trackerDB.DrugInfos
                            orderby d.Name
                            select d;
            ViewData["DrugInfoId"] = new SelectList(drugnames, "DrugInfoId","Name");
        }
        protected override void Dispose(bool disposing)
        {
            trackerDB.Dispose();
            base.Dispose(disposing);
        }
    }
}

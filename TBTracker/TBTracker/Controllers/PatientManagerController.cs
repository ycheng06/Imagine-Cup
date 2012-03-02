using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;
using System.Collections.ObjectModel;

namespace TBTracker.Controllers
{ 
    [Authorize]
    public class PatientManagerController : Controller
    {
        private TrackerEntities db = new TrackerEntities();

        //
        // GET: /PatientManager/

        public ViewResult Index()
        {
            return View(db.Patients.ToList());
        }

        //
        // GET: /PatientManager/Details/5

        public ViewResult Details(int id)
        {
            Patient patient = db.Patients.Find(id);
            return View(patient);
        }

        //
        // GET: /PatientManager/Create

        public ActionResult Create()
        {
            populateTimeZones(null);
            return View();
        } 

        //
        // POST: /PatientManager/Create

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            populateTimeZones(null);
            return View(patient);
        }
        
        //
        // GET: /PatientManager/Edit/5
 
        public ActionResult Edit(int id)
        {
            Patient patient = db.Patients.Find(id);
            populateTimeZones(patient.TimeZone);
            return View(patient);
        }

        //
        // POST: /PatientManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            populateTimeZones(patient.TimeZone);
            return View(patient);
        }

        [HttpPost]
        public ActionResult SaveAndEditTimeline(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "MsgTemplate", new { id = patient.PatientId });
            }
            return View(patient);
        }

        //
        // GET: /PatientManager/Delete/5
 
        public ActionResult Delete(int id)
        {
            Patient patient = db.Patients.Find(id);
            return View(patient);
        }

        //
        // POST: /PatientManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private void populateTimeZones(string id)
        {
            Dictionary<string, string> timeZones = new Dictionary<string, string>();
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                timeZones.Add(tz.Id, tz.DisplayName);
            }
            if (id == null)
            {
                ViewData["TimeZone"] = new SelectList(timeZones, "Key", "Value");
            }
            else
            {
                ViewData["TimeZone"] = new SelectList(timeZones, "Key", "Value", id);
            }
        }
    }
}
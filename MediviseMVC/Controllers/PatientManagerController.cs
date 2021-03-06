﻿/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediviseMVC.Models;
using System.Collections.ObjectModel;
using MediviseMVC.Twilio;
using Quartz;
using MediviseMVC.Jobs;
using Quartz.Impl;
using MediviseMVC.ActionFilters;
using System.Diagnostics;

namespace MediviseMVC.Controllers
{ 
    [Authorize]
    public class PatientManagerController : Controller
    {
        private TwilioSender twilio = new TwilioSender();
        private MediviseEntities db = new MediviseEntities();
        // GET: /PatientManager/
        public ViewResult Index()
        {
            return View(db.Patients.Where(patient => patient.RegisteredBy == User.Identity.Name).ToList());
        }

        // GET: /PatientManager/Details/5
        [PreventUrlHacking]
        public ActionResult Details(int id)
        {
            Patient patient = db.Patients.Find(id);
            /* pass associated alerts to the view */
            List<Alert> alerts = db.Alerts.Where(a => a.PatientId == id).OrderBy(a=>a.AlertDate).ToList();
            ViewData["alerts"] = alerts;
            return View(patient);
        }

        // GET: /PatientManager/Create
        public ActionResult Create()
        {
            populateTimeZones(Profile.GetCurrent().TimeZone);
            populateGenderList(null);

            return View(new Patient
            {
                TreatmentStartDate = DateTime.UtcNow,
                TreatmentEndDate = DateTime.UtcNow.AddMonths(6),
            });
        } 

        // POST: /PatientManager/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.RegisteredBy = User.Identity.Name;
                patient.ResponseReceived = true; //first day - change later if this doesn't work
                db.Patients.Add(patient);
                db.SaveChanges();
                sendRegisterConfirmation(patient);
               // uncomment this call for testing 
               // sendDemoReminders(patient.PatientId);
                return RedirectToAction("Index");  
            }
              
            if (patient.TimeZone != null)
            {
                populateTimeZones(patient.TimeZone);
            }
            else
            {
                populateTimeZones(Profile.GetCurrent().TimeZone);
            }
            if (patient.Gender != null)
            {
                populateGenderList(patient.Gender);
            }
            else
            {
                populateGenderList(null);
            }
            return View(patient);
        }

        private void sendRegisterConfirmation(Patient p)
        {
            string msg = String.Format("Dear {0}, welcome to Medivise! Hope you get well soon!\n", p.FirstName);
            TwilioSender sender = new TwilioSender();
            sender.SendSMS(p.Phone, msg);
            //Trace.WriteLine(msg);
        }
        // GET: /PatientManager/Edit/5
        [PreventUrlHacking]
        public ActionResult Edit(int id)
        {
            Patient patient = db.Patients.Find(id);
            populateTimeZones(patient.TimeZone);
            populateGenderList(patient.Gender);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PatientEdit", patient);
            }
            else
            {
                return View(patient);
            }
        }

        // POST: /PatientManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            bool isAjax = Request.IsAjaxRequest();
            if (ModelState.IsValid)
            {
                patient.RegisteredBy = User.Identity.Name;
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                if (isAjax)
                {
                    return PartialView("PatientView", patient);
                }
                return RedirectToAction("Details", new { id = patient.PatientId });
            }
            populateTimeZones(patient.TimeZone);
            populateGenderList(patient.Gender);
            return isAjax ? View("PatientEdit",patient) : View(patient);
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

        // GET: /PatientManager/Delete/5
        public ActionResult Delete(int id)
        {
            Patient patient = db.Patients.Find(id);
            return View(patient);
        }

        // POST: /PatientManager/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //*******JSON Data Feeds for AJAX*********************
        [HttpPost]
        public JsonResult PatientList(int jtStartIndex,int jtPageSize,string jtSorting=null)
        {
            try
            {
                //reconsider this part
                List<Patient> patients = db.Patients.Where(p => p.RegisteredBy == User.Identity.Name).ToList();
                switch (jtSorting)
                {
                    case "FirstName ASC":
                        patients = patients.OrderBy(p => p.FirstName).ToList();
                        break;
                    case "FirstName DESC":
                        patients = patients.OrderByDescending(p => p.FirstName).ToList();
                        break;
                    case "LastName ASC":
                        patients = patients.OrderBy(p => p.LastName).ToList();
                        break;
                    case "LastName DESC":
                        patients = patients.OrderByDescending(p => p.LastName).ToList();
                        break;
                    case "TreatmentStartDate ASC":
                        patients = patients.OrderBy(p => p.TreatmentStartDate).ToList();
                        break;
                    case "TreatmentStartDate DESC":
                        patients = patients.OrderByDescending(p => p.TreatmentStartDate).ToList();
                        break;
                    case "TreatmentEndDate ASC":
                        patients = patients.OrderBy(p => p.TreatmentEndDate).ToList();
                        break;
                    case "TreatmentEndDate DESC":
                        patients = patients.OrderByDescending(p => p.TreatmentEndDate).ToList();
                        break;
                    default:
                        break;
                }
                List<Patient> currentPage = patients.Skip(jtStartIndex).Take(jtPageSize).ToList();
                var jsonData = currentPage.Select(p => JsonizePatient(p));
                return Json(new { Result = "OK", Records = jsonData,TotalRecordCount = patients.Count});
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR",Message = ex.Message});
            }
        }
        [HttpPost]
        public JsonResult CreatePatient(Patient patient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form Not Valid" });
                }
                patient.RegisteredBy = User.Identity.Name;
                db.Patients.Add(patient);
                db.SaveChanges();
                sendRegisterConfirmation(patient);
                return Json(new { Result = "OK", Record = JsonizePatient(patient) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult UpdatePatient(Patient patient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form Not Valid" });
                }
                patient.RegisteredBy = User.Identity.Name;
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); 
            }
        }
        [HttpPost]
        public JsonResult DeletePatient(int PatientId)
        {
            try
            {
                Patient patient = db.Patients.Find(PatientId);
                db.Patients.Remove(patient);
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult TimeZoneList()
        {
            try
            {
                var jsonData = TimeZoneInfo.GetSystemTimeZones().Select(t => new {
                    DisplayText = t.DisplayName, 
                    Value = t.Id
                });
                return Json(new { Result = "OK", Options = jsonData });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private Object JsonizePatient(Patient p)
        {
                return new {
                        PatientId = p.PatientId,
                        Gender    = p.Gender,
                        FirstName = p.FirstName,
                        LastName  = p.LastName,
                        Address   = p.Address,
                        Phone     = p.Phone,
                        FamilyPhone1 = p.FamilyPhone1,
                        FamilyPhone2 = p.FamilyPhone2,
                        TreatmentStartDate = p.TreatmentStartDate,
                        TreatmentEndDate = p.TreatmentEndDate,
                        TimeZone = p.TimeZone
                };
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        //******************Helper Methods********************
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

         private void populateGenderList(string id)
        {
            Dictionary<string, string> genders = new Dictionary<string, string>();
            genders.Add("Male", "Male");
            genders.Add("Female", "Female");
            genders.Add("Unspecified", "Unspecified");
            if (id == null)
            {
                ViewData["Gender"] = new SelectList(genders, "Key", "Value");
            }
            else
            {
                ViewData["Gender"] = new SelectList(genders, "Key", "Value", id);
            }
        }
    }
}
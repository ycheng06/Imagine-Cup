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
            return View(patient);
        }

        // GET: /PatientManager/Create
        public ActionResult Create()
        {
            populateTimeZones(null);
            populateGenderList(null);
            return View();
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
                sendDemoReminders(patient.PatientId);
                return RedirectToAction("Index");  
            }
              
            populateTimeZones(null);
            populateGenderList(null);
            return View(patient);
        }
        private void sendDemoReminders(int id)
        {
            ISchedulerFactory schedulePool = new StdSchedulerFactory();
            IScheduler sched = schedulePool.GetScheduler();
            sched.Start();
            //set up reminder sender
            try
            {
                JobDetail reminderJob = new JobDetail("AlertBuilder", null, typeof(SendReminderJob));
                reminderJob.JobDataMap["pid"] = id;
                Trigger trigger = TriggerUtils.MakeMinutelyTrigger("t1", 2, 1);
                trigger.StartTimeUtc = TriggerUtils.GetEvenMinuteDate(DateTime.UtcNow.AddMinutes(1));
                sched.ScheduleJob(reminderJob, trigger);
                //set up warning sender
                JobDetail warningJob = new JobDetail("Warnings", null, typeof(SendWarningJob));
                warningJob.JobDataMap["pid"] = id;
                Trigger trigger2 = TriggerUtils.MakeSecondlyTrigger("test2", 10, 0);
                trigger2.StartTimeUtc = TriggerUtils.GetEvenMinuteDate(DateTime.UtcNow).AddMinutes(2);
                sched.ScheduleJob(warningJob, trigger2);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                throw e;
            }
        }
        private void sendRegisterConfirmation(Patient p)
        {
            string msg = String.Format("Dear {0}, Welcome to Medivise! We all hope you will get well very soon!\n", p.FirstName);
            TwilioSender sender = new TwilioSender();
            sender.SendSMS(p.Phone, msg);
            Trace.WriteLine(msg);
        }
        // GET: /PatientManager/Edit/5
        public ActionResult Edit(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (isUrlValid(patient))
            {
                populateTimeZones(patient.TimeZone);
                populateGenderList(patient.Gender);
                return View(patient);
            }
            return RedirectToAction("Index");
        }

        // POST: /PatientManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.RegisteredBy = User.Identity.Name;
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            populateTimeZones(patient.TimeZone);
            populateGenderList(patient.Gender);
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

        // GET: /PatientManager/Delete/5
        public ActionResult Delete(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (isUrlValid(patient))
            {
                return View(patient);
            }
            return RedirectToAction("Index");
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

         private bool isUrlValid(Patient patient)
         {
            //check of url hacking
            if (patient == null || !patient.IsRegistedBy(User.Identity.Name))
            {
                return false; 
            }
            return true;
         }
    }
}
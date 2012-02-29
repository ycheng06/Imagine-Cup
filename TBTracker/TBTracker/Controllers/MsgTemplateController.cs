using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;

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

        //
        // GET: /Timeline/Edit/1
        // [xihan] This is the controller to display/edit the timelines for each patient
        public ActionResult Edit(int id) //id == patientId
        {
            var Patient = trackerDB.Patients.Find(id);
            return View(Patient);
        }

        protected override void Dispose(bool disposing)
        {
            trackerDB.Dispose();
            base.Dispose(disposing);
        }
    }
}

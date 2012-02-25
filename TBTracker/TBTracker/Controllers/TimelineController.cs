using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;

namespace TBTracker.Controllers
{
    public class TimelineController : Controller
    {
        TrackerEntities trackerDB = new TrackerEntities();

        //
        // GET: /Timeline/

        public ActionResult Index()
        {
            var patients = trackerDB.Patients.ToList();
            return View(patients);
        }

/*        public ActionResult Form()
        {

        }*/
    }
}

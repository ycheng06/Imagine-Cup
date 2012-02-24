using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;

namespace TBTracker.Controllers
{
    public class AlertController : Controller
    {
        //
        // GET: /Alert/
        private TrackerEntities db = new TrackerEntities();

        public ViewResult Index()
        {
            var samp_alerts = new List<Alert>
            {
                new Alert { PatientId = 666, AlertDate = DateTime.Now, AlertType = "Bad News"}
            };

            return View(samp_alerts);
            //return View(db.Alerts.ToList());
        }

    }
}

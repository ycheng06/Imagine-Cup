using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;

namespace TBTracker.Controllers
{
    [Authorize(Roles="user")]
    public class AlertController : Controller
    {
        //
        // GET: /Alert/
        private TrackerEntities db = new TrackerEntities();
        //for now
        private TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public ActionResult Index()
        {
            return View(db.Alerts.Include("Patient")
                                        .Include("AlertType").Where(alert => alert.Patient.RegisteredBy == User.Identity.Name).ToList());
        }
        public ViewResult Details(int id)
        {
            var alert = db.Alerts.Find(id);
            return View(alert);
        }
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Alert/Create

        [HttpPost]
        public ActionResult Create(Alert alert)
        {

            if (ModelState.IsValid)
            {
                alert.Patient.RegisteredBy = User.Identity.Name;
                db.Alerts.Add(alert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alert);
        }
        //
        // GET: /Alerts/Edit/5

        public ActionResult Edit(int id)
        {
            var alert = db.Alerts.Find(id);
            return View(alert);
        }

        //
        // POST: /Alerts/Edit/5

        [HttpPost]
        public ActionResult Edit(Alert alert)
        {
            if (ModelState.IsValid)
            {
                alert.Patient.RegisteredBy = User.Identity.Name;
                db.Entry(alert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alert);
        }

        //
        // GET: /Alerts/Delete/5

        public ActionResult Delete(int id)
        {
            var alert = db.Alerts.Find(id);
            return View(alert);
        }

        //
        // POST: /Alerts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var alert = db.Alerts.Find(id);
            db.Alerts.Remove(alert);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

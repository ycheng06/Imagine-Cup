/*
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
using System.Diagnostics;

namespace MediviseMVC.Controllers
{
    [Authorize]
    public class AlertController : Controller
    {
        //
        // GET: /Alert/
        private MediviseEntities db = new MediviseEntities();
        //for now
        private TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public ActionResult Index()
        {
            //if(User.IsInRole("admin")) return RedirectToAction("Register", "Account");
            return View(db.Alerts.Include("Patient")
                                        .Include("AlertType").Where(alert => alert.Patient.RegisteredBy == User.Identity.Name).OrderBy(a => a.AlertDate).ToList());
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
            bool isAjax = Request.IsAjaxRequest();
            var alert = db.Alerts.Find(id);
            db.Alerts.Remove(alert);
            db.SaveChanges();
            if (isAjax)
            {
                var pid = alert.PatientId;
                IEnumerable<Alert> alerts = db.Alerts.Where(a => a.PatientId == pid).ToList();
                return PartialView("AlertList", alerts);
            }
            return RedirectToAction("Index");
        }
    //****JSON actions****************
        [HttpPost]
        public JsonResult AlertList(int jtStartIndex,int jtPageSize,string jtSorting=null)
        {
            try
            {
                List<Alert> alerts = db.Alerts.ToList();
                List<Alert> currentPage = alerts.Skip((jtStartIndex - 1) * jtPageSize).Take(jtPageSize).ToList();
                switch (jtSorting)
                {
                    case "AlertDate ASC":
                        currentPage = currentPage.OrderBy(a => a.AlertDate).ToList();
                        foreach (var a in currentPage)
                        {
                            Trace.WriteLine("Date: " + a.AlertDate.ToShortDateString());
                        }
                        break;
                    case "AlertDate DESC":
                        currentPage = currentPage.OrderByDescending(a => a.AlertDate).ToList();
                        break;
                    default:
                        break;
                }
                var jsonData = currentPage.Select(a => JsonizeAlert(a));
                return Json(new { Result = "OK", Records = jsonData, TotalRecordCount = alerts.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeleteAlert(int AlertId)
        {
            try
            {
                Alert alert = db.Alerts.Find(AlertId);
                db.Alerts.Remove(alert);
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult ListAlertsFor(int PatientId)
        {
            try
            {
                List<Alert> alerts = db.Patients.Find(PatientId).Alerts.ToList();
                var jsonData = alerts.Select(a => JsonizeAlert(a));
                return Json(new { Result = "OK", Records = jsonData });
            }
            catch (Exception ex){
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private Object JsonizeAlert(Alert a)
        {
            return new
            {
                AlertId = a.AlertId,
                PatientId = a.PatientId,
                FullName = a.Patient.FirstName + " " + a.Patient.LastName,
                AlertDate = a.AlertDate,
                AlertType = a.AlertType.Name
            };
        }
    }
}

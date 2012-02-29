using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBTracker.Models;
using System.Data.Objects.SqlClient;

namespace TBTracker.Controllers
{ 
    public class MessageController : Controller
    {
        private TrackerEntities db = new TrackerEntities();

        //
        // GET: /Message/

        public ViewResult Index()
        {
            var messages = db.Messages.Include(m => m.Patient);
            return View(messages.ToList());
        }

        //
        // GET: /Message/Details/5

        public ViewResult Details(int id)
        {
            Message message = db.Messages.Include("Patient").SingleOrDefault(x => x.MessageId == id);
            return View(message);
        }

        //
        // GET: /Message/Create

        public ActionResult Create()
        {
            IEnumerable<SelectListItem> items = db.Patients
                .Select(c => new SelectListItem
                    {
                        Value = SqlFunctions.StringConvert((decimal)c.PatientId),
                        Text = c.FirstName + " " + c.LastName
                    });
            ViewBag.PatientId = items;
            return View(new Message
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            });

        }

        //
        // POST: /Message/Create

        [HttpPost]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            IEnumerable<SelectListItem> items = db.Patients
                .Select(c => new SelectListItem
                    {
                        Value = SqlFunctions.StringConvert((decimal)c.PatientId),
                        Text = c.FirstName + " " + c.LastName
                    });
            ViewBag.PatientId = items;
            return View();
        }

        //
        // GET: /Message/Edit/5

        public ActionResult Edit(int id)
        {
            Message message = db.Messages.Find(id);
            IEnumerable<SelectListItem> items = db.Patients
                .Select(c => new SelectListItem
                    {
                        Value = SqlFunctions.StringConvert((decimal)c.PatientId),
                        Text = c.FirstName + " " + c.LastName,
                        Selected = (c.PatientId == message.PatientId)
                    });
            ViewBag.PatientId = items;
            return View(message);
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        public ActionResult Edit(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            IEnumerable<SelectListItem> items = db.Patients
                            .Select(c => new SelectListItem
                                {
                                    Value = SqlFunctions.StringConvert((decimal)c.PatientId),
                                    Text = c.FirstName + " " + c.LastName,
                                    Selected = (c.PatientId == message.PatientId)
                                });
            ViewBag.PatientId = items;
            return View(message);
        }

        //
        // GET: /Message/Delete/5

        public ActionResult Delete(int id)
        {
            Message message = db.Messages.Include("Patient").SingleOrDefault(x => x.MessageId == id);
            return View(message);
        }

        //
        // POST: /Message/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
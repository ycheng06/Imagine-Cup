using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwilioSharp.MVC3.Controllers;
using TwilioSharp.Request;
using TBTracker.Models;
using System.Data;

namespace TBTracker.Controllers
{
    public class ReminderController : TwiMLController
    {
        [HttpPost]
        public ActionResult TextResponse(TextRequest request)
        {
            //var answer = string.Format("{0} {1}", "Your phone number is ", request.From);
            //return TwiML(response => response.Sms(answer));

            //mark number off for the day
            string caller = request.From;

            record_response(caller);
            return null; //will not respond to a text

        }

        [HttpPost]
        public ActionResult CallResponse(CallRequest request)
        {
            //mark number off for the day
            string caller = request.From;

            record_response(caller);

            //hang up
            return TwiML(response => response.Say(request.From).Hangup());
        }

        private void record_response(string caller)
        {
            //mark number off for the day

            //look up caller in patient table, mark its ResponseReceived as true
            TrackerEntities db = new TrackerEntities();
            Patient patient = db.Patients.SingleOrDefault(x => x.Phone == caller);
            patient.ResponseReceived = true;
            db.Entry(patient).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}

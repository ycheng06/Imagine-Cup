/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwilioSharp.MVC3.Controllers;
using TwilioSharp.Request;
using MediviseMVC.Models;
using System.Data;

namespace MediviseMVC.Controllers
{
    public class ReminderController : TwiMLController
    {
        private static string RESPONSE_MESSAGE = "Thank you for taking your medications today";
        private MediviseEntities db = new MediviseEntities();

        [HttpPost]
        public ActionResult TextResponse(TextRequest request)
        {
            //strip the leading "+1"
            string caller = request.From.Substring(2);
            record_response(caller);
            
            return TwiML(response => response.Sms(RESPONSE_MESSAGE));
        }

        [HttpPost]
        public ActionResult CallResponse(CallRequest request)
        {
            //strip the leading "+1"
            string caller = request.From.Substring(2);
            record_response(caller);
            //hang up
            return TwiML(response => response.Say(RESPONSE_MESSAGE).Hangup());
        }

        //takes in number without the "+1" at the beginning
        private void record_response(string caller)
        {
            //mark number off for the day

            //look up caller in patient table, mark its ResponseReceived as true
            Patient patient = db.Patients.SingleOrDefault(x => x.Phone == caller);
            if (patient != null) //also need to check the time when the patient calls in
            {
                patient.ResponseReceived = true;
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}

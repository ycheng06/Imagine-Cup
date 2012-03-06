using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Twilio;
using System.Diagnostics;
using MediviseMVC.Models;

namespace MediviseMVC
{
    public class ReminderSender : IJob
    {
        const string accountSid = "ACc27d757e2015405a8da8ad6c4966a3e7";
        const string authToken = "ba5fef2eae6c8e0beec4f76239df114f";
        const string twiliNumber = "+16177022951";
        private TwilioRestClient twilio; 
        public ReminderSender() 
        {
            twilio = new TwilioRestClient(accountSid, authToken);
        }

        public virtual void Execute(JobExecutionContext context)
        {
            //string phone = "+16469266783";
            //string message = "This is a test message sent at " + DateTime.Now.ToLocalTime();
            //var msg = twilio.SendSmsMessage(twiliNumber, phone, message);
            //Trace.WriteLine("Sent message <" + message + "> to " + phone + " at " + DateTime.Now.ToLocalTime());
            traverse();
        }
        private void traverse()
        {
            var db = new MediviseEntities();
            var patients = from p in db.Patients
                           select p;
            foreach (var p in patients)
            {
                string title = (p.Gender == "Male") ? "Mr. " : "Mrs. ";
                List<string> drugs = new List<string>();
                foreach (var d in p.Drugs)
                {
                    string msg = "Please take " + d.DrugInfo.Name + " "+d.TimesPerWeek + " Times Per Week\n";
                    drugs.Add(msg);
                }
                var sms = title + p.LastName +":\n";
                foreach (var d in drugs)
                {
                    sms += d;
                }
                Trace.WriteLine(sms);
            }
        }

        public void send_message(string phone, string message)
        {
            var msg = twilio.SendSmsMessage(twiliNumber, phone, message);
            Trace.WriteLine("Sent message <" + message + "> to " + phone + " at " + DateTime.Now.ToLocalTime());
        }
            
    }
}
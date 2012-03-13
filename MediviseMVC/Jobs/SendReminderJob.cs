/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediviseMVC.Models;
using Quartz;
using MediviseMVC.Twilio;
using System.Diagnostics;
using System.Data;
namespace MediviseMVC.Jobs
{
    public class SendReminderJob : IJob
    {
        private TwilioSender sender;
        private MediviseEntities db;
        private MessageConstructor msgBuilder = new MessageConstructor();
        public void Execute (IJobExecutionContext context)
        {
            sender = new TwilioSender();
            db = new MediviseEntities();
            sendReminders();
        }
        private void sendReminders()
        {
            var patients = db.Patients.ToArray();
            foreach (Patient p in patients)
            {
                if (p.ResponseReceived == false)
                {
                    string warning = "You forgot yesterday's medication!";
                    sender.SendSMS(p.Phone, warning);
                    constructAlert(p);
                }
                p.ResponseReceived = false;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                string message = msgBuilder.ConstructMsg(p);
                Trace.WriteLine(p.Phone, "PHONE NUMBER ********************");
                sender.SendSMS(p.Phone, message);
            }
        }
        private void constructAlert(Patient p)
        {
            Alert a = new Alert
            {
                PatientId = p.PatientId,
                AlertDate = DateTime.UtcNow,
                AlertTypeId = 1 // 1 = "missed medication"
            };
            db.Alerts.Add(a);
            db.SaveChanges();
        }

    }
}
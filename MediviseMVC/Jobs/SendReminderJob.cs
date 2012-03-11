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
        private TwilioSender sender = new TwilioSender();
        private MediviseEntities db = new MediviseEntities();
        private MessageConstructor msgBuilder = new MessageConstructor();
        public void Execute (JobExecutionContext context)
        {
            sendReminders();
        }
        private void sendReminders()
        {
            var patients = db.Patients.ToList();
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
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
                    //sender.SendSMS(p.Phone, warning);
                    Trace.WriteLine(warning);
                    constructAlert(p);
                }
                  /*  string p1 = "From off a hill whose concave womb reworded" +
                                    "A plaintful story from a sist ring vale" +
                                    "My spirits t'attend this double voice accorded," +
                                   " And down I laid to list the sad-tuned tale," +
                                   " Ere long espied a fickle maid full pale, " +
                                   " Tearing of papers, breaking rings atwain, " +
                                   " Storming her world with sorrow's wind and rain. " +
                                   " Upon her head a platted hive of straw, " +
                                   " Which fortified her visage from the sun, " +
                                   " Whereon the thought might think sometime it saw"  +
                                   " The carcase of a beauty spent and done. " +
                                   " Time had not scythed all that youth begun, " +
                                   " Nor youth all quit, but spite of heaven's fell rage" +
                                   " Some beauty peeped through lattice of seared age\n";
                    string p2 = "Sometimes her levelled eyes their carriage ride\n" +
                           " As they did batt'ry to the spheres intend\n" +
                           " Sometime diverted their poor balls are tied\n" +
                           " To th'orbed earth; sometimes they do extend\n" +
                           " Their view right on; anon their gazes lend\n" +
                           " To every place at once, and nowhere fixed\n" +
                           " The mind and sight distractedly commixed\n";*/
                p.ResponseReceived = false;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                string message = msgBuilder.ConstructMsg(p);
                Trace.WriteLine(message);
                //sender.SendSMS(p.Phone, message+p1+p2);
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
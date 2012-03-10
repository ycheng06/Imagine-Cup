using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediviseMVC.Models;
using Quartz;
using MediviseMVC.Twilio;

namespace MediviseMVC.Jobs
{
    //things to do:
    //                - please test using a null field and also a number that is not 10 digits (for US numbers)
    //remove test in Execute

    //later: add a confirmation message on successful registration

    public class TaskManager : ReminderSender
    {
        private MediviseEntities db = new MediviseEntities();
        private TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        private TimeSpan reminder_time = new TimeSpan(9, 0, 0); //9:00AM
        private TimeSpan follow_up_time = new TimeSpan(15, 0, 0); //15:00 = 3:00PM
        private MessageConstructor msg = new MessageConstructor();
        private TwilioSender twilio = new TwilioSender();

        public override void Execute(JobExecutionContext context)
        {
            TimeSpan timeFired = TimeZoneInfo.ConvertTimeFromUtc((DateTime)context.FireTimeUtc,userTimeZone).TimeOfDay;
            //testing
            if (true)
            {
                first_reminder();
            }
            else if (timeFired.Equals(reminder_time))
            {
                first_reminder();
            }

            else if (timeFired.Equals(follow_up_time))
            {
                first_warning();
            }
        }

        private void first_reminder()
        {
            var patients = db.Patients.ToList();
            foreach (Patient p in patients)
            {
                if (p.ResponseReceived == false)
                {
                    string warning = "You forgot yesterday's medication!";
                    twilio.SendSMS(p.Phone, warning);
                    missed_drug_intake_alert(p);
                }
                    string p1 = "From off a hill whose concave womb reworded" +
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
                           " The mind and sight distractedly commixed\n";
                p.ResponseReceived = false;
                string message = msg.ConstructMsg(p);
                twilio.SendSMS(p.Phone, message+p1+p2);
            }
        }

        private void first_warning()
        {
            //for each patient, if its ResponseReceived field is false, text them again
            var missed_patients = db.Patients.Where(patient => patient.ResponseReceived == false).ToList();
            foreach (Patient p in missed_patients)
            {
                //add warning to the beginning
                string message = "Please take your medicine, and be sure to respond this time!\n";
                twilio.SendSMS(p.Phone, message);
                message = msg.ConstructMsg(p);
                twilio.SendSMS(p.Phone, message);

                //more work
                string family_message = String.Format(
                    p.FirstName + " has forgotten to take {0} medication today. Please immediately remind {1} to take the treatment.",
                    p.Gender == "Male" ? "his" : "her",
                    p.Gender == "Male" ? "him" : "her");
                twilio.SendSMS(p.FamilyPhone1, family_message);
                twilio.SendSMS(p.FamilyPhone2, family_message);
            }
        }
        private void pre_checkup_reminder()
        {
        }
        private void missed_drug_intake_alert(Patient p)
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
        private void missed_checkup_alert()
        {
        }

    }
}
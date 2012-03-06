using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TBTracker.Models;
using Quartz;
using TBTracker.Twilio;

namespace TBTracker.Jobs
{
    //things to do:
    //                - please test using a null field and also a number that is not 10 digits (for US numbers)
    //remove test in Execute

    //later: add a confirmation message on successful registration

    public class TaskManager : ReminderSender
    {
        private TrackerEntities db = new TrackerEntities();
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
                p.ResponseReceived = false;
                string message = msg.ConstructMsg(p);
                twilio.SendSMS(p.Phone, message);
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
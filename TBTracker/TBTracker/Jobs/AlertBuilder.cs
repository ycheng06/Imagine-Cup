﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TBTracker.Models;
using Quartz;

namespace TBTracker.Jobs
{
    public class AlertBuilder : ReminderSender
    {
        private TrackerEntities db = new TrackerEntities();
        private TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        private TimeSpan first_follow_up = new TimeSpan(15, 0, 0); //15:00 = 3:00PM
        private TimeSpan second_follow_up = new TimeSpan(18, 0, 0); //18:00 = 6:00PM

        public override void Execute(JobExecutionContext context)
        {
            TimeSpan timeFired = TimeZoneInfo.ConvertTimeFromUtc((DateTime)context.FireTimeUtc,userTimeZone).TimeOfDay;
            //testing
            if (true)
            {
                additional_drug_intake_reminder();
            }

            else if (timeFired.Equals(first_follow_up))
            {
                additional_drug_intake_reminder();
            }
            else if (timeFired.Equals(second_follow_up))
            {
                missed_drug_intake_alert();
            }
        }

        
        private void additional_drug_intake_reminder()
        {
            //for each patient, if its ResponseReceived field is false, text them again
            var missed_patients = db.Patients.Where(patient => patient.ResponseReceived == false).ToList();
            foreach (Patient p in missed_patients)
            {
                string message = "Please remember to take your # medicines today. After you are done, please call or send a blank text message to this number.";
                send_message(p.Phone, message);

                string family_message = String.Format(
                    p.FirstName + " has forgotten to take {0} medication today. Please immediately remind {1} to take the treatment.",
                    p.Gender=="Male"?"his":"her",
                    p.Gender=="Male"?"him":"her");
                send_message(p.FamilyPhone1, family_message);
                send_message(p.FamilyPhone2, family_message);
            }
        }
        private void pre_checkup_reminder()
        {
        }
        private void missed_drug_intake_alert()
        {
            //for each patient, if its ResponseReceived field is false, make an alert
            var missed_patients = db.Patients.Where(patient => patient.ResponseReceived == false).ToList();
            foreach (Patient p in missed_patients)
            {
                Alert a = new Alert {
                    PatientId = p.PatientId,
                    AlertDate = DateTime.UtcNow,
                    AlertTypeId = 1 // 1 = "missed medication"
                };
                db.Alerts.Add(a);
                db.SaveChanges(); //can I save changes at the very end instead of for each loop?
            }
        }
        private void missed_checkup_alert()
        {
        }
    }
}
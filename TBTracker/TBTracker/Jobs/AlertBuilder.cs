using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TBTracker.Models;

namespace TBTracker.Jobs
{
    public class AlertBuilder
    {
        private TrackerEntities db = new TrackerEntities();
        private ReminderSender rs = new ReminderSender();

        private void additional_drug_intake_reminder()
        {
            //for each patient, if its ResponseReceived field is false, text them again
            var missed_patients = db.Patients.Where(patient => patient.ResponseReceived == false).ToList();
            foreach (Patient p in missed_patients)
            {
                string message = "";
                rs.send_message(p.Phone, message);

                string family_message = 
                    p.FirstName + " has forgotten to take his/her medication today. Please immediately remind him/her to take the treatment.";
                rs.send_message(p.FamilyPhone1, message);
                rs.send_message(p.FamilyPhone2, message);
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
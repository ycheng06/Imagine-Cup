﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediviseMVC.Models;
using Quartz;
using MediviseMVC.Twilio;
using System.Diagnostics;

namespace MediviseMVC.Jobs
{
    public class SendWarningJob : IJob
    {
        private TwilioSender sender = new TwilioSender();
        private MediviseEntities db = new MediviseEntities();
        private MessageConstructor msgBuilder = new MessageConstructor();

        public void Execute(JobExecutionContext context)
        {
            int id = (int)context.JobDetail.JobDataMap["pid"];
            sendWarnings();
        }

        private void sendWarnings()
        {
            //for each patient, if its ResponseReceived field is false, text them again
            Trace.WriteLine("About to send warnings!!!Excited!!!");
            var missed_patients = db.Patients.Where(patient => patient.ResponseReceived == false).ToList();
            foreach (Patient p in missed_patients)
            {
                //add warning to the beginning
                string message = "Please take your medicine, and be sure to respond this time!\n";
                //sender.SendSMS(p.Phone, message);
                Trace.WriteLine(message);
                message = msgBuilder.ConstructMsg(p);
                //sender.SendSMS(p.Phone, message);
                Trace.WriteLine(message);

                //more work
                string family_message = String.Format(
                    p.FirstName + " has forgotten to take {0} medication today. Please immediately remind {1} to take the treatment.",
                    p.Gender == "Male" ? "his" : "her",
                    p.Gender == "Male" ? "him" : "her");
                Trace.WriteLine(String.Format("Family Message: " + family_message));
                //sender.SendSMS(p.FamilyPhone1, family_message);
                //sender.SendSMS(p.FamilyPhone2, family_message);

            }
        }
    }
}
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

namespace MediviseMVC.Jobs
{
    public class SendWarningJob : IJob
    {
        private TwilioSender sender = new TwilioSender();
        private MediviseEntities db = new MediviseEntities();
        private MessageConstructor msgBuilder = new MessageConstructor();
        private int warningHour = 21; //warning is sent at 21:00 or 9pm

        public void Execute(IJobExecutionContext context)
        {
            sendWarnings();
        }

        private void sendWarnings()
        {

            //for each patient, if its ResponseReceived field is false, text them again
            var missed_patients = db.Patients.Where(patient => patient.ResponseReceived == false).ToList();
            foreach (Patient p in missed_patients)
            {
                //see what time it is in user TimeZone
                TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(p.TimeZone);
                DateTime timeInUserTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);

                if (timeInUserTimeZone.Hour == warningHour)
                {
                    //add warning to the beginning
                    string message = "Please take your medicine, and be sure to call us back this time!\n";
                    sender.SendSMS(p.Phone, message);
                    Trace.WriteLine(p.Phone);
                    //message = msgBuilder.ConstructMsg(p);
                    //sender.SendSMS(p.Phone, message);

                    //more work
                    string family_message = String.Format(
                        p.FirstName + " has forgotten to take {0} medication today. Please immediately remind {1} to take the treatment.",
                        p.Gender == "Male" ? "his" : "her",
                        p.Gender == "Male" ? "him" : "her");
                    sender.SendSMS(p.FamilyPhone1, family_message);
                    Trace.WriteLine(p.FamilyPhone1);
                    if (p.FamilyPhone2 != null)
                    {
                        Trace.WriteLine(p.FamilyPhone2);
                        sender.SendSMS(p.FamilyPhone2, family_message);
                    }
                }
            }
        }
    }
}
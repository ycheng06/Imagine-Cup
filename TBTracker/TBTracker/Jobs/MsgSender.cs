using System.Linq;
using System.Web;
using System.Text;
using Quartz;
using Twilio;
using System.Diagnostics;
using TBTracker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TBTracker.Twilio;

namespace TBTracker.Jobs
{
    public class MsgSender : IJob
    {
        TrackerEntities db = new TrackerEntities();
        TwilioSender smsClient = new TwilioSender();
        public void Execute(JobExecutionContext context)
        {
            SendReminders();
        }
        
        public void SendReminders()
        {
            foreach(var p in db.Patients.ToList())
            {
                string msg = ConstructMsg(p);
                Trace.WriteLine("About to text Jason");
                Trace.WriteLine(msg);
                smsClient.SendSMS("+16469266783", msg);
            }
        }
       
        
        private string ConstructMsg(Patient p)
        {
            DateTime now = DateTime.UtcNow;
            StringBuilder msg = ConstructTitle(p);
            int num_drugs = p.Drugs.Count;
            msg.Append("Please follow the instructions:\n");
            if (num_drugs == 0)
            {
                msg.Append("No Medicines for today\n");
            }
            else
            {
                msg.AppendFormat("Take {0} medicines as prescribed for you\n", num_drugs);
            }
            List<Test> testMessages = p.Tests.ToList().FindAll(t => dateInRange(t.TestDate,now,now.AddDays(10)));
            if (testMessages.Count > 0)
            {
                msg.Append("You have tests on the following dates: ");
                foreach (var t in testMessages)
                {
                    msg.Append(ConstructTestMsg(t));  
                }
                msg.Remove(msg.Length - 2, 2);
            }
            return msg.ToString();

        }
         
        private StringBuilder ConstructTestMsg(Test test)
        {
            StringBuilder testMsg = new StringBuilder();
            testMsg.AppendFormat("{0}, ",test.TestDate.ToShortDateString());
            return testMsg;
        }
        private StringBuilder ConstructTitle(Patient p)
        {
            StringBuilder title = new StringBuilder();
            switch (p.Gender)
            {
                case "Male":
                    title.Append("Dear Mr. ");
                    title.Append(p.LastName);
                    break;
                case "Female":
                    title.Append("Dear Mrs. ");
                    title.Append(p.LastName);
                    break;
                default:
                    title.Append("Dear ");
                    title.Append(p.FirstName);
                    break;
            }
            title.AppendLine();
            return title;
        }
        private bool dateInRange(DateTime now, DateTime from, DateTime to)
        {
            return (now >= from && now <= to);
        }
         
    }
}
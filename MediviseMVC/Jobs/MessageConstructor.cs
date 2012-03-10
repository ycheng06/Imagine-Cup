using System.Linq;
using System.Web;
using System.Text;
using Quartz;
using Twilio;
using System.Diagnostics;
using MediviseMVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using MediviseMVC.Twilio;

namespace MediviseMVC.Jobs
{
    public class MessageConstructor
    {
        
        public string ConstructMsg(Patient p)
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
            List<Message> customMessages = p.Messages.ToList().FindAll(m => dateInRange(now, m.StartDate, m.EndDate));
            if (customMessages.Count > 0)
            {
                msg.AppendLine();
                msg.Append("Note:");
                foreach (var c in customMessages)
                {
                    msg.AppendLine();
                    msg.Append(c.MessageText);
                }
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
                    title.Append("Dear Ms. ");
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
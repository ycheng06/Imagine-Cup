/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
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
        
        public string ConstructMsg(Patient p, int num_drugs)
        {
            DateTime now = DateTime.UtcNow;
            StringBuilder msg = ConstructTitle(p);
            //int num_drugs = p.Drugs.Count;
            if (num_drugs == 0)
            {
                msg.Append("No medicines for today.\n");
            }
            else if (num_drugs == 1)
            {
                msg.Append("Take 1 medicine as prescribed for you.\n");
            }
            else
            {
                msg.AppendFormat("Take {0} medicines as prescribed for you.\n", num_drugs);
            }
            List<Test> testMessages = p.Tests.ToList().FindAll(t => dateInRange(t.TestDate,now,now.AddDays(10)));
            if (testMessages.Count > 0)
            {
                msg.Append("Checkups on the following dates: ");
                foreach (var t in testMessages)
                {
                    msg.Append(ConstructTestMsg(t));  
                }
                msg.Remove(msg.Length - 2, 2);
                msg.AppendLine();
            }
            List<Message> customMessages = p.Messages.ToList().FindAll(m => dateInRange(now, m.StartDate, m.EndDate));
            if (customMessages.Count > 0)
            {
                msg.Append("Note:\n");
                foreach (var c in customMessages)
                {
                    msg.Append(c.MessageText);
                    msg.AppendLine();
                }
                msg.Remove(msg.Length - 1, 1);
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
            title.AppendFormat("Hi {0}:\n", p.FirstName);
            return title;
        }
        private bool dateInRange(DateTime now, DateTime from, DateTime to)
        {
            return (now.Date >= from.Date && now.Date <= to.Date);
        }
         
    }
}
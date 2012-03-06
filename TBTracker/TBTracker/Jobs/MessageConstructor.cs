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
            /*
            string foo1 = "lots of new lines\n";
            string foo2 = "lots of new lines again\n";
            string foo3 = "lots of new lines once more\n";
            string foo4 = "lots of new lines ahahha\n";
            string foo5 = "lots of new lines opposfioasd\n";
            string foo6 = "lots of new lines woohooo\n";
            string blob = "justgivingyoujunktakethejunkjunkgarbagewhateverahahahhahahnanannananwawawawaolalalalkla;sjfalkdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd;;;;;;;;;;alskjdflak;sjdflkasjdflka;jsfiowaejfkajsdlfkajsdlfkajsldfja;lskdjfa;lksdjf;asdf\njunkjunkjunkjunkjunkaakjs;dflkajsdlkfajs;asdfaskjdlhfakjsdhfalkjsdhfalkjshfdalkjsdhfalkjsdhflkasjdhfalkjshfdlkajshdlkjfahdslkfjahdskfajhsdkjfahslkdfjhalksdhfalkjdshfalkhfdslkajhdsflkajlf\naskdjf;alksjdf;laksjdf;lakjdsf;lkasjfda;lksjfd;laksdjflkajfds;lkajfa;lkjdsflkajsdflkja;lsjfda;lkdsjf;aljdsf;aljds;dlfkajs;dlkfjas;lkdfja;lkdsfja;lskjfda;lskjdfa;lksjdf;lkasjdf;lkajsd;flkjaoisuerpquwepoiruqwepoiruqpweiurqpoiewurqpoiuewrpqoiwuerpoiquewpoiquewpoiquewoiurpoiquwrepqiourepoqurqoiuerqpoiwureqpoiueroiqpuewrpoiqueoirquflkajs;lfajslkdfjalksdjfalkjdsflkajdsflajfdla;jdf;lakjds;lkfajds;lkfasjd;fa;lkjfdlka;jdslksjfdlkasjdflkasjdf;lkasjdf;laksjdf29874098qhfriuaoyreuoiaywiuashkjdfhsajkdhfakjshfdalkjshfdalkjshfdalkhfdsalkjshfdalkjshfdalkjhfdakjshfda\nlkjshfdakjhsfdlkjahfdsl";
            string plop = new String(blob.ToCharArray());
            msg.Append(foo1); msg.Append(foo2); msg.Append(foo3); msg.Append(foo4); msg.Append(foo5); msg.Append(foo6); msg.Append(blob);
            msg.Append(plop);
            */
            
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
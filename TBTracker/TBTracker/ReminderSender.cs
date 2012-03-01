using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Twilio;
using System.Diagnostics;
namespace TBTracker
{
    public class ReminderSender : IJob
    {
        const string accountSid = "ACc27d757e2015405a8da8ad6c4966a3e7";
        const string authToken = "ba5fef2eae6c8e0beec4f76239df114f";
        const string twiliNumber = "+16177022951";
        public ReminderSender() { }

        public void Execute(JobExecutionContext context)
        {
            //var twilio = new TwilioRestClient(accountSid, authToken);
            //var msg = twilio.SendSmsMessage(twiliNumber, "+16469266783", "Testing");
            Trace.WriteLine("wtf");
        }
            
    }
}
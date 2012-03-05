using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Twilio;
using System.Diagnostics;
using TBTracker.Models;
using TBTracker.Jobs;

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
            //string phone = "+16469266783";
            //string message = "This is a test message sent at " + DateTime.Now.ToLocalTime();
            //var msg = twilio.SendSmsMessage(twiliNumber, phone, message);
            //Trace.WriteLine("Sent message <" + message + "> to " + phone + " at " + DateTime.Now.ToLocalTime());
            //traverse();
            MsgSender sender = new MsgSender();
            sender.SendReminders();
        }
    }
}
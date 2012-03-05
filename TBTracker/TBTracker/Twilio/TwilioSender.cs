using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

namespace TBTracker.Twilio
{
    class TwilioSender
    {
        private const string accountSid = "ACc27d757e2015405a8da8ad6c4966a3e7";
        private const string authToken = "ba5fef2eae6c8e0beec4f76239df114f";
        const string twiliNumber = "+16177022951";
        TwilioRestClient smsClient = new TwilioRestClient(accountSid, authToken);

        public void SendSMS(string receiver,string msg)
        {
            smsClient.SendSmsMessage(twiliNumber, receiver, msg);     
        }

    }
}
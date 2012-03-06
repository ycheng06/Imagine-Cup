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
        private const int us_num_digits = 10;
        TwilioRestClient smsClient = new TwilioRestClient(accountSid, authToken);

        public void SendSMS(string receiver,string msg)
        {
            if (has_enough_digits(receiver, us_num_digits))
            {
                smsClient.SendSmsMessage(twiliNumber, receiver, msg);
            }
        }
        private bool has_enough_digits(string phone, int digits)
        {
            if (phone != null && phone.Length == digits)
            {
                return true;
            }
            return false;
        }

    }
}
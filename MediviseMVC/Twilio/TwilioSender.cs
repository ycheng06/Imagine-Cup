/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;
using System.Text;
using System.Diagnostics;

namespace MediviseMVC.Twilio
{
    class TwilioSender
    {
        private const int MAX_CHARS = 160;
        private const string accountSid = "ACc27d757e2015405a8da8ad6c4966a3e7";
        private const string authToken = "ba5fef2eae6c8e0beec4f76239df114f";
        private const string twiliNumber = "+16177022951";
        private const int us_num_digits = 10;
        TwilioRestClient smsClient = new TwilioRestClient(accountSid, authToken);

        public void SendSMS(string receiver, string msg)
        {
            //Trace.WriteLine(String.Format("Incoming msg length is {0}", msg.Length));
            if (has_enough_digits(receiver, us_num_digits))
            {
                if (msg.Length <= MAX_CHARS)
                {
                    smsClient.SendSmsMessage(twiliNumber, receiver, msg);
                    //Trace.WriteLine(msg);
                }
                else
                {
                    foreach (var frag in truncateMsg(msg, MAX_CHARS))
                    {
                        smsClient.SendSmsMessage(twiliNumber, receiver, frag);
                        //Trace.WriteLine(frag);
                        //Trace.WriteLine("newlines!!");
                    }
                }
            }
        }
        private bool has_enough_digits(string phone, int digits)
        {
            return (phone != null && phone.Length == digits) ? true : false;
        }
        private List<string> truncateMsg(string msg, int max_len)
        {
            List<string> fragments = new List<string>();
            string[] strs = msg.Split(new Char[] { '\n' });
            StringBuilder sb = new StringBuilder();
            foreach (var s in strs)
            {
                if (sb.Length + s.Length + 1 <= max_len)
                {
                    Trace.WriteLine("Eating a new string");
                    sb.Append(s);
                    sb.AppendLine();
                }
                else if (s.Length + 1 <= max_len)
                {
                    Trace.WriteLine("clearing sb and eating a string");
                    fragments.Add(sb.ToString());
                    sb.Clear();
                    sb.Append(s);
                    sb.AppendLine();
                }
                else
                {
                    Trace.WriteLine("string tooooo long");
                    if (sb.Length != 0)
                    {
                        fragments.Add(sb.ToString());
                        sb.Clear();
                    }
                    int splits = (s.Length / max_len) + (s.Length % 2);
                    for (int i = 0; i < splits; i++)
                    {
                        int strip_from = i * max_len;
                        int strip_by = (strip_from + max_len > s.Length) ? (s.Length - strip_from) : max_len;
                        string substring = s.Substring(strip_from, strip_by);
                        fragments.Add(substring);
                    }
                }
            }
            if (sb.Length != 0)
            {
                fragments.Add(sb.ToString());
            }
            return fragments;
        }
    }
}

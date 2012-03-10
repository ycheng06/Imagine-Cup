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
            Trace.WriteLine(String.Format("Incoming msg length is {0}", msg.Length));
            if (has_enough_digits(receiver, us_num_digits))
            {
                if (msg.Length <= MAX_CHARS)
                {
                    //smsClient.SendSmsMessage(twiliNumber, receiver, msg);
                    Trace.WriteLine(msg);
                }
                else
                {
                    foreach (var frag in truncateMsg(msg, MAX_CHARS))
                    {
                        //smsClient.SendSmsMessage(twiliNumber, receiver, frag);
                        Trace.WriteLine(frag);
                        Trace.WriteLine("newlines!!");
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
            Trace.WriteLine("Starting split examination");
            foreach (var s in strs)
            {
                Trace.WriteLine("*****************************");
               Trace.WriteLine(s);
                Trace.WriteLine("*****************************");
            }
            Trace.WriteLine("Ending split examination");
            StringBuilder sb = new StringBuilder();
            foreach (var s in strs)
            {
                if (sb.Length + s.Length + 1 <= max_len)
                {
                    sb.Append(s);
                    sb.AppendLine();
                }
                else if (s.Length + 1 <= max_len)
                {
                    fragments.Add(sb.ToString());
                    sb.Clear();
                    sb.Append(s);
                    sb.AppendLine();
                }
                else
                {
                    if (sb.Length != 0)
                    {
                        fragments.Add(sb.ToString());
                        sb.Clear();
                    }
                    int splits = (s.Length / max_len) + 1;
                    for (int i = 0; i < splits; i++)
                    {
                        int strip_from = i * max_len;
                        int strip_by = (strip_from + max_len > s.Length) ? (s.Length - strip_from) : max_len;
                        try
                        {
                            string substring = s.Substring(strip_from, strip_by);
                            fragments.Add(substring);
                        }
                        catch
                        {
                            Trace.WriteLine(String.Format("strip starting {0}, and with max_len {1}\n", strip_from, max_len));
                            Trace.WriteLine(String.Format("Length of string is: {0}, num_splits: {1}", s.Length, splits));
                            Trace.WriteLine(String.Format("striping by: {0}",strip_by));
                        }
                    }
                }
            }
            return fragments;
        }
    }
}
            /*
            int splits;
            if (msg.Length % max_len == 0)
            {
                splits = msg.Length / max_len;
            }
            else
            {
                splits = msg.Length / max_len + 1;
            }

            for (int i = 0; i < msg.Length; i += max_len)
            {
                string substring;
                if (msg.Length - i < max_len)
                {
                    substring = msg.Substring(i, msg.Length - i);
                }
                else
                {
                    substring = msg.Substring(i, max_len);
                }
                fragments.Add(substring);
            }

            return fragments;
        }
            */
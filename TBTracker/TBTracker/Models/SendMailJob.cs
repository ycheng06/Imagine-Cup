using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.IO;

namespace TBTracker.Models
{
    /// <summary>
    /// Summary description for SendMailJob
    /// </summary>
    public class SendMailJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            WriteToFile();
        }
        private static void WriteToFile()
        {
            StreamWriter SW;
            SW = File.CreateText("C:\\Documents and Settings\\chendur\\Desktop\\samp.txt");
            SW.WriteLine("God is greatest of them all");
            SW.WriteLine("This is second line");
            SW.WriteLine(getIndianStandardTime());
            SW.Close();
        }
        public static DateTime getIndianStandardTime()
        {
            TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
        }
    }
}
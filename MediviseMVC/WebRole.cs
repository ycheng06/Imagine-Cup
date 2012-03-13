using System;
using System.Collections.Generic;
using System.Linq;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Impl;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using MediviseMVC.Jobs;
using System.Threading;
using System.Diagnostics;

namespace MediviseMVC
{
    public class WebRole : RoleEntryPoint
    {
        //private ISchedulerFactory schedulePool;

        public override bool OnStart()
        {

            ////DateTime reminderTime = new DateTime(2012, 3, 11, 21, 0, 0);
            ////DateTime convertedR = TimeZoneInfo.ConvertTime(reminderTime, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"), TimeZoneInfo.Utc);
            ////Trace.WriteLine(convertedR.ToShortTimeString(), "Time ****************");

            ////DateTime warningTime = new DateTime(2012, 3, 11, 21, 5, 0);
            ////DateTime convertedW = TimeZoneInfo.ConvertTime(warningTime, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"), TimeZoneInfo.Utc); 

            //////Set up reminder trigger
            ////Trigger reminderTrigger = TriggerUtils.MakeDailyTrigger("reminderTrigger", convertedR.Hour, convertedR.Minute);
            ////reminderTrigger.StartTimeUtc = DateTime.UtcNow;
            ////sched.ScheduleJob(makeReminder, reminderTrigger);

            //////set up warning trigger
            ////Trigger warningTrigger = TriggerUtils.MakeDailyTrigger("warningTrigger", convertedW.Hour, convertedW.Minute);
            ////warningTrigger.StartTimeUtc = DateTime.UtcNow; 
            ////sched.ScheduleJob(makeWarning, warningTrigger);

            return base.OnStart();
        }
    }
}

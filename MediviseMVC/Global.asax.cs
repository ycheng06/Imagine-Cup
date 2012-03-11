﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using MediviseMVC.Models;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics;
using MediviseMVC.Jobs;

namespace MediviseMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            //Database.SetInitializer<MediviseEntities>(new Seeder());
            Scheduler();
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.DefaultBinder = new DateTimeConversionBinder();
        }

        private void Scheduler()
        {
            ISchedulerFactory schedulePool = new StdSchedulerFactory();
            IScheduler sched = schedulePool.GetScheduler();
            sched.Start();

            //construct job info
            JobDetail makeReminder = new JobDetail("reminder", null, typeof(SendReminderJob));
            JobDetail makeWarning = new JobDetail("warning", null, typeof(SendWarningJob));

           //10 AM in the morning
            DateTime reminderTime = new DateTime(1, 1, 1, 10, 0, 0);
            DateTime convertedR = TimeZoneInfo.ConvertTime(reminderTime, TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time"), TimeZoneInfo.Local);

            //2 PM in the afternoon
            DateTime warningTime = new DateTime(1, 1, 1, 14, 0, 0);
            DateTime convertedW = TimeZoneInfo.ConvertTime(warningTime, TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time"), TimeZoneInfo.Local); 

            //Set up reminder trigger
            Trigger reminderTrigger = TriggerUtils.MakeDailyTrigger("reminderTrigger", convertedR.Hour , convertedR.Minute);
            reminderTrigger.StartTimeUtc = DateTime.UtcNow;
            sched.ScheduleJob(makeReminder, reminderTrigger);
            //set up warning trigger
            Trigger warningTrigger = TriggerUtils.MakeDailyTrigger("warningTrigger", convertedW.Hour, convertedW.Minute);
            warningTrigger.StartTimeUtc = DateTime.UtcNow; 
            sched.ScheduleJob(makeWarning, warningTrigger);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using MediviseMVC.Models;
using System.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Impl;
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
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.DefaultBinder = new DateTimeConversionBinder();
            //Scheduler();
        }

        private void Scheduler()
        {
            ISchedulerFactory schedulePool = new StdSchedulerFactory();
            IScheduler sched = schedulePool.GetScheduler();
            sched.Start();

            JobDetailImpl makeReminder = new JobDetailImpl("reminder", typeof(SendReminderJob));
            JobDetailImpl makeWarning = new JobDetailImpl("warning", typeof(SendWarningJob));

            /*
             * Only support eastern timezone for prototype
             * 10 AM Reminder
             * 5 PM Warning
             */ 
            CronTriggerImpl reminderTrigger = new CronTriggerImpl("reminderTrigger");
            reminderTrigger.CronExpressionString = "0 0 14 * * ?";
            reminderTrigger.StartTimeUtc = DateTime.UtcNow;
            sched.ScheduleJob(makeReminder, reminderTrigger);

            CronTriggerImpl warningTrigger = new CronTriggerImpl("warningTrigger");
            warningTrigger.CronExpressionString = "0 0 21 * * ?";
            reminderTrigger.StartTimeUtc = DateTime.UtcNow;
            sched.ScheduleJob(makeWarning, warningTrigger);

        }

    }
}
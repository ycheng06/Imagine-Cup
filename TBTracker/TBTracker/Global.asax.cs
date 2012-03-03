using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using TBTracker.Models;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics;

namespace TBTracker
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
                new { controller = "Alert", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            Database.SetInitializer<TrackerEntities>(new Seeder());
            Scheduler();
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.DefaultBinder = new DateTimeConversionBinder();

            //delete as required:
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();
            // construct job info
            JobDetail jobDetail = new JobDetail("myReminderSender", typeof(ReminderSender));
            // fire every day at 06:00
            //Trigger trigger = TriggerUtils.MakeDailyTrigger(06, 00);
            SimpleTrigger trigger2 = new SimpleTrigger("myTrigger",
                                    null,
                                    DateTime.UtcNow,
                                    null,
                                    1, //SimpleTrigger.RepeatIndefinitely,
                                    TimeSpan.FromSeconds(1));
            //Trigger trigger = TriggerUtils.MakeHourlyTrigger();
            //// start on the next even hour
            //trigger.StartTimeUtc = TriggerUtils.GetEvenHourDate(DateTime.UtcNow);  
            //trigger.Name = "mySendMailTrigger";
            // schedule the job for execution
            sched.ScheduleJob(jobDetail, trigger2);

        }

        private void Scheduler()
        {
            ISchedulerFactory schedulePool = new StdSchedulerFactory();
            IScheduler sched = schedulePool.GetScheduler();
            sched.Start();

            //construct job info
            JobDetail jobDetail = new JobDetail("SendReminder", null, typeof(ReminderSender));

            //Set when to repeat the job
            Trigger trigger = TriggerUtils.MakeMinutelyTrigger(2, 3);
            trigger.StartTimeUtc = DateTime.UtcNow; 
            trigger.Name = "Testing";
            sched.ScheduleJob(jobDetail, trigger);
        }
    }
}
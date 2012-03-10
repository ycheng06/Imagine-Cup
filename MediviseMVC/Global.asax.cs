using System;
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
            JobDetail makeAlert = new JobDetail("AlertBuilder", null, typeof(TaskManager));

            //Set when to repeat the job
            Trigger trigger = TriggerUtils.MakeSecondlyTrigger(100);
            trigger.StartTimeUtc = DateTime.UtcNow; 
            trigger.Name = "Testing";
            sched.ScheduleJob(makeAlert, trigger);
        
            /* To be restored 
            CronExpression alert_first_reminder = new CronExpression("0 0 9 * * ?");

            Trigger alertBuilder = new CronTrigger("alertBuilder_first", "group1", "0 0 9,15 * * ?");// don't know what group1 is for
                                                                                               // cron expression is 9:00AM and 3:00AM every day
            alertBuilder.StartTimeUtc = DateTime.UtcNow;
            alertBuilder.Name = "Missed Response";
             * */
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using MediviseMVC.Models;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Security;

namespace MediviseMVC
{
    //!!!!!!!comment this debug method out for production
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MediviseService : DataService<MediviseEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            //!!!!!!comment out for production
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("Patients", EntitySetRights.All);
            config.SetEntitySetAccessRule("Alerts", EntitySetRights.All);
            config.SetEntitySetAccessRule("Drugs", EntitySetRights.All);
            config.SetEntitySetAccessRule("Tests", EntitySetRights.All);
            config.SetEntitySetAccessRule("Messages", EntitySetRights.All);
            config.SetEntitySetAccessRule("DrugInfos", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("TestInfos", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("AlertTypes", EntitySetRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        private IIdentity UserIdentity
        {
            get
            {
                string ticketValue = null;
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    //from cookie
                    ticketValue = cookie.Value;
                }
                else if (HttpContext.Current.Request.Headers["AuthToken"] != null)
                {
                    //from http header
                    ticketValue = HttpContext.Current.Request.Headers["AuthToken"];
                }

                if (!string.IsNullOrEmpty(ticketValue))
                {
                    try
                    {
                        var ticket = FormsAuthentication.Decrypt(ticketValue);
                        if (ticket != null)
                        {
                            return new FormsIdentity(ticket);
                        }
                    }
                    catch
                    { }
                }
                return null;
            }
        }

        [QueryInterceptor("Alerts")]
        public Expression<Func<Alert, bool>> AlertsFiler()
        {
            if(!HttpContext.Current.Request.IsAuthenticated)
                return (Alert a) => false;

            return (Alert a) => a.Patient.RegisteredBy == HttpContext.Current.User.Identity.Name;
        }


        [QueryInterceptor("Patients")]
        public Expression<Func<Patient, bool>> PatientsFilter()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return (Patient p) => false;

            var username = HttpContext.Current.User.Identity.Name;
            return (Patient p) => p.RegisteredBy == username;
        }

        [QueryInterceptor("Drugs")]
        public Expression<Func<Drug, bool>> DrugsFilter()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return (Drug d) => false;

            return (Drug d) => d.Patient.RegisteredBy == HttpContext.Current.User.Identity.Name;
        }

        [QueryInterceptor("Tests")]
        public Expression<Func<Test, bool>> TestsFilter()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return (Test t) => false;

            return (Test t) => t.Patient.RegisteredBy == HttpContext.Current.User.Identity.Name;
        }

        [QueryInterceptor("Messages")]
        public Expression<Func<Message, bool>> MessagesFilter()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return (Message m) => false;

            return (Message m) => m.Patient.RegisteredBy == HttpContext.Current.User.Identity.Name;
        }

        //protected override MediviseEntities CreateDataSource()
        //{
        //    var context = base.CreateDataSource();
        //    ((IObjectContextAdapter) context).ObjectContext.ContextOptions.ProxyCreationEnabled = false;
        //    return context;
        //}
    }
}

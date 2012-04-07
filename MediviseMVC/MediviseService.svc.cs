using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using MediviseMVC.Models;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Entity.Infrastructure;

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
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        protected override MediviseEntities CreateDataSource()
        {
            var context = base.CreateDataSource();
            ((IObjectContextAdapter) context).ObjectContext.ContextOptions.ProxyCreationEnabled = false;
            return context;
        }
    }
}

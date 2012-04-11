/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_ImagineCup@hotmail.com
 */
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using MediviseMVC.Models;
using System.Web;

namespace MediviseMVC.ActionFilters
{
    public class PreventUrlHacking : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MediviseEntities db = new MediviseEntities();
            int patientId = Convert.ToInt32(filterContext.ActionParameters["id"].ToString());
            Patient patient = db.Patients.Find(patientId);
            if (patient == null || !patient.IsRegisteredBy(filterContext.RequestContext.HttpContext.User.Identity.Name))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {{"controller", "PatientManager"},
                    {"action", "Index"}});
            }
        }
    }
}
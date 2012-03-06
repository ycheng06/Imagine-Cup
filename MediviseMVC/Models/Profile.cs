using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.ComponentModel.DataAnnotations;

namespace MediviseMVC.Models
{
    public class Profile : ProfileBase
    {
       [Display(Name = "Time Zone")]
        public string TimeZone
        {
            get
            {
                return (this.GetPropertyValue("TimeZone").ToString());
            }
            set{
                this.SetPropertyValue("TimeZone", value);
            }
        }

       public static Profile GetProfile(string username)
       {
           return Create(username) as Profile;
       }

    }
}
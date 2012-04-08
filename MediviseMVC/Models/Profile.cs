/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

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
           return (Profile)Create(username);
       }

       public static Profile GetCurrent()
       {
           return (Profile)Create(Membership.GetUser().UserName);
       }

    }
}
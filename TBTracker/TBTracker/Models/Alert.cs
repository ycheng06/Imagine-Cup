using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace TBTracker.Models
{
    public class Alert
    {
        public int AlertId { get; set; }
        public DateTime AlertDate { get; set; }
        public string AlertType { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        //not sure why this is neccessary, will find out
    }
}
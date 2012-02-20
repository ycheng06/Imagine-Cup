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
    public class Message
    {
        public int MessageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reminder { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
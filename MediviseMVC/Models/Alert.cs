/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using System.Data.Services.Common;

namespace MediviseMVC.Models
{
    [DataServiceKey("AlertId")]
    public class Alert
    {
        public int AlertId { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime AlertDate { get; set; }

        public int AlertTypeId { get; set; } //defined in table AlertTypes: 1 = missed medication, 2 = missed checkup
        public virtual AlertType AlertType { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
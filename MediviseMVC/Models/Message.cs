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
using MediviseMVC.Validation;

namespace MediviseMVC.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DateGreaterThanAttribute("StartDate")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage="Message is blank")]
        public string MessageText { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
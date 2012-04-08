/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MediviseMVC.Validation;

namespace MediviseMVC.Models
{
    public class Drug
    {
        public int DrugId { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DateGreaterThanAttribute("StartDate")]
        [Required]
        public DateTime EndDate { get; set; }
        public int TimesPerWeek { get; set; }
        public string Type { get; set; }   //unused for now

        [Required]
        public bool Monday { get; set; } //whether the drug is needed this day
        [Required]
        public bool Tuesday { get; set; }
        [Required]
        public bool Wednesday { get; set; }
        [Required]
        public bool Thursday { get; set; }
        [Required]
        public bool Friday { get; set; }
        [Required]
        public bool Saturday { get; set; }
        [Required]
        public bool Sunday { get; set; }

        public int DrugInfoId { get; set; }
        public virtual DrugInfo DrugInfo { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }

    public class DrugInfo
    {
        public int DrugInfoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
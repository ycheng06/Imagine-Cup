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
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DateGreaterThanAttribute("StartDate")]
        public DateTime EndDate { get; set; }
        public int TimesPerWeek { get; set; }
        public string Type { get; set; }   //unused for now

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
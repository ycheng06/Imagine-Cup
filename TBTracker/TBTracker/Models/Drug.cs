using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TBTracker.Models
{
    public class Drug
    {
        public int DrugId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeZone { get; set; }
        public int TimesPerWeek { get; set; }
        public string Type { get; set; }

        public int DrugInfoId { get; set; }
        public DrugInfo DrugInfo { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }

    public class DrugInfo
    {
        public int DrugInfoId { get; set; }
        public string Name { get; set; }
        public string Descirption { get; set; }
    }
}
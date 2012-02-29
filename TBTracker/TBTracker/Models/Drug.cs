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
        public int Timezone { get; set; }   //timezone in relation to +0
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
        public virtual ICollection<Drug> Drugs { get; set; }
    }
}
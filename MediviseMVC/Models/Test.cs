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
    [DataServiceKey("TestId")]
    public class Test
    {
        public int TestId { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime TestDate { get; set; }
        public string TestResult { get; set; }

        public int TestInfoId { get; set; }
        public virtual TestInfo TestInfo { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }

    [DataServiceKey("TestInfoId")]
    public class TestInfo
    {
        public int TestInfoId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
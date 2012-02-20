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
    public class Test
    {
        public int TestId { get; set; }
        public DateTime TestDate { get; set; }
        public string TestResult { get; set; }

        public int TestInfoId { get; set; }
        public TestInfo TestInfo { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }

    public class TestInfo
    {
        public int TestInfoId { get; set; }
        public string Name { get; set; }
    }
}
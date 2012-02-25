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
    public class AlertType
    {
        public int AlertTypeId { get; set; }
        public string Name { get; set; }
    }
}
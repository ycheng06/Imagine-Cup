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
using DataAnnotationsExtensions;
using System.Data.Entity;
using MediviseMVC.Validation;
using System.Data.Services.Common;

namespace MediviseMVC.Models
{
    [DataServiceKey("PatientId")]
    public class Patient
    {
        public int PatientId { get; set; }
        public string Gender { get; set; } 
        
        [Required(ErrorMessage="First/Last Name is Required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="First/Last name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
       
        [Required(ErrorMessage="Phone Number is required")]
        [Digits]
        [StringLength(10)]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
      
        [Required(ErrorMessage="At least one family number is required")]
        [Digits]
        [StringLength(10)]
        [DisplayName("Family Phone #1")]
        public string FamilyPhone1 { get; set; }
       
        [DisplayName("Family Phone #2")] 
        [Digits]
        [StringLength(10)]
        public string FamilyPhone2 { get; set; }
        
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TreatmentStartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DateGreaterThanAttribute("TreatmentStartDate")]
        public DateTime TreatmentEndDate { get; set; }
        [Required]
        public string TimeZone { get; set; }

        public string RegisteredBy { get; set; } //which hospital "owns" this patient

        public bool ResponseReceived { get; set; } //confirmation received from patient for the day?

        public virtual ICollection<Drug> Drugs { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Alert> Alerts { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public bool IsRegistedBy(string userName)
        {
            return RegisteredBy.Equals(userName);
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using System.Data.Entity;

namespace TBTracker.Models
{
    /*
    public enum Gender
    {
        Female = 0,
        Male = 1,
        NotSpecified = -1
    }
    */
    public class Patient
    {
        public int PatientId { get; set; }
        public string Gender { get; set; } //change this to a simpler representation, like int, when you figure out how to make it work
        
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
        public string TimeZone { get; set; }

        public string RegisteredBy { get; set; }

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
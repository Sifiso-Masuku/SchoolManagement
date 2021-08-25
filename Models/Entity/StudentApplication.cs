using System;
using IdentitySample.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using SchoolManagement.Model.Entity;

namespace SchoolManagement.Model.Entity
{
    public class StudentApplication
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Students's Full Name(s)")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Students's Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Date fo Birth is required"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DOB { get; set; }
        [Required,DisplayName("National Identinty Number")]
        public string NID { get; set; }
        [Required(ErrorMessage = "You must select a gender")]

        public string Gender { get; set; }
        [Required(ErrorMessage = "You must select your Home Language")]
        public string HomeLanguage { get; set; }
        public string Race { get; set; }
        [Required(ErrorMessage = "Required!")]
        [EmailAddress]
        public string Email { get; set; }
        public string GuardianEmail { get; set; }
        public string GuardianName { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        [Required(ErrorMessage = "Required!")]
        [Display(Name = "Parmanent Address")]
        public string ParmanentAddress { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string Religion { get; set; }
        public string creator { get; set; }
        public string StudentNumber { get; set; }

        [DisplayName("Class Applying for")]
        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
        public string Status { get; set; }
        public string subject { get; set; }

        public ICollection<ApplicationDocuments> applicationDocuments { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public int studentapp { get; set; }

        [Display(Name = "Students's Name")]
        public string Name { get; set; }
      
        [Display(Name = "Students's Surname")]
        public string Surname { get; set; }
 
        [Display(Name = "Date of Birth")]       
        [Required(ErrorMessage = "Date fo Birth is required"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
       
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }      
        public decimal deposit { get; set; }
        public decimal Tuition { get; set; }
        public decimal BalanceDue { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string NID { get; set; }
        public string subject { get; set; }
        public string Grade { get; set; }
        public string StudentNumber { get; set; }
        public string GuardianName { get; set; }
        public string Status { get; set; }
        public string date { get; set; }


        //public virtual ICollection<Guardian> Guardians { get; set; }
        //public virtual ICollection<Registration> Registrations { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        [Display(Name = "National ID Number")]
        public string NID { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Home Address")]
        public string HomeAddress { get; set; }
        public ICollection<AssignTeacheToClass> assignTeacheToClasses { get; set; }
        public ICollection<WorkSchedule> workSchedules { get; set; }

    }
}
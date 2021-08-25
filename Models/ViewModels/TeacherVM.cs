
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Model.Enum;

namespace SchoolManagement.Model.ViewModels
{
    public class TeacherVM
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Gender Gender { get; set; }

        [Display(Name = "National ID Number")]
        public string NID { get; set; }
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Home Address")]
        public string HomeAddress { get; set; }
    }
}
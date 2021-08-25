using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Model.Enum;
using SchoolManagement.Model.Entity;
using IdentitySample.Models;
using System.ComponentModel;

namespace SchoolManagement.Model.ViewModels
{
    public class StudentApplicationVM
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

        public Gender Gender { get; set; }
        [Required(ErrorMessage = "You must select your Home Language")]
        public HomeLanguage HomeLanguage { get; set; }
        public Race Race { get; set; }
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
        public Religion Religion { get; set; }
        public string creator { get; set; }
        public string StudentNumber { get; set; }

       
       
        

        
        [DisplayName("Class Applying for")]
        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
        public string Status { get; set; }
        public string subject { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        public string generateStudenbtNumber(string id)
        {
            string year = Convert.ToString(System.DateTime.Now.Year);
            string result = year.Substring(0,1) + year.Substring(2) + id.Substring(8);

            return result;
        }

        public string getSubject()
        {
            var sub = (from s in db.Subjects
                       where s.ClassNameId == ClassNameId
                       select s.Name).FirstOrDefault();
            return sub;
        }
        public decimal getPrice(string type, decimal priceId)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var price = (from c in _context.FeeTypes
                         where c.Id == priceId && c.Name == type
                         select c.FeeAmount).FirstOrDefault();
            return price;
        }
    }
}
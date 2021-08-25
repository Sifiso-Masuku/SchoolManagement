using SchoolManagement.Model.Entity;
using IdentitySample.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class ClassFee
    {
        [Key]
        public int Id { get; set; }
        public  int FeeTypeId { get; set; }
        public FeeType FeeType { get; set; }
        [Required]
        [Display(Name = "Class")]
        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
    }
}
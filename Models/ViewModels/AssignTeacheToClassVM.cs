
using SchoolManagement.Model.Entity;
using System.ComponentModel.DataAnnotations;
using IdentitySample.Models;

namespace SchoolManagement.Model.ViewModels
{
    public class AssignTeacheToClassVM
    {
      [Key]
        public int TeacherClassId { get; set; }
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public bool CheckExists()
        {
            bool result = false;
            var dbRecord = db.AssignTeacheToClasses;
            foreach (var item in dbRecord)
            {
                if(item.TeacherId==TeacherId && item.ClassNameId == ClassNameId)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
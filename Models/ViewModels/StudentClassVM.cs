
using System.Linq;
using SchoolManagement.Model.Entity;
using IdentitySample.Models;

namespace SchoolManagement.Model.ViewModels
{
    public class StudentClassVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
      
        public int SectionId { get; set; }
        public Section Section { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetName()
        {
            var f = (from c in db.ClassNames
                     where ClassNameId == c.Id
                     select c.Name).FirstOrDefault();
            return f ;
        }
    }

}
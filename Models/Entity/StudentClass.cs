using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class StudentClass
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
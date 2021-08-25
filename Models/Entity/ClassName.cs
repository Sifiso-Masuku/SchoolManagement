using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class ClassName
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Class Name")]
        public string Name { get; set; }

        public ICollection<ClassFee> classFees { get; set; }
        public ICollection<Subject> subjects { get; set; }
        public ICollection<AssignTeacheToClass> assignTeacheToClasses { get; set; }
        public ICollection<StudentClass> studentClasses { get; set; }
        public ICollection<WorkSchedule> workSchedules { get; set; }
    }
}
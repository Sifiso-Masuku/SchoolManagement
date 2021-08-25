using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class Section
    {
     [Key]
        public int Id { get; set; }

        [Required,DisplayName("Section")]
        public string Name { get; set; }
        public ICollection<StudentClass> studentClasses { get; set; }

    }
}
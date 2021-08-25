using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model.Entity
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required,DisplayName("Subject Name")]
        public string Name { get; set; }

        [Required, DisplayName("Subject Code")]
        public string Code { get; set; }
        public double Subject_Credit { get; set; }

        public string SubjectAssignTo { get; set; }
        public int Theory { get; set; }
        public int Mcq { get; set; }
        public int Practical { get; set; }
        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
        
    }
}
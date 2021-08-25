using Microsoft.Ajax.Utilities;
using SchoolManagement.Model;
using SchoolManagement.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model.Entity
{
    public class AssignTeacheToClass
    {
      [Key]
        public int TeacherClassId { get; set; }
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int ClassNameId { get; set; }
        public virtual ClassName ClassName { get; set; }
    }
}
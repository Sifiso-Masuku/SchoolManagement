using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model.Entity
{
    public class GuardianType
    {
        [Key]
        public int Id { get; set; }

        [Required,DisplayName("Guardian Type Name")]
        public string Name { get; set; }
    }
}
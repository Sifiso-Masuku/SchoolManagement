using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class ResidenceType
    {
        [Key]
        public int ResidenceTypeId { get; set; }
        [DisplayName("Resdence Type Name"),Required]
        public string ResidenceTypeName { get; set; }
    }
}
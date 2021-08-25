using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model.Entity
{
    public class FeeType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Fee Type ")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Fee Amount")]
        [DataType(DataType.Currency)]
        public decimal FeeAmount { get; set; }
        
    }
}
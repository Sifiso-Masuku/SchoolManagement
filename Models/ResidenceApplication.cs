using SchoolManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class ResidenceApplication
    {
        [Key]
        public int ResidenceApplicationId { get; set; }
        public int ResidenceId { get; set; }
        public Residence Residence { get; set; }
        public string UeserEmail { get; set; }
        [DisplayName("DateApplied"),DataType(DataType.Date)]
        public DateTime DateApplied { get; set; }
        [DisplayName("Registration Fee")]
        public decimal RegistrationFee { get; set; }
        public string Status { get; set; }

    }
}
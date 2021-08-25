using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models.Entity
{
    public class Residence
    {
        public int ID { get; set; }
        public int ResidenceTypeId { get; set; }
        public virtual ResidenceType ResidenceType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Rooms { get; set; }
        [DisplayName("Anual Fee"),Required, DataType(DataType.Currency)]
        public decimal AnualFee { get; set; }
        [DisplayName("Registration Fee"), DataType(DataType.Currency)]

        public decimal RegistrationFee { get; set; }

        public decimal CalacRegistrationFee()
        {
            return (AnualFee - (AnualFee * 0.15M));
        }
    }
}
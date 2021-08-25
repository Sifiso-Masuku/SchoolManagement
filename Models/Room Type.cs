using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace SchoolManagement.Models
{
    public class Room_Type
    {
        [Key]
        public int Room_TypeId { get; set; }
        [DisplayName("Room Type Name"),Required]
        public string RtName { get; set; }
        [DisplayName("Number Of Accupance"),Required]
        public int No_Occupants { get; set; }
    }
}
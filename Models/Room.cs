using SchoolManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public int Room_TypeId { get; set; }
        public virtual Room_Type Room_Type { get; set; }
        public int ResidenceId { get; set; }
        public virtual Residence Residence { get; set; }
        [DisplayName("Room Number")]
        public string RoomNumber { get; set; }
        public string Status { get; set; }

    }
}
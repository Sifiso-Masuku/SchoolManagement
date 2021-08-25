using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models.Entity
{
    public class ResidenceStudent
    {
        public int ID { get; set; }
        public int ResidenceID { get; set; }
        public int StudentID { get; set; }
    }
}
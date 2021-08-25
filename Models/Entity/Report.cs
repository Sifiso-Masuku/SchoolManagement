using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolManagement.Model.Entity;

namespace SchoolManagement.Models.Entity
{
    public class Report
    {
        public int ID { get; set; }
        public Student student { get; set; }
        public int StudentID { get; set; }

        public string Term { get; set; }
        public decimal Mark { get; set; }

        public string Subject { get; set; }
    }
}
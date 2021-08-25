using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models.Entity
{
    public class Meeting
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
    }
}
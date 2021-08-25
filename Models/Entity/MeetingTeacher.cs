using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models.Entity
{
    public class MeetingTeacher
    {
        public int ID { get; set; }
        public int TeacherID { get; set; }
        public int MeetingID { get; set; }
    }
}
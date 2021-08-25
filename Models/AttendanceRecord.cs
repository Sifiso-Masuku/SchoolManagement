using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public int NumberAbsent { get; set; }
        public int NumberPresent { get; set; }
        public string GuardianEmail { get; set; }
        public string Attendance_Date { get; set; }
        public string Attendance_status { get; set; }
    }
}
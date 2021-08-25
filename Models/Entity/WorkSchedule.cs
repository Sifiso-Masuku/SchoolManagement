using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class WorkSchedule
    {
        [Key]
        public int scheduleId { get; set; }
        public DateTime scheduleStartDate { get; set; }
        public DateTime scheduleEndDate { get; set; }
        public string ThemeColor { get; set; }
        public bool archived { get; set; }
        public int classRoomId { get; set; }
        public virtual ClassName ClassName { get; set; }
        public int staffMemberId { get; set; }
        public virtual Teacher teachers { get; set; }

    }
}
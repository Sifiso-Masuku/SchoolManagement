using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SchoolManagement.Model.Entity;
using IdentitySample.Models;

namespace SchoolManagement.Models.ViewModels
{
    public class resourceModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //classroom details
        public int id { get; set; }
        public int resourceId { get; set; }
        public string title { get; set; }
        public string groupId { get; set; }
        public int roomNumber { get; set; }
        public string GradeName { get; set; }
        public string Description { get; set; }
        //Staff details
        // public string title { get; set; }
        public string StaffMemberName { get; set; }
        public string StaffMemberSurname { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public DateTime dateOfBirth { get; set; }

        //schedule Model
        public int staffMemberId { get; set; }
        public int classRoomId { get; set; }
        public int scheduleId { get; set; }
        public DateTime scheduleStartDate { get; set; }
        public DateTime? scheduleEndDate { get; set; }
        public string ThemeColor { get; set; }

        public List<resourceModel> GetworkSchedules()
        {

            //DataContext db = new DataContext();

            db.Configuration.ProxyCreationEnabled = false;
            var workSchedules = db.WorkSchedule.Where(m => m.archived == false).ToList();
            //Tempory holder for resrouces
            List<resourceModel> resList = new List<resourceModel>();


            foreach (var wrks in workSchedules)
            {
                //Finds Employee Linked to Event
                var tempStaff = new  Teacher();
                tempStaff = db.Teachers.Where(c => c.Id == wrks.staffMemberId).FirstOrDefault();
                var grd = db.StudentClasses.Where(v => v.Id == wrks.classRoomId).Select(m => m.Name).FirstOrDefault();
                //Saves All information about event to temporary result model
                resourceModel res = new resourceModel();

                res.title = grd;
                //res.id
                res.scheduleId = wrks.scheduleId;
                res.staffMemberId = wrks.staffMemberId;
                res.classRoomId = wrks.classRoomId;
                res.GradeName = grd;
                res.StaffMemberName = tempStaff.Name;
                res.StaffMemberSurname = tempStaff.Surname;
                res.email = tempStaff.Email;
                res.groupId = "Teacher";
                res.phonenumber = tempStaff.PhoneNumber;
                res.resourceId = tempStaff.Id;
                res.scheduleStartDate = wrks.scheduleStartDate;
                res.scheduleEndDate = wrks.scheduleEndDate;
                res.ThemeColor = wrks.ThemeColor;
                res.Description = tempStaff.Name + " " + tempStaff.Surname;
                resList.Add(res);
            }
            return resList;
        }
        public List<resourceModel> GetResources()
        {

            db.Configuration.ProxyCreationEnabled = false;
            var resources = db.Teachers.ToList();

            //Tempory holder for resrouces
            List<resourceModel> resList = new List<resourceModel>();


            foreach (var stf in resources)
            {
                resourceModel res = new resourceModel();
                res.title = stf.Name + " " + stf.Surname;
                res.staffMemberId = stf.Id;
                res.resourceId = stf.Id;
                res.phonenumber = stf.PhoneNumber;
                res.email = stf.Email;
                //res.dateOfBirth = stf.dateOfBirth;
                res.groupId = "Teacher";
                res.id = stf.Id;
                resList.Add(res);

            }
            return resList;
        }
        public List<SchoolManagement.Model.Entity.StudentClass> GetClassroms()
        {

            //DataContext db = new DataContext();

            //ApplicationDbContext db = new ApplicationDbContext();

            db.Configuration.ProxyCreationEnabled = false;
            var workSchedules = db.StudentClasses.ToList();
            //Tempory holder for resrouces
            List<SchoolManagement.Model.Entity.StudentClass> resList = new List<SchoolManagement.Model.Entity.StudentClass>();


            foreach (var wrks in workSchedules)
            {
                //Finds Employee Linked to Event


                //Saves All information about event to temporary result model
             var res = new Model.Entity.StudentClass();

                res.Name = wrks.Name;

                res.Id = wrks.Id;
                res.ClassName = wrks.ClassName;

                //TimeSpan? timeDif = res.scheduleEndDate - res.scheduleStartDate;
                //res.Description = tempEmp.Occupation + ", works " + timeDif.Value.TotalHours + " hours today.";

                resList.Add(res);
            }
            //return WorkSchedulerepo.GetAll().Select(x => new ClinicView() { ClinicId = x.ClinicId, ClinicName = x.ClinicName }).ToList();
            return resList;
        }

        public List<ThemeColor> GetThemeColors()
        {

            //DataContext db = new DataContext();

            //ApplicationDbContext db = new ApplicationDbContext();

            db.Configuration.ProxyCreationEnabled = false;
            var workSchedules = db.colors.ToList();
            //Tempory holder for resrouces
            List<ThemeColor> resList = new List<ThemeColor>();


            foreach (var wrks in workSchedules)
            {
                //Finds Employee Linked to Event


                //Saves All information about event to temporary result model
               ThemeColor res = new ThemeColor();

                res.colorName = wrks.colorName;


                resList.Add(res);
            }
            //return WorkSchedulerepo.GetAll().Select(x => new ClinicView() { ClinicId = x.ClinicId, ClinicName = x.ClinicName }).ToList();
            return resList;
        }

        public SchoolManagement.Models.ViewModels.schedulelist checkWorkSchedule(WorkSchedule wrks)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var status = false;
            wrks.scheduleStartDate = new DateTime(wrks.scheduleStartDate.Year, wrks.scheduleStartDate.Month, wrks.scheduleStartDate.Day, wrks.scheduleStartDate.Hour + 2, wrks.scheduleStartDate.Minute, 0);
            wrks.scheduleEndDate = new DateTime(wrks.scheduleEndDate.Year, wrks.scheduleEndDate.Month, wrks.scheduleEndDate.Day, wrks.scheduleEndDate.Hour + 2, wrks.scheduleEndDate.Minute, 0);

            var ClassSchedule = db.WorkSchedule.Where(a => a.classRoomId == wrks.classRoomId && (a.archived == false) && ((a.scheduleStartDate <= wrks.scheduleStartDate && (a.scheduleStartDate.Hour <= wrks.scheduleStartDate.Hour && a.scheduleEndDate.Hour >= wrks.scheduleStartDate.Hour) && wrks.scheduleStartDate < a.scheduleEndDate))).Select(h => h.scheduleId).FirstOrDefault();

            var sfaName = "";
            if (ClassSchedule == 0)
            {
                //SaveWorkSchedule(wrks);

                status = true;
            }
            else
            {
                sfaName = db.Teachers.Where(a => a.Id == wrks.staffMemberId).Select(h => h.Name).FirstOrDefault();
                status = false;
            }
            schedulelist obj = new schedulelist();
            obj.ClassSchedule = ClassSchedule;
            obj.status = status;
            obj.sfaName = sfaName;
            return obj;
        }

        public bool saveWork(WorkSchedule wrks)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var status = false;
            wrks.scheduleStartDate = new DateTime(wrks.scheduleStartDate.Year, wrks.scheduleStartDate.Month, wrks.scheduleStartDate.Day, wrks.scheduleStartDate.Hour + 2, wrks.scheduleStartDate.Minute, 0);
            wrks.scheduleEndDate = new DateTime(wrks.scheduleEndDate.Year, wrks.scheduleEndDate.Month, wrks.scheduleEndDate.Day, wrks.scheduleEndDate.Hour + 2, wrks.scheduleEndDate.Minute, 0);
            //If the event ID is bigger than zero its a existing event.
            if (wrks.scheduleId > 0)
            {
                //Grabs event with given ID from the database
                var oldwrks = db.WorkSchedule.Where(a => a.scheduleId == wrks.scheduleId).FirstOrDefault();

                if (oldwrks != null)
                {
                    //Replaces fields that has been updated
                    //oldEvent. = wrks.EventID;
                    oldwrks.scheduleId = wrks.scheduleId;
                    oldwrks.staffMemberId = wrks.staffMemberId;
                    oldwrks.classRoomId = wrks.classRoomId;
                    oldwrks.scheduleStartDate = wrks.scheduleStartDate;
                    oldwrks.scheduleEndDate = wrks.scheduleEndDate;
                    // oldEvent.IsFullDay = wrks.IsFullDay;
                    //oldEvent.Description = wrks.Description;
                    oldwrks.ThemeColor = wrks.ThemeColor;
                    db.Entry(oldwrks).State = EntityState.Modified;
                }
            }
            //If a new event is added, it just adds the new event to DB
            else
            {
                var workSchedule = new WorkSchedule();
                workSchedule.archived = wrks.archived;
                workSchedule.classRoomId = wrks.classRoomId;
                workSchedule.scheduleEndDate = wrks.scheduleEndDate;
                workSchedule.scheduleId = wrks.scheduleId;
                workSchedule.scheduleStartDate = wrks.scheduleStartDate;
                workSchedule.ThemeColor = wrks.ThemeColor;
                workSchedule.staffMemberId = wrks.staffMemberId;
                db.WorkSchedule.Add(workSchedule);
            }

            db.SaveChanges();
            status = true;
            return (status);
        }

    }
}
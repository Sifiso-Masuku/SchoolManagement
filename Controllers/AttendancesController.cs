using IdentitySample.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SchoolManagement.Model.Entity;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class AttendancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attendances
        public ActionResult Index(string search)
        {
            var username = User.Identity.GetUserName();

            var teacherId = db.Teachers.Where(x => x.Email == username).Select(x => x.Id).FirstOrDefault();
            var grade = db.AssignTeacheToClasses.Where(x => x.TeacherId == teacherId).Select(x => x.ClassName.Name).FirstOrDefault();
             //var list = db.Attendances.Where(x => x.Grade == grade.ToString() && x.Subject.Contains(search)).ToList();
             var list = db.Attendances.Where(x => x.Grade == grade.ToString()).ToList();

            return View(list);
        }

        
        public ActionResult Index1()
        {
            var username = User.Identity.GetUserName();

            var teacherId = db.Teachers.Where(x => x.Email == username).Select(x => x.Id).FirstOrDefault();
            var grade = db.AssignTeacheToClasses.Where(x => x.TeacherId == teacherId).Select(x => x.ClassName.Name).FirstOrDefault();

            return View(db.Attendances.ToList().Where(x => x.Grade == grade.ToString()));
        }
        public ActionResult Index5()
        {
            var username = User.Identity.GetUserName();
            return View(db.Attendances.ToList().Where(x => x.GuardianEmail == username));
        }
        public ActionResult Present(int? id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if(attendance.Attendance_Date == System.DateTime.Now.Date.ToString() && attendance.Grade == attendance.Grade && attendance.Id == id || attendance.Attendance_status == "Absent")
            {
                TempData["AlertMessage"] = "You already marked the student present";
                return RedirectToAction("Index", new { search = attendance.Subject });
            }
            else
            {
                //AttendanceRecord attendanceRecord = new AttendanceRecord();
                //attendanceRecord.StudentName = attendance.StudentName;
                //attendanceRecord.StudentSurname = attendance.StudentSurname;
                //attendanceRecord.Subject = attendance.Subject;
                //attendanceRecord.Grade = attendance.Grade;
                //attendanceRecord.NumberAbsent = attendance.NumberAbsent;
                //attendanceRecord.NumberPresent = attendance.NumberPresent;
                //attendanceRecord.GuardianEmail = attendance.GuardianEmail;
                //attendanceRecord.Attendance_Date = System.DateTime.Now.Date.ToString();
                //attendanceRecord.Attendance_status = "Present";

                //db.Entry(attendanceRecord).State = EntityState.Modified;
                //db.SaveChanges();
                attendance.Attendance_status = "Present";
                attendance.Attendance_Date = System.DateTime.Now.Date.ToString();
                attendance.NumberPresent += 1;
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();

           

                return RedirectToAction("Index", new { search = attendance.Subject });
            }
     
        }
        public ActionResult Absent(int? id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if (attendance.Attendance_Date == System.DateTime.Now.Date.ToString() && attendance.Grade == attendance.Grade && attendance.Id == id || attendance.Attendance_status == "Present")
            {
                TempData["AlertMessage"] = "You already marked the student absent";
                return RedirectToAction("Index", new { search = attendance.Subject });
            }
            else
            {
                attendance.Attendance_status = "Absent";
                attendance.Attendance_Date = System.DateTime.Now.Date.ToString();
                attendance.NumberAbsent += 1;
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();

                //AttendanceRecord attendanceRecord = new AttendanceRecord();
                //attendanceRecord.StudentName = attendance.StudentName;
                //attendanceRecord.StudentSurname = attendance.StudentSurname;
                //attendanceRecord.Subject = attendance.Subject;
                //attendanceRecord.Grade = attendance.Grade;
                //attendanceRecord.NumberAbsent = attendance.NumberAbsent;
                //attendanceRecord.NumberPresent = attendance.NumberPresent;
                //attendanceRecord.GuardianEmail = attendance.GuardianEmail;
                //attendanceRecord.Attendance_Date = attendance.Attendance_Date;
                //attendanceRecord.Attendance_status = attendance.Attendance_status;
                //db.Entry(attendanceRecord).State = EntityState.Modified;
                //db.SaveChanges();

                return RedirectToAction("Index", new { search = attendance.Subject });
            }
        }
        //public ActionResult IncreaseDate(int? id)
        //{
        //    Attendance attendance = db.Attendances.Find(id);
            
        //        attendance.Attendance_Date = (System.DateTime.Now.Date + System.DateTime.Now.Date.ToString()).ToString();
        //        db.Entry(attendance).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index", new { search = attendance.Subject });
        //}
        public ActionResult EmailstudentInfor(int? id)
        {
            Attendance attendance = db.Attendances.Find(id);
            var username = User.Identity.GetUserName();
            var name = db.Guardians.Where(x => x.Email == attendance.GuardianEmail).FirstOrDefault();
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(attendance.GuardianEmail, name.Name));
            var body = $"Hello {name.Name}, here are the attendance details for {attendance.StudentName} {attendance.StudentSurname} <br/> Class: {attendance.Grade}<br/> Subject: {attendance.Subject} <br/> He/she has been absent for {attendance.NumberAbsent} days and Present for {attendance.NumberPresent}" +
                $" <br/> Regards,<br/><br/> Bright Ideas <br/> .";

            SchoolManagement.Models.EmailService emailService = new SchoolManagement.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + attendance.Id,
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Bright Ideas</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            }); ;

            return RedirectToAction("Index1");
        }
        public ActionResult Index2()
        {
            return View(db.Attendances.ToList());
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentName,StudentSurname,Subject,Grade,NumberAbsent,NumberPresent,GuardianEmail,Attendance_Date,Attendance_status")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { search = attendance.Subject });
            }
            return RedirectToAction("Index", new { search = attendance.Subject });
        }
    }
}

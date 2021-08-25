using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using SchoolManagement.Model.Entity;
using SchoolManagement.Models.Entity;

namespace SchoolManagement.Controllers
{
    public class MeetingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Meetings
        public ActionResult Index()
        {
            return View(db.meetings.ToList());
        }

        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.MeetingID = id;
            Meeting meeting = db.meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        [HttpGet]
        public JsonResult addTeacher(int meetingid,int teacherid)
        {
            MeetingTeacher mt = new MeetingTeacher();
            mt.MeetingID = meetingid;
            mt.TeacherID = teacherid;

            db.meetingTeachers.Add(mt);
            db.SaveChanges();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getAllTeachers()
        {
            List<Teacher> teachers = db.Teachers.ToList();

            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getTeachersInMeeting(int id)
        {
            List<MeetingTeacher> meetingteacher = db.meetingTeachers.Where(x => x.MeetingID == id).ToList();
            List<Teacher> teachers = new List<Teacher>();

            db.Teachers.ToList().ForEach(item =>
            {
                meetingteacher.ForEach(item2 =>
                {
                    if(item.Id == item2.TeacherID)
                    {
                        teachers.Add(item);
                    }
                });
            });

            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        // GET: Meetings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Date,Time,Venue")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                db.meetings.Add(meeting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,Time,Venue")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meeting meeting = db.meetings.Find(id);
            db.meetings.Remove(meeting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

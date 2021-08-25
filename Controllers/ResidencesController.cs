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
    public class ResidencesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Residences
        public ActionResult Index()
        {
            return View(db.Residences.ToList());
        }
        public ActionResult ResidenceView(int id)
        {
            return View(db.Residences.ToList());
        }

        // GET: Residences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Residence residence = db.Residences.Find(id);
            ViewBag.ID = residence.ID;
            if (residence == null)
            {
                return HttpNotFound();
            }
            return View(residence);
        }

        [HttpGet]
        public JsonResult getAllStudents()
        {
            return Json(db.Students.Where(x => x.Status == "Paid Full Amount").ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getAllStudentsInRes(int id)
        {
            List<Student> students = new List<Student>();

            List<ResidenceStudent> studentRes = db.residenceStudents.Where(x => x.ResidenceID == id).ToList();

            studentRes.ForEach(st =>
            {
                db.Students.Where(x => x.Status == "Paid Full Amount").ToList().ForEach(s =>
                  {
                      if(st.StudentID == s.Id)
                      {
                          students.Add(s);
                      }
                  });
            });

            return Json(students, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult addStudentToRes(int studentid,int resid)
        {
            ResidenceStudent rs = new ResidenceStudent();
            rs.StudentID = studentid;
            rs.ResidenceID = resid;

            db.residenceStudents.Add(rs);
            db.SaveChanges();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        // GET: Residences/Create
        public ActionResult Create()
        {
            ViewBag.ResidenceTypeId = new SelectList(db.ResidenceTypes, "ResidenceTypeId", "ResidenceTypeName");
            return View();
        }

        // POST: Residences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,Rooms,ResidenceTypeId,AnualFee,RegistrationFee")] Residence residence)
        {
            if (ModelState.IsValid)
            {
                residence.RegistrationFee = residence.CalacRegistrationFee();
                db.Residences.Add(residence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResidenceTypeId = new SelectList(db.ResidenceTypes, "ResidenceTypeId", "ResidenceTypeName", residence.ResidenceTypeId);
            return View(residence);
        }

        // GET: Residences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Residence residence = db.Residences.Find(id);
            if (residence == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResidenceTypeId = new SelectList(db.ResidenceTypes, "ResidenceTypeId", "ResidenceTypeName");
            return View(residence);
        }

        // POST: Residences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,Rooms,ResidenceTypeId,AnualFee,RegistrationFee")] Residence residence)
        {
            if (ModelState.IsValid)
            {
                residence.RegistrationFee = residence.CalacRegistrationFee();
                db.Entry(residence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResidenceTypeId = new SelectList(db.ResidenceTypes, "ResidenceTypeId", "ResidenceTypeName", residence.ResidenceTypeId);
            return View(residence);
        }

        // GET: Residences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Residence residence = db.Residences.Find(id);
            if (residence == null)
            {
                return HttpNotFound();
            }
            return View(residence);
        }

        // POST: Residences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Residence residence = db.Residences.Find(id);
            db.Residences.Remove(residence);
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
